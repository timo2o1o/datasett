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
    public class BusinessObjectViewModel : INotifyPropertyChanged
    {
        public BusinessObjectViewModel(IMetaDataIOService metaDataIOService, IOptions<AppSettings> appSettings)
        {
            MetaDataIOService = metaDataIOService;
            _businessObjects = new ObservableCollection<BusinessObject>();
        }

        private IMetaDataIOService MetaDataIOService { get; set; }

        private ObservableCollection<BusinessObject> _businessObjects;
        public ObservableCollection<BusinessObject> BusinessObjects => _businessObjects;

        private BusinessObject? _selectedBusinessObject;
        public BusinessObject? SelectedBusinessObject
        {
            get => _selectedBusinessObject;
            set
            {
                if (_selectedBusinessObject != value)
                {
                    _selectedBusinessObject = value;
                    OnPropertyChanged();
                }
            }
        }

        public void GetBusinessObjectData()
        {

            _businessObjects.Clear();

            foreach (BusinessDomain currentDomain in MetaDataIOService.GetBusinessDomains())
            {
                foreach (BusinessObject currentBusinessObject in currentDomain.BusinessObjects)
                {
                    _businessObjects.Add(currentBusinessObject);
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
