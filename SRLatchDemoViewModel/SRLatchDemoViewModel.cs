using System.ComponentModel;
using DigitalElectronics.Components.FlipFlops;

namespace SRLatchDemoViewModels
{
    public class SRLatchDemoViewModel : INotifyPropertyChanged
    {
        private readonly ISRLatch _srLatch;
        private bool _reset;
        private bool _set;

        public SRLatchDemoViewModel()
            :this(new SRLatch())
        {}

        public SRLatchDemoViewModel(ISRLatch srLatch)
        {
            _srLatch = srLatch ?? throw new ArgumentNullException(nameof(srLatch));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool Reset {
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

        public bool OutputQ => _srLatch.OutputQ;

        public bool OutputNQ => _srLatch.OutputNQ;
    }
}
