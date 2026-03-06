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
    public class BusinessConceptViewModel : INotifyPropertyChanged
    {
        public BusinessConceptViewModel(IMetaDataIOService metaDataIOService, IOptions<AppSettings> appSettings)
        {
            MetaDataIOService = metaDataIOService;
            _businessConcepts = new ObservableCollection<BusinessConcept>();
        }

        private IMetaDataIOService MetaDataIOService { get; set; }

        private ObservableCollection<BusinessConcept> _businessConcepts;
        public ObservableCollection<BusinessConcept> BusinessConcepts => _businessConcepts;

        private BusinessConcept? _selectedBusinessConcept;
        public BusinessConcept? SelectedBusinessConcept
        {
            get => _selectedBusinessConcept;
            set
            {
                if (_selectedBusinessConcept != value)
                {
                    _selectedBusinessConcept = value;
                    OnPropertyChanged();
                }
            }
        }

        public void GetBusinessConceptData()
        {

            _businessConcepts.Clear();

            foreach (BusinessDomain currentDomain in MetaDataIOService.GetBusinessDomains())
            {
                foreach (BusinessConcept currentBusinessConcept in currentDomain.BusinessConcepts)
                {
                    _businessConcepts.Add(currentBusinessConcept);
                }
            }

        }

        public void AddKeyPartToSelectedConcept(string name, AttributeProperties keyProperties)
        {
            if (SelectedBusinessConcept == null) return;

            var keyPart = new BusinessConceptKeyPart
            {
                Name = name,
                KeyProperties = keyProperties,
                ParentBusinessConcept = SelectedBusinessConcept
            };

            SelectedBusinessConcept.KeyParts.Add(keyPart);
            OnPropertyChanged(nameof(SelectedBusinessConcept));
        }

        public void RemoveKeyPartFromSelectedConcept(BusinessConceptKeyPart keyPart)
        {
            if (SelectedBusinessConcept == null) return;

            SelectedBusinessConcept.KeyParts.Remove(keyPart);
            OnPropertyChanged(nameof(SelectedBusinessConcept));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
