using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using DataSett.Metamodel;

namespace DataSettWorkbench.Components.DiagramNodes;

public class DomainNodeModel : NodeModel
{
    public DomainNodeModel(BusinessDomain domain, Point? position = null) : base(position)
    {
        Domain = domain;
        Title = domain.Name;
    }

    public BusinessDomain Domain { get; }
}
