using DataSett.Metamodel;
using DataSett.ViewModel.DisplayItems;
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
    public class BusinessConceptRelationViewModel : INotifyPropertyChanged
    {
        public BusinessConceptRelationViewModel(IMetaDataIOService metaDataIOService, IOptions<AppSettings> appSettings)
        {
            MetaDataIOService = metaDataIOService;
            _businessConcepts = new ObservableCollection<BusinessConcept>();
            _businessConceptRelations = new ObservableCollection<BusinessConceptRelationDisplayitem>();
        }

        private IMetaDataIOService MetaDataIOService { get; set; }

        private ObservableCollection<BusinessConcept> _businessConcepts;
        public ObservableCollection<BusinessConcept> BusinessConcepts => _businessConcepts;

        private ObservableCollection<BusinessConceptRelationDisplayitem> _businessConceptRelations;
        public ObservableCollection<BusinessConceptRelationDisplayitem> BusinessConceptRelations => _businessConceptRelations;

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

        public void GetBusinessConceptRelationData()
        {

            _businessConceptRelations.Clear();

            var businessDomains = MetaDataIOService.GetBusinessDomains();

            // This is a two-step process. The easy part is to retrieve the relations stored in the datamodel.
            businessDomains
                .SelectMany(d => d.BusinessConceptRelations)
                .Select(r => new BusinessConceptRelationDisplayitem(r))
                .ToList()
                .ForEach(item => _businessConceptRelations.Add(item));

            // The more complex part is to derive the relations from the BusinessConceptMappings.
            var businessConceptMappings = businessDomains
                .SelectMany(d => d.BusinessConcepts)
                .SelectMany(bc => bc.BusinessConceptMappings);

            foreach (BusinessConceptRelationDisplayitem currentRDI in DeriveRelationsFromMappings(businessConceptMappings))
            {
                _businessConceptRelations.Add(currentRDI);
            }

        }

        private IEnumerable<BusinessConceptRelationDisplayitem> DeriveRelationsFromMappings(IEnumerable<BusinessConceptMapping> businessConceptMappings)
        {

            // This method derives BusinessConceptRelations from the BusinessConceptMappings. The logic is as follows:
            // 1.   For each SourceInterface, we check the BusinessConceptMappings for role "BusinessKey".
            // 2.   If a SourceInterface got more than two BusinessConceptMappings poiting to different
            //      BusinessConcepts this defines a new possible BusinessConceptRelation.
            var derivedRelations = businessConceptMappings
                .Where(m => m.Role == SourceAttributeRole.BusinessKey && m.SourceAttribute != null)
                .GroupBy(m => m.SourceAttribute!.ParentSourceInterface)
                .Where(g => g.Select(m => m.ParentBusinessConcept).Distinct().Count() > 1)
                .Select(g => new BusinessConceptRelationDisplayitem(
                    g.Select(m => m.ParentBusinessConcept)
                     .Where(bc => bc is not null)
                     .Select(bc => bc!)
                     .Distinct()));

            return derivedRelations;

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
