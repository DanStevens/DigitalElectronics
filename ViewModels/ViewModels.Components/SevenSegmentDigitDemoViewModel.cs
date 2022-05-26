using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Concepts;
using DigitalElectronics.ViewModels.Utilities;

namespace DigitalElectronics.ViewModels.Components;

public class SevenSegmentDigitDemoViewModel : INotifyPropertyChanged
{
    private FullyObservableCollection<Bit>? _segmentLines;

    public SevenSegmentDigitDemoViewModel()
    {
        SegmentLines = new FullyObservableCollection<Bit>(Enumerable.Range(0, 7).Select(_ => new Bit(true)));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public FullyObservableCollection<Bit>? SegmentLines
    {
        get => _segmentLines;
        set
        {
            if (_segmentLines?.SequenceEqual(value) != true)
            {
                if (_segmentLines != null) _segmentLines.ItemPropertyChanged -= OnSegmentLineChanged;
                _segmentLines = value;
                if (_segmentLines != null) _segmentLines.ItemPropertyChanged += OnSegmentLineChanged;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SegmentLinesAsBools));
            }
        }
    }

    private void OnSegmentLineChanged(object? sender, ItemPropertyChangedEventArgs e)
    {
        RaisePropertyChanged(nameof(SegmentLinesAsBools));
    }

    public ICollection<bool>? SegmentLinesAsBools => SegmentLines?.Select(i => (bool)i)?.ToArray();

    protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
