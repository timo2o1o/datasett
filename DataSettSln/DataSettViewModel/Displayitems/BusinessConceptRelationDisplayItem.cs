using System.ComponentModel;
using DataSett.Metamodel;
using System.Runtime.CompilerServices;
using System.Net.Http.Headers;

namespace DataSett.ViewModel.DisplayItems;

public class BusinessConceptRelationDisplayitem : DisplayitemBase<BusinessConceptRelation>
{

    private IList<BusinessConceptRelationItem> _businessConceptRelationItems;
    private string? _relationName;

    public BusinessConceptRelationDisplayitem(BusinessConceptRelation businessConceptRelation) : base(businessConceptRelation)
    {
    
        _businessConceptRelationItems = businessConceptRelation.RelatedConcepts ?? new List<BusinessConceptRelationItem>();
        _relationName = businessConceptRelation.Name;

    }

    public BusinessConceptRelationDisplayitem(IList<BusinessConceptRelationItem> businessConceptRelationItems) : base(null)
    {
        _businessConceptRelationItems = businessConceptRelationItems;
    }

    public BusinessConceptRelationDisplayitem(IEnumerable<BusinessConcept> businessConcepts) :
    this([.. businessConcepts.Select(bc => new BusinessConceptRelationItem {IsLeadingKey = false, RelatedBusinessConcept = bc})])
        {
        }

    public string? RelationName
    {
        get => _relationName;
        set
        {
            if (!SetField(ref _relationName, value))
            {
                return;
            }

            if (_existingItem != null)
            {
                _existingItem.Name = value;
            }
        }
    }

    public IList<BusinessConceptRelationItem> BusinessConceptRelationItems
    {
        get => _businessConceptRelationItems;
        set => SetField(ref _businessConceptRelationItems, value);
    }

    /// <summary>
    /// Promotes this derived (unpersisted) display item to a persisted state
    /// by creating a <see cref="BusinessConceptRelation"/> domain object and
    /// adding it to the parent <see cref="BusinessDomain"/>'s collection so
    /// it is included during serialization.
    /// </summary>
    public void Persist()
    {

        if (!IsPersisted)
        {
            
            // Determine the parent domain from the first related concept.
            var parentDomain = _businessConceptRelationItems
                .Select(item => item.RelatedBusinessConcept?.ParentBusinessDomain)
                .FirstOrDefault(d => d != null)
                ?? throw new InvalidOperationException(
                    "Cannot persist a relation when none of the related concepts belong to a business domain.");

            var relation = new BusinessConceptRelation
            {
                Name = _relationName,
                ParentBusinessDomain = parentDomain
            };

            foreach (var item in _businessConceptRelationItems)
            {
                relation.RelatedConcepts.Add(item);
            }

            // Add to the domain so WriteLBCMAsync picks it up.
            parentDomain.BusinessConceptRelations.Add(relation);

            _existingItem = relation;
            BusinessConceptRelationItems = relation.RelatedConcepts;
            RelationName = _existingItem.Name;
            OnPropertyChanged(nameof(IsPersisted));

        }

    }

    private bool HasChangedRelationItems()
    {
        if (_existingItem == null)
        {
            return _businessConceptRelationItems.Count > 0;
        }

        return !_businessConceptRelationItems.SequenceEqual(_existingItem.RelatedConcepts);
    }

    public override bool IsDirty
    {
        get
        {
            if (_existingItem == null)
            {
                return !string.IsNullOrWhiteSpace(_relationName) || _businessConceptRelationItems.Count > 0;
            }

            return !string.Equals(_relationName, _existingItem.Name, StringComparison.Ordinal)
                || HasChangedRelationItems();
        }
    }

    public override void ApplyChanges()
    {
        if (_existingItem == null) return;

        _existingItem.Name = _relationName;

        _existingItem.RelatedConcepts.Clear();
        foreach (var item in _businessConceptRelationItems)
        {
            _existingItem.RelatedConcepts.Add(item);
        }
    }

    public bool HasSameConcepts(IEnumerable<BusinessConcept?> concepts) =>
        _businessConceptRelationItems
            .Select(i => i.RelatedBusinessConcept)
            .ToHashSet()
            .SetEquals(concepts.Where(c => c is not null));
}