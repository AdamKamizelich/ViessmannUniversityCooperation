﻿namespace UniversityIot.UI.Mvvm
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using UniversityIot.UI.Core.Annotations;

    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}