using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using DataSett.ViewModel.DisplayItems;

namespace DataSettWorkbench.Components.DiagramNodes;

public class DiamondNodeModel : NodeModel, IBusinessConceptRelationDisplayitemNode
{
    private const double MinDiamondSize = 80;
    private const double CharWidthEstimate = 10.3; // px per character at the label font size

    public DiamondNodeModel(BusinessConceptRelationDisplayitem displayItem, Point? position = null) : base(position)
    {
        ParentDisplayItem = displayItem;
        UpdateSize();
    }

    public string? RelationName {
        get
        {
            return ParentDisplayItem.RelationName;
        }
        set
        {
            ParentDisplayItem.RelationName = value;
            UpdateSize();
        }
     }

    public bool IsPersisted
    {
        get
        {
            return ParentDisplayItem.IsPersisted;
        }
    }

    /// <summary>Side length (px) of the diamond square before rotation.</summary>
    public double DiamondSize { get; private set; }

    public BusinessConceptRelationDisplayitem ParentDisplayItem { get; set; }

    private void UpdateSize()
    {
        var labelWidth = Math.Max(MinDiamondSize, (RelationName?.Length ?? 0) * CharWidthEstimate);
        // Label width is diamondSize * 0.875, so diamondSize = labelWidth / 0.875
        DiamondSize = Math.Max(MinDiamondSize, labelWidth / 0.875);
        // The bounding box of a rotated square is side * sqrt(2)
        var boundingBox = DiamondSize * Math.Sqrt(2);
        Size = new Size(boundingBox, boundingBox);
    }
}
