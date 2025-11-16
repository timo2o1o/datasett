namespace DataSett.Metamodel
{
    public abstract class SourceInterfaceBase
    {

        /// <summary>
        /// Schema name (for databases)
        /// </summary>
        public string? Schema { get; set; }

        /// <summary>
        /// Catalog name (for databases)
        /// </summary>
        public string? Catalog { get; set; }

        /// <summary>
        /// Name of the interface (table, file, etc.)
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// List of attributes within the source interface
        /// </summary>
        public IList<SourceAttribute>? SourceAttributes { get; set; }

    }
}
