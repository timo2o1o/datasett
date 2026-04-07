using DataSett.Metamodel;
using DataSett.ViewModel.Services;

using Microsoft.Extensions.Options;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataSett.ViewModel
{
    public class MainLayoutViewModel : INotifyPropertyChanged
    {
        private readonly IMetaDataIOService _metaDataIOService;

        public MainLayoutViewModel(IMetaDataIOService metaDataIOService, IOptions<AppSettings> appSettings)
        {
            _metaDataIOService = metaDataIOService;
            _serverPath = appSettings.Value.RepositoryPath ?? string.Empty;
        }

        private string _serverPath;
        public string ServerPath
        {
            get => _serverPath;
            set
            {
                if (_serverPath != value)
                {
                    _serverPath = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Raised after data has been successfully reloaded from the repository path.
        /// Pages should subscribe to this event to refresh their local state.
        /// </summary>
        public event Action? DataReloaded;

        public async Task LoadDataFromPathAsync()
        {
            if (!string.IsNullOrWhiteSpace(ServerPath))
            {
                await _metaDataIOService.LoadDataAsync(ServerPath);
                DataReloaded?.Invoke();
            }
        }

        private Func<Task>? _saveHandler;

        /// <summary>
        /// Registers a callback that performs the actual save operation.
        /// Called by WorkbenchMainViewmodel to provide its save logic.
        /// </summary>
        public void RegisterSaveHandler(Func<Task> handler)
        {
            _saveHandler = handler;
        }

        public async Task SaveChangesAsync()
        {
            if (_saveHandler != null)
            {
                await _saveHandler();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
