using System.ComponentModel;
using DigitalElectronics.Components.FlipFlops;

namespace DigitalElectronics.ViewModels.Components
{
    public class SRLatchWithEnableViewModel : INotifyPropertyChanged
    {
        private readonly ISRLatch _srLatch;
        private bool _reset;
        private bool _set;
        private bool _enable;

        public SRLatchWithEnableViewModel()
            : this(new SRLatch())
        { }

        public SRLatchWithEnableViewModel(ISRLatch srLatch)
        {
            _srLatch = srLatch ?? throw new ArgumentNullException(nameof(srLatch));
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
                    _srLatch.SetInputR(value);
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
                    _srLatch.SetInputS(value);
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Enable)));
                }
            }
        }

        public bool OutputQ => _srLatch.OutputQ;

        public bool OutputNQ => _srLatch.OutputNQ;
    }
}
