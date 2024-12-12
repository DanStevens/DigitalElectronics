using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using DigitalElectronics.Concepts;
using DigitalElectronics.ViewModels.Modules.Annotations;

namespace DigitalElectronics.Demos.Components
{

    /// <summary>
    /// Interaction logic for LedStripDemo.xaml
    /// </summary>
    public partial class DipSwitchDemo : Window
    {
        public DipSwitchDemo()
        {
            InitializeComponent();
            DataContext = new DipSwitchDemoViewModel();
        }
    }

    public class DipSwitchDemoViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Bit> _lines;

        public DipSwitchDemoViewModel()
        {
            _lines = new ObservableCollection<Bit>(Enumerable.Range(0, 8).Select(_ => new Bit()));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Bit> Lines
        {
            get => _lines;
            set
            {
                if (_lines.SequenceEqual(value))
                {
                    _lines = value;
                    RaisePropertyChanged();
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
