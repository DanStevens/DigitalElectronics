using System.ComponentModel;
using DigitalElectronics.Components.FlipFlops;

namespace DigitalElectronics.ViewModels.Components
{
    public class DFlipFlopViewModel : INotifyPropertyChanged
    {
        private readonly IDFlipFlop _dFlipFlop;
        private bool _data;

        public DFlipFlopViewModel()
            : this(new DFlipFlop())
        { }

        public DFlipFlopViewModel(IDFlipFlop dFlipFlop)
        {
            _dFlipFlop = dFlipFlop ?? throw new ArgumentNullException(nameof(dFlipFlop));
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
                    _dFlipFlop.SetInputD(_data);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Data)));
                }
            }
        }


        public bool OutputQ => _dFlipFlop.OutputQ;

        public bool OutputNQ => _dFlipFlop.OutputNQ
        ;
        public void Clock()
        {
            _dFlipFlop.Clock();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputQ)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputNQ)));
        }
    }
}
