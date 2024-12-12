using System;
using System.ComponentModel;

namespace DigitalElectronics.BenEater.Computers.ViewModels
{
    public class ModuleViewModel : INotifyPropertyChanged
    {
        public virtual void Clock() { }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
