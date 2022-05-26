using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DigitalElectronics.Concepts;
using DigitalElectronics.ViewModels.Modules.Annotations;
using DigitalElectronics.ViewModels.Utilities;

namespace DigitalElectronics.Demos.Components
{
    /// <summary>
    /// Interaction logic for SeventSegmentDigitDemo.xaml
    /// </summary>
    public partial class SevenSegmentDigitDemo : Window
    {
        public SevenSegmentDigitDemo()
        {
            InitializeComponent();
            BySegmentLayoutRoot.DataContext = new SevenSegmentDigitDemoViewModel();
        }
    }

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

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
