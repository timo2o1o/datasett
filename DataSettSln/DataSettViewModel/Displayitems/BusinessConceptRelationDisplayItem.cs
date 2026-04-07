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
        set => SetField(ref _relationName, value);
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
    public void Persist(string? relationName)
    {
        if (IsPersisted) return;

        // Determine the parent domain from the first related concept.
        var parentDomain = _businessConceptRelationItems
            .Select(item => item.RelatedBusinessConcept?.ParentBusinessDomain)
            .FirstOrDefault(d => d != null);

        var relation = new BusinessConceptRelation
        {
            Name = relationName,
            ParentBusinessDomain = parentDomain
        };

        foreach (var item in _businessConceptRelationItems)
        {
            relation.RelatedConcepts.Add(item);
        }

        // Add to the domain so WriteLBCMAsync picks it up.
        parentDomain?.BusinessConceptRelations.Add(relation);

        _existingItem = relation;
        _relationName = relationName;
        OnPropertyChanged(nameof(IsPersisted));
        OnPropertyChanged(nameof(RelationName));
    }

    public override bool IsDirty
    {
        get
        {
            return true;
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
}