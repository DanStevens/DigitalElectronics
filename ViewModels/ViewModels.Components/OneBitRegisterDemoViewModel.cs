using System.ComponentModel;
using DigitalElectronics.Components.FlipFlops;
using DigitalElectronics.Components.Memory;

namespace DigitalElectronics.ViewModels.Components
{
    public class OneBitRegisterDemoViewModel : INotifyPropertyChanged
    {
        private readonly IRegisterBit _bitRegister;
        private bool _data;
        private bool _enable;
        private bool _load;

        public OneBitRegisterDemoViewModel()
            : this(new RegisterBit())
        { }

        public OneBitRegisterDemoViewModel(IRegisterBit bitRegister)
        {
            _bitRegister = bitRegister ?? throw new ArgumentNullException(nameof(bitRegister));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool Data
        {
            get => _data;
            set
            {
                if (_data != value)
                {
                    _data = value;
                    _bitRegister.SetInputD(_data);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Data)));
                }
            }
        }

        public bool Load
        {
            get => _load;
            set
            {
                if (_load != value)
                {
                    _load = value;
                    _bitRegister.SetInputL(value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Load)));
                }
            }
        }

        public bool Enable
        {
            get => _enable;
            set
            {
                if (_enable != value)
                {
                    _enable = value;
                    _bitRegister.SetInputE(value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Enable)));
                }
            }
        }

        public bool? OutputQ => _bitRegister.OutputQ;

        public bool ProbeQ => _bitRegister.ProbeState();

        public void Clock()
        {
            if (Load && Enable)
                throw new InvalidOperationException("Load and Enable should not both be set high at the same time");

            _bitRegister.Clock();
            if (Load) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProbeQ)));
            if (Enable) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputQ)));
        }
    }
}
