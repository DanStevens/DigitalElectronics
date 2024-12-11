using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Memory;
using DigitalElectronics.ViewModels.Utilities;
using DigitalElectronics.Utilities;

namespace DigitalElectronics.ViewModels.Components;

public class SevenSegmentDigitDemoViewModel
{
    public SevenSegmentDigitDemoViewModel()
    {
        BySegmentDigitDemo = new BySegmentDigitDemoViewModel();
        HexDigitDemo = new SingleHexDigitDemoViewModel();
        HexDigitWithRegisterDemo = new SingleHexDigitWithRegisterDemoViewModel();
    }

    public BySegmentDigitDemoViewModel BySegmentDigitDemo { get; }
    public SingleHexDigitDemoViewModel HexDigitDemo { get; }
    public SingleHexDigitWithRegisterDemoViewModel HexDigitWithRegisterDemo { get; }

    public class BySegmentDigitDemoViewModel : INotifyPropertyChanged
    {
        public BySegmentDigitDemoViewModel()
        {
            SegmentLines = new FullyObservableCollection<Bit>(Enumerable.Range(0, 7).Select(_ => new Bit(true)));
            SegmentLines.ItemPropertyChanged += OnSegmentLineChanged;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public FullyObservableCollection<Bit>? SegmentLines { get; }

        public ICollection<bool>? SegmentLinesAsBools => SegmentLines?.Select(i => (bool)i)?.ToArray();

        private bool _digitIsActive = true;

        public bool DigitIsActive
        {
            get => _digitIsActive;
            set
            {
                if (_digitIsActive != value)
                {
                    _digitIsActive = value;
                    RaisePropertyChanged(nameof(DigitIsActive));
                }
            }
        }

        public virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnSegmentLineChanged(object? sender, ItemPropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(SegmentLinesAsBools));
        }
    }

    public class SingleHexDigitDemoViewModel : INotifyPropertyChanged
    {
        private ROM _valueToSegmentsROM = new (
            new byte[] { 0x3F, 0x06, 0x5B, 0x4F, 0x66, 0x6D, 0x7D, 0x07, 0x7F, 0x6F, 0x77, 0x7C, 0x39, 0x5E, 0x79, 0x71 }
        );

        public SingleHexDigitDemoViewModel()
        {
            Value = new FullyObservableCollection<Bit>(Enumerable.Repeat(false, 4).Select(b => new Bit(b)));
            Value.ItemPropertyChanged += OnValueBitChanged;
            Sync();
            _valueToSegmentsROM.SetInputE(true);
        }

        public FullyObservableCollection<Bit> Value { get; }

        private bool _digitIsActive = true;

        public bool DigitIsActive
        {
            get => _digitIsActive;
            set
            {
                if (_digitIsActive != value)
                {
                    _digitIsActive = value;
                    RaisePropertyChanged(nameof(DigitIsActive));
                }
            }
        }

        private void OnValueBitChanged(object? sender, ItemPropertyChangedEventArgs e)
        {
            Sync();
        }

        private void Sync()
        {
            var val = Value.ToBitArray();
            _valueToSegmentsROM.SetInputA(val);
            RaisePropertyChanged(nameof(SegmentLines));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICollection<bool> SegmentLines => _valueToSegmentsROM.Output!.Value.ToArray();
    }

    public class SingleHexDigitWithRegisterDemoViewModel : INotifyPropertyChanged
    {
        private ROM _valueToSegmentsROM = new(
            new byte[] { 0x3F, 0x06, 0x5B, 0x4F, 0x66, 0x6D, 0x7D, 0x07, 0x7F, 0x6F, 0x77, 0x7C, 0x39, 0x5E, 0x79, 0x71 }
        );

        private readonly Register _register = new (8);


        public SingleHexDigitWithRegisterDemoViewModel()
        {
            Value = new FullyObservableCollection<Bit>(Enumerable.Repeat(false, 4).Select(b => new Bit(b)));
            Value.ItemPropertyChanged += OnValueBitChanged;

            _valueToSegmentsROM.SetInputE(true);
            _valueToSegmentsROM.SetInputA(new BitArray(0, length: 4));

            Load = true;
            _register.SetInputD(_valueToSegmentsROM.Output!.Value);
            _register.Clock();
            _register.SetInputE(true);

            System.Diagnostics.Debug.Assert(_register.ProbeState().ToByte() == 0x3F);

            Sync();
        }

        private bool _digitIsActive = true;

        public bool DigitIsActive
        {
            get => _digitIsActive;
            set
            {
                if (_digitIsActive != value)
                {
                    _digitIsActive = value;
                    RaisePropertyChanged(nameof(DigitIsActive));
                }
            }
        }


        private bool _load;

        public bool Load
        {
            get => _load;
            set
            {
                if (_load != value)
                {
                    _load = value;
                    _register.SetInputL(value);
                    RaisePropertyChanged(nameof(Load));
                }
            }
        }

        public ICollection<bool> SegmentLines => _register.Output!.Value.ToArray();

        public ReadOnlyObservableCollection<bool> RegisterState =>
            new(new ObservableCollection<bool>(_register.ProbeState().AsEnumerable()));

        public FullyObservableCollection<Bit> Value { get; }

        public void Clock()
        {
            _register.Clock();
            RaisePropertyChanged(nameof(SegmentLines));
            RaisePropertyChanged(nameof(RegisterState));
        }

        private void OnValueBitChanged(object? sender, ItemPropertyChangedEventArgs e)
        {
            Sync();
        }

        private void Sync()
        {
            var val = Value.ToBitArray();
            _valueToSegmentsROM.SetInputA(val);
            _register.SetInputD(_valueToSegmentsROM.Output!.Value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
