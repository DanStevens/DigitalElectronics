using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Output;
using DigitalElectronics.ViewModels.Modules.Annotations;
using DigitalElectronics.ViewModels.Utilities;

namespace DigitalElectronics.ViewModels.Modules
{
    public class FourDigit7SegmentDisplayViewModel : INotifyPropertyChanged
    {
        private readonly ByteTo4DigitMultiplexedDisplayDecoder _displayDecoder = new();

        public FourDigit7SegmentDisplayViewModel()
        {
            var initialValue = new BitArray((byte)0);
            Value = new FullyObservableCollection<Bit>(initialValue.Select(b => new Bit(b)));
            Value.ItemPropertyChanged += OnValueBitChanged;
        }

        private void OnValueBitChanged(object? sender, ItemPropertyChangedEventArgs e)
        {
            _displayDecoder.SetInput(new BitArray(Value.ToArray()));
            RaisePropertyChanged(nameof(Lines));
        }

        public FullyObservableCollection<Bit> Value { get; }

        public IList<bool> Lines => _displayDecoder.Output.ToList();

        private double updateInterval = 1000;

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
            _displayDecoder.Clock();
            RaisePropertyChanged(nameof(Lines));
        }

        public event EventHandler? UpdateIntervalChanged;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
