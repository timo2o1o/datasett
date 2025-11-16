namespace DataSett.Metamodel
{
    /// <summary>
    /// This class is meant to represent physical source attribute relationships.
    /// </summary>
    //TODO: This seems to be unused currently. Consider removing it if not needed.
    public class SourceAttributeRelation
    {
        
        public string? Name { get; set; }

        public SourceAttributeRelationType? RelationType { get; set; }

        public int? Order { get; set; }

        public string? LocalKey { get; set; }

        public string? ParentTable { get; set; }

        public string? ParentKey { get; set; }

    }
}
