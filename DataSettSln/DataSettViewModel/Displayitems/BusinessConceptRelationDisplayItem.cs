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
    /// by creating a <see cref="BusinessConceptRelation"/> domain object.
    /// </summary>
    public void Persist(string? relationName)
    {
        if (IsPersisted) return;

        var relation = new BusinessConceptRelation
        {
            Name = relationName
        };

        foreach (var item in _businessConceptRelationItems)
        {
            relation.RelatedConcepts.Add(item);
        }

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
        throw new NotImplementedException();
    }
}