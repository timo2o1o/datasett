namespace DataSett.Metamodel
{

    public abstract class SourceSystemBase
    {

        /// <summary>
        /// Database driver or connector type
        /// </summary>
        public string? Driver { get; set; }

        /// <summary>
        /// Connection string to the source system
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Server name or address
        /// </summary>
        public string? Server { get; set; }

        /// <summary>
        /// Name of the source system
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the short name associated with this System.
        /// </summary>
        public string? ShortName { get; set; }

        /// <summary>
        /// Version of the source system
        /// </summary>
        public string? Version { get; set; }

    }

}