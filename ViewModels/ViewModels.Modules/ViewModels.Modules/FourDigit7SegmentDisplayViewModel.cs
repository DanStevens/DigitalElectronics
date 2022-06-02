using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Modules.Output;
using DigitalElectronics.ViewModels.Modules.Annotations;

namespace DigitalElectronics.ViewModels.Modules
{
    public class FourDigit7SegmentDisplayViewModel : INotifyPropertyChanged
    {
        private readonly ByteTo4DigitMultiplexedDisplayDecoder _displayDecoder = new();

        private ObservableCollection<bool> bools;

        public ObservableCollection<bool> Value
        {
            get => bools;
            set
            {
                if (bools != value)
                {
                    bools = value;
                    RaisePropertyChanged(nameof(Value));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
