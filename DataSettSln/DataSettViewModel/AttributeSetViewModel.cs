using DataSett.Metamodel;

namespace DataSett.ViewModel
{
    public class AttributeSetViewModel : BaseViewModel
    {
        private AttributeSet _model;
        private string? _id;
        private string? _name;
        private string? _businessObjectId;

        public AttributeSetViewModel() : this(new AttributeSet())
        {
        }

        public AttributeSetViewModel(AttributeSet model)
        {
            _model = model;
            _id = model.Id;
            _name = model.Name;
            _businessObjectId = model.BusinessObjectId;
        }

        public AttributeSet Model => _model;

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

        public string? BusinessObjectId
        {
            get => _businessObjectId;
            set
            {
                if (SetProperty(ref _businessObjectId, value))
                {
                    _model.BusinessObjectId = value;
                }
            }
        }
    }
}
