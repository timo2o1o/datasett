using System.ComponentModel;
using DataSett.Metamodel;
using System.Runtime.CompilerServices;

namespace DataSett.ViewModel;

public class BusinessConceptRelationDisplayItem : INotifyPropertyChanged
{
    private readonly BusinessConceptRelation _businessConceptRelation;

    public BusinessConceptRelationDisplayItem(BusinessConceptRelation businessConceptRelation)
    {
        _businessConceptRelation = businessConceptRelation;
    }

    public BusinessConceptRelation BusinessConceptRelation => _businessConceptRelation;

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