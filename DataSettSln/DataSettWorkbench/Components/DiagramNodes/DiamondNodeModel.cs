using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using DataSett.ViewModel.DisplayItems;

namespace DataSettWorkbench.Components.DiagramNodes;

public class DiamondNodeModel : NodeModel, IBusinessConceptRelationDisplayitemNode
{
    public DiamondNodeModel(BusinessConceptRelationDisplayitem displayItem, Point? position = null) : base(position)
    {
        ParentDisplayItem = displayItem;
    }

    public string? RelationName {
        get
        {
            return ParentDisplayItem.RelationName;
        }
        set
        {
            ParentDisplayItem.RelationName = value;
        }
     }

    public bool IsPersisted
    {
        get
        {
            return ParentDisplayItem.IsPersisted;
        }
    }

    public BusinessConceptRelationDisplayitem ParentDisplayItem { get; set; }
}
