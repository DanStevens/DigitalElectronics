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
            DataContext = new SevenSegmentDigitDemoViewModel();
        }
    }

    public class SevenSegmentDigitDemoViewModel : INotifyPropertyChanged
    {
        private FullyObservableCollection<Bit>? _lines;

        public SevenSegmentDigitDemoViewModel()
        {
            Lines = new FullyObservableCollection<Bit>(Enumerable.Range(0, 7).Select(_ => new Bit(true)));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public FullyObservableCollection<Bit>? Lines
        {
            get => _lines;
            set
            {
                if (_lines?.SequenceEqual(value) != true)
                {
                    if (_lines != null) _lines.ItemPropertyChanged -= OnLineChanged;
                    _lines = value;
                    if (_lines != null) _lines.ItemPropertyChanged += OnLineChanged;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(LinesAsBools));
                }
            }
        }

        private void OnLineChanged(object? sender, ItemPropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(LinesAsBools));
        }

        public ICollection<bool>? LinesAsBools => Lines?.Select(i => (bool)i)?.ToArray();

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
