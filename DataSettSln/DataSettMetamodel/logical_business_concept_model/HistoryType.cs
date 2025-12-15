using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// This enum defines the different types of history tracking for business object attributes.
    /// It is used implement effective dates and versioning in data warehouse models.
    /// </summary>
    public enum HistoryType
    {

        /// <summary>
        /// No history tracking
        /// </summary>
        None,

        /// <summary>
        /// Track history with effective and end date
        /// </summary>
        EffectiveDated,

        /// <summary>
        /// Track history with valid from and valid to date
        /// </summary>
        Validated,

        /// <summary>
        /// Track history with version numbers
        /// </summary>
        Versioned,

        /// <summary>
        /// Track history with a change datetime
        /// </summary>
        ChangedDateTime,

        /// <summary>
        /// Track history with a deleted flag
        /// </summary>
        DeleteFlag,

        /// <summary>
        /// Other type of history tracking
        /// </summary>
        Other

    }
}