using DataSett.Metamodel;
using System.Collections.ObjectModel;

namespace DataSett.ViewModel
{
    public class BusinessObjectViewModel : BaseViewModel
    {
        private BusinessObject _model;
        private string? _id;
        private string? _name;
        private ObservableCollection<AttributeSetViewModel> _attributeSets;

        public BusinessObjectViewModel() : this(new BusinessObject())
        {
        }

        public BusinessObjectViewModel(BusinessObject model)
        {
            _model = model;
            _id = model.Id;
            _name = model.Name;
            _attributeSets = new ObservableCollection<AttributeSetViewModel>();

            if (model.AttributeSets != null)
            {
                foreach (var attrSet in model.AttributeSets)
                {
                    _attributeSets.Add(new AttributeSetViewModel(attrSet));
                }
            }
        }

        public BusinessObject Model => _model;

        public string? Id
        {
            get => _id;
            set
            {
                if (SetProperty(ref _id, value))
                {
                    _model.Id = value;
                }
            }
        }

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

        public ObservableCollection<AttributeSetViewModel> AttributeSets
        {
            get => _attributeSets;
            set => SetProperty(ref _attributeSets, value);
        }

        public void AddAttributeSet(AttributeSetViewModel attributeSet)
        {
            _attributeSets.Add(attributeSet);
            _model.AttributeSets?.Add(attributeSet.Model);
            _model.AttributeSetIds.Add(attributeSet.Id ?? string.Empty);
        }

        public void RemoveAttributeSet(AttributeSetViewModel attributeSet)
        {
            _attributeSets.Remove(attributeSet);
            _model.AttributeSets?.Remove(attributeSet.Model);
            _model.AttributeSetIds.Remove(attributeSet.Id ?? string.Empty);
        }
    }
}
