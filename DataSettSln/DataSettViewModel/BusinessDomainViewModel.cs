using DataSett.Metamodel;
using System.Collections.ObjectModel;

namespace DataSett.ViewModel
{
    public class BusinessDomainViewModel : BaseViewModel
    {
        private BusinessDomain _model;
        private string? _name;
        private BusinessDomainViewModel? _hierarchy;
        private ObservableCollection<BusinessObjectViewModel> _businessObjects;
        private ObservableCollection<BusinessObjectRelationViewModel> _businessRelations;

        public BusinessDomainViewModel() : this(new BusinessDomain())
        {
        }

        public BusinessDomainViewModel(BusinessDomain model)
        {
            _model = model;
            _name = model.Name;
            _businessObjects = new ObservableCollection<BusinessObjectViewModel>();
            _businessRelations = new ObservableCollection<BusinessObjectRelationViewModel>();

            if (model.BusinessObjects != null)
            {
                foreach (var bo in model.BusinessObjects)
                {
                    _businessObjects.Add(new BusinessObjectViewModel(bo));
                }
            }

            if (model.BusinessRelations != null)
            {
                foreach (var br in model.BusinessRelations)
                {
                    _businessRelations.Add(new BusinessObjectRelationViewModel(br));
                }
            }

            if (model.Hierarchy != null)
            {
                _hierarchy = new BusinessDomainViewModel(model.Hierarchy);
            }
        }

        public BusinessDomain Model => _model;

        public string? Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    _model.Name = value;
                }
            }
        }

        public BusinessDomainViewModel? Hierarchy
        {
            get => _hierarchy;
            set
            {
                if (SetProperty(ref _hierarchy, value))
                {
                    _model.Hierarchy = value?.Model;
                }
            }
        }

        public ObservableCollection<BusinessObjectViewModel> BusinessObjects
        {
            get => _businessObjects;
            set => SetProperty(ref _businessObjects, value);
        }

        public ObservableCollection<BusinessObjectRelationViewModel> BusinessRelations
        {
            get => _businessRelations;
            set => SetProperty(ref _businessRelations, value);
        }

        public void AddBusinessObject(BusinessObjectViewModel businessObject)
        {
            _businessObjects.Add(businessObject);
            _model.BusinessObjects?.Add(businessObject.Model);
            _model.BusinessObjectIds.Add(businessObject.Id ?? string.Empty);
        }

        public void RemoveBusinessObject(BusinessObjectViewModel businessObject)
        {
            _businessObjects.Remove(businessObject);
            _model.BusinessObjects?.Remove(businessObject.Model);
            _model.BusinessObjectIds.Remove(businessObject.Id ?? string.Empty);
        }

        public void AddBusinessRelation(BusinessObjectRelationViewModel relation)
        {
            _businessRelations.Add(relation);
            _model.BusinessRelations?.Add(relation.Model);
        }

        public void RemoveBusinessRelation(BusinessObjectRelationViewModel relation)
        {
            _businessRelations.Remove(relation);
            _model.BusinessRelations?.Remove(relation.Model);
        }
    }
}
