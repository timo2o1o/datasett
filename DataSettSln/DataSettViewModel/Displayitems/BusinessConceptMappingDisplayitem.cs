using DataSett.Metamodel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataSett.ViewModel.DisplayItems;

/// <summary>
/// UI wrapper that presents a unified view of both persisted and unmapped source attributes.
/// </summary>
public class BusinessConceptMappingDisplayitem : DisplayitemBase<BusinessConceptMapping>
{
    private readonly SourceAttribute _sourceAttribute;

    // Editable backing fields for new/unmapped items
    private BusinessConcept? _parentBusinessConcept;
    private BusinessConceptKeyPart? _assignedKeyPart;
    private string? _harmonizedName;
    private int? _orderNo;
    private SourceAttributeRole _role;
    private HistoryType _historyType;
    private string _attributeSetName;
    private AttributeProperties _mappingProperties;

    public BusinessConceptMappingDisplayitem(BusinessConceptMapping existingMapping) : base(existingMapping)
    {
        _sourceAttribute = existingMapping.SourceAttribute!;
        
        // Initialize from existing mapping
        _parentBusinessConcept = existingMapping.ParentBusinessConcept;
        _assignedKeyPart = existingMapping.AssignedKeyPart;
        _harmonizedName = existingMapping.HarmonizedName;
        _orderNo = existingMapping.OrderNo;
        _role = existingMapping.Role;
        _historyType = existingMapping.HistoryType;
        _attributeSetName = existingMapping.AttributeSetName;
        _mappingProperties = existingMapping.MappingProperties;
    }

    public BusinessConceptMappingDisplayitem(SourceAttribute unmappedAttribute) : base(null)
    {
        _sourceAttribute = unmappedAttribute;
        
        // Initialize defaults for new mapping
        _parentBusinessConcept = null;
        _assignedKeyPart = null;
        _harmonizedName = unmappedAttribute.Name;
        _orderNo = null;
        _role = SourceAttributeRole.Unclassified;
        _historyType = HistoryType.None;
        _attributeSetName = "Default";
        _mappingProperties = unmappedAttribute.AttributeProperties.Copy();
    }

    public SourceAttribute SourceAttribute => _sourceAttribute;
    
    // Editable properties for UI binding
    public BusinessConcept? ParentBusinessConcept
    {
        get => _parentBusinessConcept;
        set
        {
            if (SetField(ref _parentBusinessConcept, value))
            {
                // Auto-assign the key part if there's exactly one available and the role is BUsinessKey,
                // otherwise clear it since key parts are specific to a business concept
                var keyParts = value?.KeyParts;
                AssignedKeyPart = keyParts?.Count == 1 && Role == SourceAttributeRole.BusinessKey ? keyParts[0] : null;
                OnPropertyChanged(nameof(AvailableKeyParts));
            }
        }
    }

    public BusinessConceptKeyPart? AssignedKeyPart
    {
        get => _assignedKeyPart;
        set => SetField(ref _assignedKeyPart, value);
    }

    /// <summary>
    /// Returns the list of key parts available for selection based on the current ParentBusinessConcept.
    /// </summary>
    public IEnumerable<BusinessConceptKeyPart> AvailableKeyParts =>
        _parentBusinessConcept?.KeyParts ?? Enumerable.Empty<BusinessConceptKeyPart>();

    public string? HarmonizedName
    {
        get => _harmonizedName;
        set => SetField(ref _harmonizedName, value);
    }

    public int? OrderNo
    {
        get => _orderNo;
        set => SetField(ref _orderNo, value);
    }

    public bool IsHistoryTypeEnabled()
    {
        return (Role != SourceAttributeRole.BusinessKey && Role != SourceAttributeRole.SelfReferencedBusinessKey);
    }

    public bool IsAttributeSetNameEnabled()
    {
        return (Role == SourceAttributeRole.Descriptive);
    }

    public SourceAttributeRole Role
    {
        get => _role;
        set => SetField(ref _role, value);
    }

    public HistoryType HistoryType
    {
        get => _historyType;
        set => SetField(ref _historyType, value);
    }

    public string AttributeSetName
    {
        get => _attributeSetName;
        set => SetField(ref _attributeSetName, value);
    }

    public AttributeProperties MappingProperties
    {
        get => _mappingProperties;
        set => SetField(ref _mappingProperties, value);
    }

    public override void ApplyChanges()
    {
        if (!IsDirty && IsPersisted) return;

        // Remove from old parent if it was persisted and parent changed
        if (_existingItem != null 
            && _existingItem.ParentBusinessConcept != null 
            && _existingItem.ParentBusinessConcept != _parentBusinessConcept)
        {
            _existingItem.ParentBusinessConcept.BusinessConceptMappings.Remove(_existingItem);
        }

        if (_parentBusinessConcept != null)
        {
            var mapping = _existingItem ?? new BusinessConceptMapping { SourceAttribute = _sourceAttribute };
            
            // Update all properties
            mapping.ParentBusinessConcept = _parentBusinessConcept;
            mapping.AssignedKeyPart = _assignedKeyPart;
            mapping.HarmonizedName = _harmonizedName;
            mapping.OrderNo = _orderNo;
            mapping.Role = _role;
            mapping.HistoryType = _historyType;
            mapping.AttributeSetName = _attributeSetName;
            mapping.MappingProperties = _mappingProperties;

            if (!_parentBusinessConcept.BusinessConceptMappings.Contains(mapping))
            {
                _parentBusinessConcept.BusinessConceptMappings.Add(mapping);
            }
        }
    }

    public override bool IsDirty
    {
        get
        {
            if (_existingItem == null)
            {
                // Unmapped attributes are only dirty when the user has assigned a business concept
                return _parentBusinessConcept != null;
            }
            return _parentBusinessConcept != _existingItem.ParentBusinessConcept
                || _assignedKeyPart != _existingItem.AssignedKeyPart
                || _harmonizedName != _existingItem.HarmonizedName
                || _orderNo != _existingItem.OrderNo
                || _role != _existingItem.Role
                || _historyType != _existingItem.HistoryType;
        }
    }

}