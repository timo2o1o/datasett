using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataSett.ViewModel.DisplayItems
{
    public abstract class DisplayitemBase<T> : INotifyPropertyChanged
    {

        protected T? _existingItem;

        public DisplayitemBase(T? existingItem)
        {
            _existingItem = existingItem;
        }

        public bool IsPersisted => _existingItem != null;

        public abstract bool IsDirty { get; }

        /// <summary>
        /// Applies all pending changes to the underlying data source. Call this at save time.
        /// </summary>
        /// <remarks>Derived classes must implement this method to define how changes are committed.
        /// Callers should ensure that all necessary validations are performed before invoking this method, as it may
        /// modify the state of the data source.</remarks>
        public abstract void ApplyChanges();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<TField>(ref TField field, TField value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<TField>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            OnPropertyChanged(nameof(IsDirty));
            return true;
        }

    }
}
