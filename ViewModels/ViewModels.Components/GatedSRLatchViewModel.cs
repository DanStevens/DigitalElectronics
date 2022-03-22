using System.ComponentModel;
using DigitalElectronics.Components.FlipFlops;

namespace DigitalElectronics.ViewModels.Components
{
    public class GatedSRLatchViewModel : INotifyPropertyChanged
    {
        private readonly IGatedSRLatch _gatedSRLatch;
        private bool _reset;
        private bool _set;
        private bool _enable;

        public GatedSRLatchViewModel()
            : this(new GatedSRLatch())
        { }

        public GatedSRLatchViewModel(IGatedSRLatch gatedSRLatch)
        {
            _gatedSRLatch = gatedSRLatch ?? throw new ArgumentNullException(nameof(gatedSRLatch));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool Reset
        {
            get => _reset;
            set
            {
                if (_reset != value)
                {
                    _reset = value;
                    _gatedSRLatch.SetInputR(value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Reset)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputQ)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputNQ)));
                }
            }
        }


        public bool Set
        {
            get => _set;
            set
            {
                if (_set != value)
                {
                    _set = value;
                    _gatedSRLatch.SetInputS(value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Set)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputQ)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputNQ)));
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
                    _gatedSRLatch.SetInputE(_enable);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Enable)));
                }
            }
        }

        public bool OutputQ => _gatedSRLatch.OutputQ;

        public bool OutputNQ => _gatedSRLatch.OutputNQ;
    }
}
