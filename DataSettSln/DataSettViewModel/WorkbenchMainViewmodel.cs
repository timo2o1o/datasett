using DataSett.Metamodel;
using DataSett.ViewModel.Services;
using DataSett.ViewModel.DisplayItems;

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
    public class WorkbenchMainViewmodel : INotifyPropertyChanged, IDisposable
    {
        private readonly MainLayoutViewModel _mainLayoutViewModel;

        public WorkbenchMainViewmodel(IMetaDataIOService metaDataIOService, MainLayoutViewModel mainLayoutViewModel)
        {
            MetaDataIOService = metaDataIOService;
            _mainLayoutViewModel = mainLayoutViewModel;

            // Set standard values for properties:
            _sourceSystems = new ObservableCollection<SourceSystem>();
            _businessConcepts = new ObservableCollection<BusinessConcept>();
            _businessDomains = new ObservableCollection<BusinessDomain>();
            DisplayMappings = new ObservableCollection<BusinessConceptMappingDisplayitem>();

            // Subscribe to data reload events from the layout
            _mainLayoutViewModel.DataReloaded += RefreshData;

            // Register save handler so MainLayout's save button delegates to us
            _mainLayoutViewModel.RegisterSaveHandler(SaveChangesAsync);

            // Populate collections from IO service if data was already loaded
            RefreshData();
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

        private ObservableCollection<BusinessConceptMappingDisplayitem> _displayMappings = new();
        public ObservableCollection<BusinessConceptMappingDisplayitem> DisplayMappings
        {
            get => _displayMappings;
            private set { _displayMappings = value; OnPropertyChanged(); }
        }

        public bool HasPendingChanges => DisplayMappings.Any(m => m.IsDirty);

        /// <summary>
        /// Filtered view of BusinessConceptMappings based on selected SourceInterface
        /// </summary>
        public void FilterBusinessConceptMappings()
        {
            if (SelectedSourceInterface?.SourceAttributes == null)
            {
                DisplayMappings = new ObservableCollection<BusinessConceptMappingDisplayitem>();
                return;
            }

            var sourceAttributes = SelectedSourceInterface.SourceAttributes.ToHashSet();
            var allMappings = BusinessConcepts.SelectMany(bc => bc.BusinessConceptMappings);

            var items = sourceAttributes.Select(sa =>
            {
                var existing = allMappings.FirstOrDefault(m => m.SourceAttribute == sa);
                return existing != null
                    ? new BusinessConceptMappingDisplayitem(existing)
                    : new BusinessConceptMappingDisplayitem(sa);
            });

            DisplayMappings = new ObservableCollection<BusinessConceptMappingDisplayitem>(items);
        }

        /// <summary>
        /// Repopulates all collections from the IO service.
        /// Called on construction and whenever the MainLayoutViewModel fires DataReloaded.
        /// </summary>
        private void RefreshData()
        {
            SourceSystems.Clear();
            foreach (SourceSystem currentSourceSystem in MetaDataIOService.GetSourceSystems())
            {
                SourceSystems.Add(currentSourceSystem);
            }

            BusinessDomains.Clear();
            BusinessConcepts.Clear();

            foreach (BusinessDomain currentBusinessDomain in MetaDataIOService.GetBusinessDomains())
            {
                BusinessDomains.Add(currentBusinessDomain);

                foreach (BusinessConcept currentBusinessConcept in currentBusinessDomain.BusinessConcepts)
                {
                    BusinessConcepts.Add(currentBusinessConcept);
                }
            }
        }

        public async Task SaveChangesAsync()
        {
            foreach (var item in DisplayMappings.Where(i => i.IsDirty))
            {
                item.ApplyChanges();
            }

            if (!string.IsNullOrWhiteSpace(_mainLayoutViewModel.ServerPath))
            {
                await MetaDataIOService.WriteDataAsync(_mainLayoutViewModel.ServerPath, BusinessDomains);
            }

        }

        public async Task<BusinessConcept?> AddBusinessConceptAsync(string name, BusinessDomain domain)
        {
            var newConcept = new BusinessConcept { Name = name, ParentBusinessDomain = domain };
            BusinessConcepts.Add(newConcept);
            
            BusinessDomains.Where(d => d == domain).First().BusinessConcepts.Add(newConcept);

            return newConcept;
        }

        public BusinessDomain AddBusinessDomain(string name)
        {
            var newDomain = new BusinessDomain { Name = name };
            _businessDomains.Add(newDomain);
            return newDomain;
        }

        public void Dispose()
        {
            _mainLayoutViewModel.DataReloaded -= RefreshData;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
