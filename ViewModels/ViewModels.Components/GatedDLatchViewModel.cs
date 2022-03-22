using System.ComponentModel;
using DigitalElectronics.Components.FlipFlops;

namespace DigitalElectronics.ViewModels.Components
{
    public class GatedDLatchViewModel : INotifyPropertyChanged
    {
        private readonly IGatedDLatch _gatedDLatch;
        private bool _data;
        private bool _enable;

        public GatedDLatchViewModel()
            : this(new GatedDLatch())
        { }

        public GatedDLatchViewModel(IGatedDLatch gatedDLatch)
        {
            _gatedDLatch = gatedDLatch ?? throw new ArgumentNullException(nameof(gatedDLatch));
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
                    _gatedDLatch.SetInputD(_data);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Data)));
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
                    _gatedDLatch.SetInputE(_enable);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Enable)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Data)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputQ)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputNQ)));
                }
            }
        }

        public bool OutputQ => _gatedDLatch.OutputQ;

        public bool OutputNQ => _gatedDLatch.OutputNQ;
    }
}
