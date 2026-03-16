using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models;

namespace DataSettWorkbench.Components.DiagramNodes;

public class RelationLinkModel : LinkModel
{
    public RelationLinkModel(Anchor source, Anchor target, bool isPersisted)
        : base(source, target)
    {
        IsPersisted = isPersisted;
    }

    public bool IsPersisted { get; set; }
}
