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
        public BusinessConceptRelationViewModel(IMetaDataIOService metaDataIOService)
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

        private readonly List<BusinessConceptRelationDisplayitem> _selectedRelations = new();
        private readonly List<BusinessConceptRelationItem> _selectedRelationItems = new();

        /// <summary>
        /// The currently selected relation display items as reported by the view.
        /// </summary>
        public IReadOnlyList<BusinessConceptRelationDisplayitem> SelectedRelations => _selectedRelations;

        /// <summary>
        /// Returns <c>true</c> when the current selection contains at least one
        /// unpersisted relation that can be promoted to a domain object.
        /// </summary>
        public bool CanPersistSelection => _selectedRelations.Any(r => !r.IsPersisted);

        /// <summary>
        /// Returns <c>true</c> when there is at least one link selected whose
        /// <see cref="BusinessConceptRelationItem.IsLeadingKey"/> can be toggled.
        /// </summary>
        public bool CanToggleLeadingKey => _selectedRelationItems.Any();

        /// <summary>
        /// Replaces the current selection with the given display items.
        /// Raises <see cref="PropertyChanged"/> for <see cref="SelectedRelations"/>
        /// and <see cref="CanPersistSelection"/>.
        /// </summary>
        public void UpdateSelection(IEnumerable<BusinessConceptRelationDisplayitem> selectedItems)
        {
            _selectedRelations.Clear();
            _selectedRelations.AddRange(selectedItems);
            OnPropertyChanged(nameof(SelectedRelations));
            OnPropertyChanged(nameof(CanPersistSelection));
        }

        /// <summary>
        /// Replaces the current link-level selection with the given relation items.
        /// Raises <see cref="PropertyChanged"/> for <see cref="CanToggleLeadingKey"/>.
        /// </summary>
        public void UpdateLinkSelection(IEnumerable<BusinessConceptRelationItem> selectedItems)
        {
            _selectedRelationItems.Clear();
            _selectedRelationItems.AddRange(selectedItems);
            OnPropertyChanged(nameof(CanToggleLeadingKey));
        }

        /// <summary>
        /// Toggles <see cref="BusinessConceptRelationItem.IsLeadingKey"/> for all
        /// currently selected relation items. If all items are already leading keys
        /// the value is set to <c>false</c>; otherwise all are set to <c>true</c>.
        /// Returns the affected items so the view can refresh the corresponding links.
        /// </summary>
        public IEnumerable<BusinessConceptRelationItem> ToggleLeadingKey()
        {
            if (!_selectedRelationItems.Any())
                return [];

            bool newValue = !_selectedRelationItems.All(i => i.IsLeadingKey == true);

            foreach (var item in _selectedRelationItems)
            {
                item.IsLeadingKey = newValue;
            }

            return _selectedRelationItems.ToList();
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

            foreach (BusinessConceptRelationDisplayitem currentRDI in DeriveRelationsFromMappings(businessConceptMappings, _businessConceptRelations))
            {
                _businessConceptRelations.Add(currentRDI);
            }

        }

        private IEnumerable<BusinessConceptRelationDisplayitem> DeriveRelationsFromMappings(
            IEnumerable<BusinessConceptMapping> businessConceptMappings,
            IEnumerable<BusinessConceptRelationDisplayitem> existingRelations)
        {

            // This method derives BusinessConceptRelations from the BusinessConceptMappings. The logic is as follows:
            // 1.   For each SourceInterface, we check the BusinessConceptMappings for role "BusinessKey".
            // 2.   If a SourceInterface has BusinessConceptMappings pointing to two or more different
            //      BusinessConcepts, this defines a new possible BusinessConceptRelation.
            var derivedRelations = businessConceptMappings
                .Where(m => m.Role == SourceAttributeRole.BusinessKey && m.SourceAttribute != null)
                .GroupBy(m => m.SourceAttribute!.ParentSourceInterface)
                .Where(g => g.Select(m => m.ParentBusinessConcept).Distinct().Count() > 1)
                .Select(g => new BusinessConceptRelationDisplayitem(
                    g.Select(m => m.ParentBusinessConcept)
                     .Where(bc => bc is not null)
                     .Select(bc => bc!)
                     .Distinct()))
                .Where(derived => !existingRelations.Any(e => e.HasSameConcepts(derived.BusinessConceptRelationItems.Select(i => i.RelatedBusinessConcept))));

            return derivedRelations;

        }

        /// <summary>
        /// Persists all currently selected unpersisted relations, promoting them
        /// to domain objects. Returns the display items that were persisted so
        /// the view can update the corresponding diagram visuals.
        /// </summary>
        public IEnumerable<BusinessConceptRelationDisplayitem> PersistSelectedRelations()
        {
            IList<BusinessConceptRelationDisplayitem> toPersist = _selectedRelations.Where(r => !r.IsPersisted).ToList();

            foreach (BusinessConceptRelationDisplayitem item in toPersist)
            {
                item.Persist();
            }

            OnPropertyChanged(nameof(CanPersistSelection));

            return toPersist;
        }

        /// <summary>
        /// Removes a <see cref="BusinessConceptRelationItem"/> from its parent display item.
        /// </summary>
        public void RemoveRelationItem(BusinessConceptRelationDisplayitem displayItem, BusinessConceptRelationItem item)
        {
            displayItem.BusinessConceptRelationItems.Remove(item);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
