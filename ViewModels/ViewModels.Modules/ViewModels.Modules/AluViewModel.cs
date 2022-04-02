using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.ALUs;
using DigitalElectronics.ViewModels.Modules.Annotations;

namespace DigitalElectronics.ViewModels.Modules
{
    public class AluViewModel : IAluViewModel
    {
        private const int _NumberOfBits = 8;

        private readonly IArithmeticLogicUnit _alu;
        private bool _enable;
        private bool _subtract;
        private ObservableCollection<bool> _probe;

        public AluViewModel()
            : this(new ArithmeticLogicUnit(_NumberOfBits))
        {
        }

        public AluViewModel(IArithmeticLogicUnit alu)
        {
            _alu = alu ?? throw new ArgumentNullException(nameof(alu));
            _probe = new ObservableCollection<bool>(_alu.ProbeState());
        }

        public bool Enable
        {
            get => _enable;
            set
            {
                if (_enable != value)
                {
                    _enable = value;
                    _alu.SetInputEO(_enable);
                    RaisePropertyChanged();
                }
            }
        }

        public ReadOnlyObservableCollection<bool>? OutputE =>
            Enable ? new ReadOnlyObservableCollection<bool>(_probe) : null;

        public bool Subtract
        {
            get => _subtract;
            set
            {
                if (_subtract != value)
                {
                    _subtract = value;
                    _alu.SetInputSu(_subtract);
                    RaisePropertyChanged();
                }
            }
        }

        public ReadOnlyObservableCollection<bool> Probe => new(_probe);

        public void SetInputA(BitArray value)
        {
            _alu.SetInputA(value);
            RaisePropertyChanged(nameof(Probe));
            if (Enable) RaisePropertyChanged(nameof(OutputE));
        }

        public void SetInputB(BitArray value)
        {
            _alu.SetInputB(value);
            RaisePropertyChanged(nameof(Probe));
            if (Enable) RaisePropertyChanged(nameof(OutputE));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
