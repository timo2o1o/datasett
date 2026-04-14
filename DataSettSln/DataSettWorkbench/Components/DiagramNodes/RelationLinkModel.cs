using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models;
using DataSett.Metamodel;
using DataSett.ViewModel.DisplayItems;

namespace DataSettWorkbench.Components.DiagramNodes;

public class RelationLinkModel : LinkModel, IBusinessConceptRelationDisplayitemNode
{
    public RelationLinkModel(Anchor source, Anchor target, BusinessConceptRelationDisplayitem parentDisplayItem,
        BusinessConceptRelationItem relationItem)
        : base(source, target)
    {
        ParentDisplayItem = parentDisplayItem;
        ParentRelationItem = relationItem;
    }

    public bool IsPersisted => ParentDisplayItem.IsPersisted;
    public BusinessConceptRelationDisplayitem ParentDisplayItem { get; }
    public BusinessConceptRelationItem ParentRelationItem { get; }
}
