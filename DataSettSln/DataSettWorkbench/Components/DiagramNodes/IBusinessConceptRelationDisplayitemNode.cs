using DataSett.ViewModel.DisplayItems;

namespace DataSettWorkbench.Components.DiagramNodes
{
    public interface IBusinessConceptRelationDisplayitemNode
    {

        bool IsPersisted { get; }

        BusinessConceptRelationDisplayitem ParentDisplayItem { get; }

    }
}
