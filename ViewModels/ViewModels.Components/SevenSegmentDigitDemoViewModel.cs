using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Concepts;
using DigitalElectronics.ViewModels.Utilities;

namespace DigitalElectronics.ViewModels.Components;

public class SevenSegmentDigitDemoViewModel
{
    public SevenSegmentDigitDemoViewModel()
    {
        BySegmentDigitDemo = new BySegmentDigitDemoViewModel();
        HexDigitDemo = new SingleHexDigitDemoViewModel();
    }

    public BySegmentDigitDemoViewModel BySegmentDigitDemo { get; }
    public SingleHexDigitDemoViewModel HexDigitDemo { get; }

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
        public SingleHexDigitDemoViewModel()
        {
            Value = new FullyObservableCollection<Bit>(Enumerable.Repeat(false, 4).Select(b => new Bit(b)));
            Value.ItemPropertyChanged += OnValueBitChanged;
        }

        public FullyObservableCollection<Bit> Value { get; }

        private void OnValueBitChanged(object? sender, ItemPropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(SegmentLines));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICollection<bool> SegmentLines => new BitArray((byte)0x3F).ToArray();
    }
}
