using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Base class for BusinessDomain containing scalar and context properties.
    /// This class is part of the Base/DTO/Domain separation pattern:
    /// - Base classes contain only scalar/context properties (Id, Name, timestamps, etc.)
    /// - DTO classes inherit from base and add foreign key references for serialization
    /// - Domain classes inherit from base and add navigation properties and business logic
    /// </summary>
    public abstract class BusinessDomainBase
    {
        /// <summary>
        /// Name of the business domain
        /// </summary>
        [JsonPropertyName("businessDomainName")]
        public string? Name { get; set; }
    }
}
