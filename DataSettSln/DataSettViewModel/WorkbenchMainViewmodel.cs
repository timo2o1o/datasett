using DataSett.Metamodel;
using DataSett.ViewModel.Services;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSett.ViewModel
{
    public class WorkbenchMainViewmodel
    {

        private MetaDataIOService _metaDataIOService;

        WorkbenchMainViewmodel(MetaDataIOService metaDataIOService)
        {
            _metaDataIOService = metaDataIOService;


            // Set standard values for properties:
            _sourceSystems = new ObservableCollection<SourceSystem>();

        }

        public MetaDataIOService MetaDataIOService => _metaDataIOService;

        // Properties for binding to the view:
        private ObservableCollection<SourceSystem> _sourceSystems;
        public ObservableCollection<SourceSystem> SourceSystems => _sourceSystems;

        public SourceSystem? SelectedSourceSystem { get; set; }

    }
}
