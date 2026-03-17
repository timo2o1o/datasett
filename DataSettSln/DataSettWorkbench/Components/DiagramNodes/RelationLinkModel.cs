using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models;
using DataSett.Metamodel;
using DataSett.ViewModel.DisplayItems;

namespace DataSettWorkbench.Components.DiagramNodes;

public class RelationLinkModel : LinkModel
{
    public RelationLinkModel(Anchor source, Anchor target, bool isPersisted,
        BusinessConceptRelationDisplayitem parentDisplayItem,
        BusinessConceptRelationItem relationItem)
        : base(source, target)
    {
        IsPersisted = isPersisted;
        ParentDisplayItem = parentDisplayItem;
        RelationItem = relationItem;
    }

    public bool IsPersisted { get; set; }
    public BusinessConceptRelationDisplayitem ParentDisplayItem { get; }
    public BusinessConceptRelationItem RelationItem { get; }
}
