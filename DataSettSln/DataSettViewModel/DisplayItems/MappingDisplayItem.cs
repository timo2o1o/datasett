using DataSett.Metamodel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataSett.ViewModel;

/// <summary>
/// UI wrapper that presents a unified view of both persisted and unmapped source attributes.
/// </summary>
public class MappingDisplayItem : INotifyPropertyChanged
{
    private readonly BusinessConceptMapping? _existingMapping;
    private readonly SourceAttribute _sourceAttribute;

    // Editable backing fields for new/unmapped items
    private BusinessConcept? _parentBusinessConcept;
    private string? _harmonizedName;
    private int? _orderNo;
    private SourceAttributeRole _role;
    private HistoryType _historyType;
    private string _attributeSetName;
    private AttributeProperties _mappingProperties;

    public MappingDisplayItem(BusinessConceptMapping existingMapping)
    {
        _existingMapping = existingMapping;
        _sourceAttribute = existingMapping.SourceAttribute!;
        
        // Initialize from existing mapping
        _parentBusinessConcept = existingMapping.ParentBusinessConcept;
        _harmonizedName = existingMapping.HarmonizedName;
        _orderNo = existingMapping.OrderNo;
        _role = existingMapping.Role;
        _historyType = existingMapping.HistoryType;
        _attributeSetName = existingMapping.AttributeSetName;
        _mappingProperties = existingMapping.MappingProperties;
    }

    public MappingDisplayItem(SourceAttribute unmappedAttribute)
    {
        _existingMapping = null;
        _sourceAttribute = unmappedAttribute;
        
        // Initialize defaults for new mapping
        _parentBusinessConcept = null;
        _harmonizedName = unmappedAttribute.Name;
        _orderNo = null;
        _role = SourceAttributeRole.Unclassified;
        _historyType = HistoryType.None;
        _attributeSetName = "Default";
        _mappingProperties = unmappedAttribute.AttributeProperties.Copy();
    }

    public SourceAttribute SourceAttribute => _sourceAttribute;
    
    public bool IsPersisted => _existingMapping != null;
    
    public bool IsDirty => _parentBusinessConcept != _existingMapping?.ParentBusinessConcept
                        || _harmonizedName != _existingMapping?.HarmonizedName
                        || _orderNo != _existingMapping?.OrderNo
                        || _role != _existingMapping?.Role
                        || _historyType != _existingMapping?.HistoryType;

    // Editable properties for UI binding
    public BusinessConcept? ParentBusinessConcept
    {
        get => _parentBusinessConcept;
        set => SetField(ref _parentBusinessConcept, value);
    }

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

    /// <summary>
    /// Applies changes to the domain model. Call this at save time.
    /// </summary>
    public void ApplyChanges()
    {
        if (!IsDirty && IsPersisted) return;

        // Remove from old parent if it was persisted and parent changed
        if (_existingMapping != null 
            && _existingMapping.ParentBusinessConcept != null 
            && _existingMapping.ParentBusinessConcept != _parentBusinessConcept)
        {
            _existingMapping.ParentBusinessConcept.BusinessConceptMappings.Remove(_existingMapping);
        }

        if (_parentBusinessConcept != null)
        {
            var mapping = _existingMapping ?? new BusinessConceptMapping { SourceAttribute = _sourceAttribute };
            
            // Update all properties
            mapping.ParentBusinessConcept = _parentBusinessConcept;
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

    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        OnPropertyChanged(nameof(IsDirty));
        return true;
    }
}