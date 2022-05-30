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
    }

    public BySegmentDigitDemoViewModel BySegmentDigitDemo { get; }

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
}
