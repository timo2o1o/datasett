using DataSett.Metamodel;
using DataSett.Metamodel.Serde;

namespace DataSett.ViewModel.Services
{

    public class MetaDataIOService : IMetaDataIOService
    {
        private readonly JsonContext _dataContext;

        public MetaDataIOService()
        {
            _dataContext = new JsonContext();

        }

        public async Task LoadDataAsync(string repositoryPath)
        {
            await _dataContext.LoadAsync(repositoryPath);
        }

        public IEnumerable<SourceSystem> GetSourceSystems()
        {
            return _dataContext.GetSourceSystems();
        }

        public async Task SaveSourceSystemsAsync(string repositoryPath, IEnumerable<SourceSystem> sourceSystems)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusinessDomain> GetBusinessDomains()
        {
            return _dataContext.GetBusinessDomains();
        }

        public Task SaveBusinessDomainsAsync(string repositoryPath, IEnumerable<BusinessDomain> businessDomains)
        {
            throw new NotImplementedException();
        }

        public async Task WriteDataAsync(string repositoryPath, IEnumerable<BusinessDomain> businessDomains)
        {
            await _dataContext.WriteLBCMAsync(repositoryPath, businessDomains);
        }
    }
}