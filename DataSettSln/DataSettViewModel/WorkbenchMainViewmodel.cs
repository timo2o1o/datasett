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
            DisplayMappings = new ObservableCollection<MappingDisplayItem>();
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
                    _selectedSourceInterface = value;
                    FilterBusinessConceptMappings();
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<BusinessDomain> _businessDomains;
        public ObservableCollection<BusinessDomain> BusinessDomains => _businessDomains;

        private ObservableCollection<MappingDisplayItem> _displayMappings = new();
        public ObservableCollection<MappingDisplayItem> DisplayMappings
        {
            get => _displayMappings;
            private set { _displayMappings = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Filtered view of BusinessConceptMappings based on selected SourceInterface
        /// </summary>
        public void FilterBusinessConceptMappings()
        {
            if (SelectedSourceInterface?.SourceAttributes == null)
            {
                DisplayMappings = new ObservableCollection<MappingDisplayItem>();
                return;
            }

            var sourceAttributes = SelectedSourceInterface.SourceAttributes.ToHashSet();
            var allMappings = BusinessConcepts.SelectMany(bc => bc.BusinessConceptMappings);

            var items = sourceAttributes.Select(sa =>
            {
                var existing = allMappings.FirstOrDefault(m => m.SourceAttribute == sa);
                return existing != null
                    ? new MappingDisplayItem(existing)
                    : new MappingDisplayItem(sa);
            });

            DisplayMappings = new ObservableCollection<MappingDisplayItem>(items);
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
            foreach (var item in DisplayMappings.Where(i => i.IsDirty))
            {
                item.ApplyChanges();
            }

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
