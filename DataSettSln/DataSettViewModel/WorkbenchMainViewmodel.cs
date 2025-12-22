using DataSett.Metamodel;
using DataSett.ViewModel.Services;

using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataSett.ViewModel
{
    public class WorkbenchMainViewmodel : INotifyPropertyChanged
    {
        public WorkbenchMainViewmodel(IMetaDataIOService metaDataIOService, IOptions<AppSettings> appSettings)
        {
            MetaDataIOService = metaDataIOService;

            // Set standard values for properties:
            _sourceSystems = new ObservableCollection<SourceSystem>();
            _businessConcepts = new ObservableCollection<BusinessConcept>();
            _serverPath = appSettings.Value.RepositoryPath ?? string.Empty;
            _businessDomains = new ObservableCollection<BusinessDomain>();
            _temporaryBusinessConceptMappings = new List<BusinessConceptMapping>();
        }

        private IMetaDataIOService MetaDataIOService { get; set; }

        // Properties for binding to the view:
        private ObservableCollection<SourceSystem> _sourceSystems;
        public ObservableCollection<SourceSystem> SourceSystems => _sourceSystems;


        private ObservableCollection<BusinessConcept> _businessConcepts;
        public ObservableCollection<BusinessConcept> BusinessConcepts => _businessConcepts;

        private SourceSystem? _selectedSourceSystem;
        public SourceSystem? SelectedSourceSystem
        {
            get => _selectedSourceSystem;
            set
            {
                if (_selectedSourceSystem != value)
                {
                    _selectedSourceSystem = value;
                    OnPropertyChanged();
                }
            }
        }

        private SourceInterface? _selectedSourceInterface;
        public SourceInterface? SelectedSourceInterface
        {
            get => _selectedSourceInterface;
            set
            {
                if (_selectedSourceInterface != value)
                {
                    ApplyNewBCMappingsToDataSett();
                    _selectedSourceInterface = value;
                    FilterBusinessConceptMappings();
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<BusinessDomain> _businessDomains;
        public ObservableCollection<BusinessDomain> BusinessDomains => _businessDomains;

        private List<BusinessConceptMapping> _temporaryBusinessConceptMappings;

        private ObservableCollection<BusinessConceptMapping> _filteredBusinessConceptMappings;

        public ObservableCollection<BusinessConceptMapping> FilteredBusinessConceptMappings
        {
            get => _filteredBusinessConceptMappings;
            set
            {
                if (_filteredBusinessConceptMappings != value)
                {
                    _filteredBusinessConceptMappings = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Filtered view of BusinessConceptMappings based on selected SourceInterface
        /// </summary>
        public void FilterBusinessConceptMappings()
        {
            if (SelectedSourceInterface != null)
            {
                if (SelectedSourceInterface.SourceAttributes != null)
                {
                    var sourceAttributes = SelectedSourceInterface.SourceAttributes.ToHashSet();

                    // Collect all mappings from all business concepts
                    IEnumerable<BusinessConceptMapping> allBusinessConceptMappings = BusinessConcepts.SelectMany(bc => bc.BusinessConceptMappings);
                        
                    // Get existing mappings for the selected interface
                    var existingMappings = allBusinessConceptMappings
                        .Where(m => m.SourceAttribute != null && sourceAttributes.Contains(m.SourceAttribute))
                        .ToList();

                    // Find source attributes without mappings and create temporary ones
                    HashSet<SourceAttribute> mappedAttributes = existingMappings
                        .Where(m => m.SourceAttribute != null)
                        .Select(m => m.SourceAttribute!)
                        .ToHashSet();

                    _temporaryBusinessConceptMappings.Clear();
                    foreach (SourceAttribute sa in sourceAttributes)
                    {

                        if (!mappedAttributes.Contains(sa))
                        {
                            var tempMapping = BusinessConceptMapping.FromSourceAttribute(sa);
                            _temporaryBusinessConceptMappings.Add(tempMapping);
                        }

                    }

                    FilteredBusinessConceptMappings = new ObservableCollection<BusinessConceptMapping>(existingMappings.Concat(_temporaryBusinessConceptMappings));
                }
                else
                {
                    FilteredBusinessConceptMappings = new ObservableCollection<BusinessConceptMapping>(Enumerable.Empty<BusinessConceptMapping>());
                }
            }
            else
            {
                FilteredBusinessConceptMappings = new ObservableCollection<BusinessConceptMapping>(Enumerable.Empty<BusinessConceptMapping>());
            }
        }

        private void ApplyNewBCMappingsToDataSett()
        {

            foreach (BusinessConceptMapping currentMapping in _temporaryBusinessConceptMappings)
            {
                if (currentMapping.ParentBusinessConcept != null)
                {
                    BusinessConcepts.Where(bc => bc == currentMapping.ParentBusinessConcept).First().BusinessConceptMappings.Add(currentMapping);
                }
            }

        }

        private string _serverPath;
        public string ServerPath
        {
            get => _serverPath;
            set
            {
                if (_serverPath != value)
                {
                    _serverPath = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task LoadDataFromPathAsync(string path)
        {

            if (!string.IsNullOrWhiteSpace(path))
            {
                await MetaDataIOService.LoadDataAsync(path);

                _sourceSystems.Clear();
                foreach (SourceSystem currentSourceSystem in MetaDataIOService.GetSourceSystems())
                {
                    _sourceSystems.Add(currentSourceSystem);
                }

                _businessDomains.Clear();
                _businessConcepts.Clear();

                foreach (BusinessDomain currentBusinessDomain in MetaDataIOService.GetBusinessDomains())
                {
                    _businessDomains.Add(currentBusinessDomain);

                    foreach (BusinessConcept currentBusinessConcept in currentBusinessDomain.BusinessConcepts)
                    {
                        _businessConcepts.Add(currentBusinessConcept);
                    }
                }

            }

        }

        public async Task SaveChangesAsync()
        {

            if (!string.IsNullOrWhiteSpace(ServerPath))
            {
                await MetaDataIOService.WriteDataAsync(ServerPath, BusinessDomains);
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
