using DataSett.Metamodel;
using DataSett.Metamodel.Serde;

namespace DataSett.ViewModel.Services
{
    public interface IMetaDataIOService
    {
        Task<IEnumerable<SourceSystem>> LoadSourceSystemsAsync(string repositoryPath);
        Task SaveSourceSystemsAsync(string repositoryPath, IEnumerable<SourceSystem> sourceSystems);
    }

    public class MetaDataIOService : IMetaDataIOService
    {
        private readonly JsonContext _dataContext;

        public MetaDataIOService()
        {
            _dataContext = new JsonContext();
        }

        public async Task<IEnumerable<SourceSystem>> LoadSourceSystemsAsync(string repositoryPath)
        {
            await _dataContext.LoadAsync(repositoryPath);
            return _dataContext.GetSourceSystems();
        }

        public async Task SaveSourceSystemsAsync(string repositoryPath, IEnumerable<SourceSystem> sourceSystems)
        {
            await _dataContext.SaveChangesAsync(repositoryPath, sourceSystems);
        }
    }
}