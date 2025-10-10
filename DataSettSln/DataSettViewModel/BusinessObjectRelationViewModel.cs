using DataSett.Metamodel;
using System.Collections.ObjectModel;

namespace DataSett.ViewModel
{
    public class BusinessObjectRelationViewModel : BaseViewModel
    {
        private BusinessObjectRelation _model;
        private string? _name;
        private ObservableCollection<BusinessObjectRelationItemViewModel> _relatedKeys;

        public BusinessObjectRelationViewModel() : this(new BusinessObjectRelation())
        {
        }

        public BusinessObjectRelationViewModel(BusinessObjectRelation model)
        {
            _model = model;
            _name = model.Name;
            _relatedKeys = new ObservableCollection<BusinessObjectRelationItemViewModel>();

            if (model.RelatedKeys != null)
            {
                foreach (var key in model.RelatedKeys)
                {
                    _relatedKeys.Add(new BusinessObjectRelationItemViewModel(key));
                }
            }
        }

        public BusinessObjectRelation Model => _model;

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

        public ObservableCollection<BusinessObjectRelationItemViewModel> RelatedKeys
        {
            get => _relatedKeys;
            set => SetProperty(ref _relatedKeys, value);
        }

        public void AddRelatedKey(BusinessObjectRelationItemViewModel item)
        {
            _relatedKeys.Add(item);
            _model.RelatedKeys?.Add(item.Model);
        }

        public void RemoveRelatedKey(BusinessObjectRelationItemViewModel item)
        {
            _relatedKeys.Remove(item);
            _model.RelatedKeys?.Remove(item.Model);
        }
    }
}
