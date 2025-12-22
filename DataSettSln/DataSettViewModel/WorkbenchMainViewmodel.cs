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
            _businessConceptMappings = new ObservableCollection<BusinessConceptMapping>();
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
                    OnPropertyChanged();

                    GetBusinessConceptMappings(_selectedSourceInterface);
                }
            }
        }

        private ObservableCollection<BusinessDomain> _businessDomains;
        public ObservableCollection<BusinessDomain> BusinessDomains => _businessDomains;

        private ObservableCollection<BusinessConceptMapping> _businessConceptMappings;
        public ObservableCollection<BusinessConceptMapping> BusinessConceptMappings => _businessConceptMappings;

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

        public async Task InitializeAsync()
        {
            await LoadDataFromPathAsync(ServerPath);
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

        private void GetBusinessConceptMappings(SourceInterface selectedSourceInterface)
        {
            _businessConceptMappings.Clear();

            if (selectedSourceInterface != null && selectedSourceInterface.SourceAttributes != null)
            {
                foreach (SourceAttribute currentSrcAttribute in selectedSourceInterface.SourceAttributes)
                {
                    IEnumerable<BusinessConceptMapping> matchingMappings = BusinessDomains
                        .SelectMany(bd => bd.BusinessConcepts)
                        .SelectMany(bc => bc.BusinessConceptMappings)
                        .Where(asm => asm.SourceAttribute == currentSrcAttribute);

                    if (matchingMappings.Count() == 1)
                    {
                        _businessConceptMappings.Add(matchingMappings.First());
                    }
                    else
                    {
                        _businessConceptMappings.Add(BusinessConceptMapping.FromSourceAttribute(currentSrcAttribute));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
