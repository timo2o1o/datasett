using System.ComponentModel;
using DataSett.Metamodel;
using System.Runtime.CompilerServices;

namespace DataSett.ViewModel.DisplayItems;

public class BusinessConceptRelationDisplayitem : DisplayitemBase<BusinessConceptRelation>
{

    private IList<BusinessConceptRelationItem> _businessConceptRelationItems;

    public BusinessConceptRelationDisplayitem(BusinessConceptRelation businessConceptRelation) : base(businessConceptRelation)
    {
        if (businessConceptRelation.RelatedConcepts != null && businessConceptRelation.RelatedConcepts.Count > 0)
        {
            _businessConceptRelationItems = businessConceptRelation.RelatedConcepts;
        }
        else
        {
            throw new ArgumentException("It makes no sense to create a BusinessConceptRelationDisplayitem without any related concepts. Please provide at least one related concept.", nameof(businessConceptRelation.RelatedConcepts));
        }
    }

    public BusinessConceptRelationDisplayitem(IList<BusinessConceptRelationItem> businessConceptRelationItems) : base(null)
    {
        _businessConceptRelationItems = businessConceptRelationItems;
    }

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