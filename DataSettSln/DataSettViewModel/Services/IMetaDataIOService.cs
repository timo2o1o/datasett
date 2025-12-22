using DataSett.Metamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSett.ViewModel.Services
{
    public interface IMetaDataIOService
    {
        Task LoadDataAsync(string repositoryPath);
        IEnumerable<SourceSystem> GetSourceSystems();
        Task SaveSourceSystemsAsync(string repositoryPath, IEnumerable<SourceSystem> sourceSystems);
        IEnumerable<BusinessDomain> GetBusinessDomains();

    }
}
