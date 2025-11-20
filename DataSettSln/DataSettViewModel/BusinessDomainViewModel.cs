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

        public void GetBusinessDomainData()
        {
            
            _businessDomains.Clear();

            foreach (BusinessDomain currentDomain in MetaDataIOService.GetBusinessDomains())
            {
                _businessDomains.Add(currentDomain);
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
