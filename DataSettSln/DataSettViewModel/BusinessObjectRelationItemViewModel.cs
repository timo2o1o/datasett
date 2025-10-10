using DataSett.Metamodel;

namespace DataSett.ViewModel
{
    public class BusinessObjectRelationItemViewModel : BaseViewModel
    {
        private BusinessObjectRelationItem _model;
        private string? _parent;
        private string? _relatedKeyId;
        private bool? _isLeadingKey;

        public BusinessObjectRelationItemViewModel() : this(new BusinessObjectRelationItem())
        {
        }

        public BusinessObjectRelationItemViewModel(BusinessObjectRelationItem model)
        {
            _model = model;
            _parent = model.Parent;
            _relatedKeyId = model.RelatedKeyId;
            _isLeadingKey = model.IsLeadingKey;
        }

        public BusinessObjectRelationItem Model => _model;

        public string? Parent
        {
            get => _parent;
            set
            {
                if (SetProperty(ref _parent, value))
                {
                    _model.Parent = value;
                }
            }
        }

        public string? RelatedKeyId
        {
            get => _relatedKeyId;
            set
            {
                if (SetProperty(ref _relatedKeyId, value))
                {
                    _model.RelatedKeyId = value;
                }
            }
        }

        public bool? IsLeadingKey
        {
            get => _isLeadingKey;
            set
            {
                if (SetProperty(ref _isLeadingKey, value))
                {
                    _model.IsLeadingKey = value;
                }
            }
        }
    }
}
