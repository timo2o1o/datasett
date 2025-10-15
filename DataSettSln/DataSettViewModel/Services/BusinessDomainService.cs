using DataSett.Metamodel;
using System.Collections.ObjectModel;

namespace DataSett.ViewModel.Services
{
    public class BusinessDomainService
    {
        private readonly ObservableCollection<BusinessDomainViewModel> _domains;

        public BusinessDomainService()
        {
            _domains = new ObservableCollection<BusinessDomainViewModel>();
        }

        public ObservableCollection<BusinessDomainViewModel> Domains => _domains;

        public BusinessDomainViewModel CreateDomain(string name)
        {
            var domain = new BusinessDomain(name);
            var viewModel = new BusinessDomainViewModel(domain);
            _domains.Add(viewModel);
            return viewModel;
        }

        public void DeleteDomain(BusinessDomainViewModel domain)
        {
            _domains.Remove(domain);
        }

        public BusinessDomainViewModel? GetDomainByName(string name)
        {
            return _domains.FirstOrDefault(d => d.Name == name);
        }
    }
}
