using System.ComponentModel;
using DataSett.Metamodel;
using System.Runtime.CompilerServices;
using System.Net.Http.Headers;

namespace DataSett.ViewModel.DisplayItems;

public class BusinessConceptRelationDisplayitem : DisplayitemBase<BusinessConceptRelation>
{

    private IList<BusinessConceptRelationItem> _businessConceptRelationItems;

    public BusinessConceptRelationDisplayitem(BusinessConceptRelation businessConceptRelation) : base(businessConceptRelation)
    {
    
        _businessConceptRelationItems = businessConceptRelation.RelatedConcepts ?? new List<BusinessConceptRelationItem>();
        RelationName = businessConceptRelation.Name;

    }

    public BusinessConceptRelationDisplayitem(IList<BusinessConceptRelationItem> businessConceptRelationItems) : base(null)
    {
        _businessConceptRelationItems = businessConceptRelationItems;
    }

    public BusinessConceptRelationDisplayitem(IEnumerable<BusinessConcept> businessConcepts) :
    this([.. businessConcepts.Select(bc => new BusinessConceptRelationItem {IsLeadingKey = false, RelatedBusinessConcept = bc})])
        {
        }

    public string? RelationName { get; init; }

    public IList<BusinessConceptRelationItem> BusinessConceptRelationItems
    {
        get => _businessConceptRelationItems;
        set => SetField(ref _businessConceptRelationItems, value);
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