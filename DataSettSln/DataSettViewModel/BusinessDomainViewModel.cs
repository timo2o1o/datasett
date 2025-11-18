using DataSett.Metamodel;
using DataSett.ViewModel.Services;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataSett.ViewModel
{
    public class BusinessDomainViewModel : INotifyPropertyChanged
    {
        public BusinessDomainViewModel(IMetaDataIOService metaDataIOService, IOptions<AppSettings> appSettings)
        {
            MetaDataIOService = metaDataIOService;
            _businessDomains = new ObservableCollection<BusinessDomain>();
            _serverPath = appSettings.Value.RepositoryPath ?? string.Empty;
        }

        private IMetaDataIOService MetaDataIOService { get; set; }

        private ObservableCollection<BusinessDomain> _businessDomains;
        public ObservableCollection<BusinessDomain> BusinessDomains => _businessDomains;

        private BusinessDomain? _selectedBusinessDomain;
        public BusinessDomain? SelectedBusinessDomain
        {
            get => _selectedBusinessDomain;
            set
            {
                if (_selectedBusinessDomain != value)
                {
                    _selectedBusinessDomain = value;
                    OnPropertyChanged();
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
            if (string.IsNullOrWhiteSpace(path))
                return;
            
            _businessDomains.Clear();

            var domains = await MetaDataIOService.LoadBusinessDomainsAsync(path);
            foreach (var domain in domains)
            {
                _businessDomains.Add(domain);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
