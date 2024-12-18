using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Output;
using DigitalElectronics.ViewModels.Utilities;
using DigitalElectronics.Utilities;

namespace DigitalElectronics.ViewModels.Modules
{
    public class FourDigit7SegmentDisplayViewModel : INotifyPropertyChanged
    {
        private const int DigitWordSize = 8;

        private readonly ByteTo4DigitMultiplexedDisplayDecoder _displayDecoder = new();
        //private readonly BinaryCounter _counter = new(2);
        //private readonly TwoBitAddressDecoder _digitSelector = new();

        private readonly Register _registerForDigit0 = new(DigitWordSize);
        private readonly Register _registerForDigit1 = new(DigitWordSize);
        private readonly Register _registerForDigit2 = new(DigitWordSize);
        private readonly Register _registerForDigit3 = new(DigitWordSize);

        public FourDigit7SegmentDisplayViewModel()
        {
            //System.Diagnostics.Debug.Assert(_counter.Output.ToByte() == 3);
            //SyncDigitSelector();
            System.Diagnostics.Debug.Assert(_displayDecoder.DigitActivateStates.ToByte() == 0b0001);

            InitializeRegister(_registerForDigit0);
            InitializeRegister(_registerForDigit1);
            InitializeRegister(_registerForDigit2);
            InitializeRegister(_registerForDigit3);

            var initialValue = new BitArray((byte)0);
            Value = new FullyObservableCollection<Bit>(initialValue.AsEnumerable<Bit>());
            Value.ItemPropertyChanged += OnValueBitChanged;

            SyncValue();

            void InitializeRegister(Register register)
            {
                register.SetInputL(true);
                register.SetInputE(true);
            }
        }


        private void OnValueBitChanged(object? sender, ItemPropertyChangedEventArgs e)
        {
            SyncValue();
        }

        public FullyObservableCollection<Bit> Value { get; }

        public IList<bool> LinesForDigit0 => _registerForDigit0.Output!.Value.ToArray();
        public IList<bool> LinesForDigit1 => _registerForDigit1.Output!.Value.ToArray();
        public IList<bool> LinesForDigit2 => _registerForDigit2.Output!.Value.ToArray();
        public IList<bool> LinesForDigit3 => _registerForDigit3.Output!.Value.ToArray();

        private double updateInterval = 10;

        /// <summary>
        /// Interval at which to update (<see cref="Clock"/>) the display, in milliseconds.
        /// </summary>
        public double UpdateInterval
        {
            get => updateInterval;
            set
            {
                if (updateInterval != value)
                {
                    updateInterval = value;
                    RaisePropertyChanged(nameof(UpdateInterval));
                    UpdateIntervalChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void Clock()
        {
            _registerForDigit0.SetInputL(_displayDecoder.DigitActivateStates[0]);
            _registerForDigit1.SetInputL(_displayDecoder.DigitActivateStates[1]);
            _registerForDigit2.SetInputL(_displayDecoder.DigitActivateStates[2]);
            _registerForDigit3.SetInputL(_displayDecoder.DigitActivateStates[3]);

            _registerForDigit0.Clock();
            _registerForDigit1.Clock();
            _registerForDigit2.Clock();
            _registerForDigit3.Clock();

            _displayDecoder.Clock();
            SyncValue();

            RaisePropertyChanged(nameof(LinesForDigit0));
            RaisePropertyChanged(nameof(LinesForDigit1));
            RaisePropertyChanged(nameof(LinesForDigit2));
            RaisePropertyChanged(nameof(LinesForDigit3));
        }

        private void SyncValue()
        {
            _displayDecoder.SetInput(BitArray.FromList(Value));
            _registerForDigit0.SetInputD(_displayDecoder.Output);
            _registerForDigit1.SetInputD(_displayDecoder.Output);
            _registerForDigit2.SetInputD(_displayDecoder.Output);
            _registerForDigit3.SetInputD(_displayDecoder.Output);
        }

        public event EventHandler? UpdateIntervalChanged;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
