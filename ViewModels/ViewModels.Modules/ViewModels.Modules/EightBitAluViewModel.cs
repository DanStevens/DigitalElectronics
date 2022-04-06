using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.ALUs;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Modules.Annotations;

namespace DigitalElectronics.ViewModels.Modules
{
    public class EightBitAluViewModel : IAluViewModel
    {
        private const int _NumberOfBits = 8;

        private readonly IArithmeticLogicUnit _alu;
        private bool _enable;
        private bool _subtract;
        private ObservableCollection<bool> _probe;

        public EightBitAluViewModel()
            : this(new ArithmeticLogicUnit(_NumberOfBits))
        {
        }

        public EightBitAluViewModel(IArithmeticLogicUnit alu)
        {
            _alu = alu ?? throw new ArgumentNullException(nameof(alu));
            _probe = new ObservableCollection<bool>(_alu.ProbeState());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? EnableChanged;
        public int NumberOfBits => _NumberOfBits;

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
                    RaisePropertyChanged(nameof(OutputE));
                    EnableChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public ReadOnlyObservableCollection<bool>? OutputE =>
            Enable ? new ReadOnlyObservableCollection<bool>(_probe) : null;

        IReadOnlyList<bool>? IAluViewModel.OutputE => OutputE;

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

        IReadOnlyList<bool> IAluViewModel.Probe => Probe;

        public void SetInputA(IEnumerable<bool> value)
        {
            _alu.SetInputA(new BitArray(value));
            Sync();
        }

        public void SetInputB(IEnumerable<bool> value)
        {
            _alu.SetInputB(new BitArray(value));
            Sync();
        }

        private void Sync()
        {
            _probe = new ObservableCollection<bool>(_alu.ProbeState());
            RaisePropertyChanged(nameof(Probe));
            if (Enable) RaisePropertyChanged(nameof(OutputE));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
