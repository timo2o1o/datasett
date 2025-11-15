namespace DataSett.ViewModel
{
    /// <summary>
    /// Application settings that can be configured via appsettings.json or environment variables
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// The path to the metadata repository
        /// Can be overridden by environment variable: AppSettings__RepositoryPath
        /// </summary>
        public string RepositoryPath { get; set; } = string.Empty;
    }
}
