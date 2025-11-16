namespace DataSett.Metamodel
{

    /// <summary>
    /// This is the definition of an attribute within a source interface.
    /// Could be a column of a table or a field in a file.
    /// This class does not get serialized on its own, but as part of SourceInterface.
    /// That is why there is no DTO for this class.
    /// </summary>
    public class SourceAttribute
    {

        public SourceAttribute()
        {
            AttributeProperties = new AttributeProperties();
        }

        // Context Properties
        public string? Name { get; set; }

        public bool? IsPk { get; set; }

        public bool? IsFk { get; set; }

        public AttributeProperties AttributeProperties { get; set; }

    }
}