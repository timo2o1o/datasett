using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace DataSettWorkbench.Components.DiagramNodes;

public class DiamondNodeModel : NodeModel
{
    public DiamondNodeModel(Point? position = null) : base(position)
    {
    }

    public string? RelationName { get; set; }
    public bool IsPersisted { get; set; }
}
