using DataSett.Metamodel;
using DataSett.ViewModel.Services;

using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            _serverPath = appSettings.Value.RepositoryPath ?? string.Empty;
            _businessDomains = new ObservableCollection<BusinessDomain>();
            _attributeSetMappings = new ObservableCollection<AttributeSetMapping>();
        }

        private IMetaDataIOService MetaDataIOService { get; set; }

        // Properties for binding to the view:
        private ObservableCollection<SourceSystem> _sourceSystems;
        public ObservableCollection<SourceSystem> SourceSystems => _sourceSystems;

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

                    GetAttributeSetMappings(_selectedSourceInterface);
                }
            }
        }

        private ObservableCollection<BusinessDomain> _businessDomains;
        public ObservableCollection<BusinessDomain> BusinessDomains => _businessDomains;

        private ObservableCollection<AttributeSetMapping> _attributeSetMappings;
        public ObservableCollection<AttributeSetMapping> AttributeSetMappings => _attributeSetMappings;

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
                foreach (BusinessDomain currentBusinessDomain in MetaDataIOService.GetBusinessDomains())
                {
                    _businessDomains.Add(currentBusinessDomain);
                }

            }

        }

        private void GetAttributeSetMappings(SourceInterface selectedSourceInterface)
        {
            _attributeSetMappings.Clear();

            if (selectedSourceInterface != null && selectedSourceInterface.SourceAttributes != null)
            {
                foreach (SourceAttribute currentSrcAttribute in selectedSourceInterface.SourceAttributes)
                {
                    IEnumerable<AttributeSetMapping> matchingMappings = BusinessDomains
                        .SelectMany(bd => bd.BusinessObjects)
                        .SelectMany(bo => bo.AttributeSets)
                        .SelectMany(attributeSet => attributeSet.AttributeSetMappings)
                        .Where(asm => asm.SourceAttribute == currentSrcAttribute);

                    if (matchingMappings.Count() == 1)
                    {
                        _attributeSetMappings.Add(matchingMappings.First());
                    }
                    else
                    {
                        _attributeSetMappings.Add(new AttributeSetMapping());
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
