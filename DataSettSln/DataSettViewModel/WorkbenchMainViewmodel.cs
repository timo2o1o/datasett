using DataSett.Metamodel;
using DataSett.ViewModel.Services;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataSett.ViewModel
{
    public class WorkbenchMainViewmodel : INotifyPropertyChanged
    {
        public WorkbenchMainViewmodel(IMetaDataIOService metaDataIOService)
        {
            MetaDataIOService = metaDataIOService;

            // Set standard values for properties:
            _sourceSystems = new ObservableCollection<SourceSystem>();
            _serverPath = @"d:\Dev\github\timo2o1o\willibald-metadata\";
        }

        private IMetaDataIOService MetaDataIOService { get; set; }

        // Properties for binding to the view:
        private ObservableCollection<SourceSystem> _sourceSystems;
        public ObservableCollection<SourceSystem> SourceSystems => _sourceSystems;

        private SourceSystem? _selectedSourceSystem;
        public SourceSystem? SelectedSourceSystem
        {
            get => _selectedSourceSystem;
            set
            {
                if (_selectedSourceSystem != value)
                {
                    _selectedSourceSystem = value;
                    OnPropertyChanged();
                }
            }
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

        public async Task InitializeAsync()
        {
            // Laden Sie Daten asynchron
            var systems = await MetaDataIOService.LoadSourceSystemsAsync(ServerPath);
            foreach (var system in systems)
            {
                _sourceSystems.Add(system);
            }
        }

        public async Task LoadDataFromPathAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;
            
            // Bestehende Daten löschen
            _sourceSystems.Clear();

            // Neue Daten vom angegebenen Pfad laden
            var systems = await MetaDataIOService.LoadSourceSystemsAsync(path);
            foreach (var system in systems)
            {
                _sourceSystems.Add(system);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
