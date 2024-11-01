using System.Windows;

namespace DigitalElectronics.Demos.Components
{
    /// <summary>
    /// Interaction logic for SevenSegmentDigitDemo.xaml
    /// </summary>
    public partial class SevenSegmentDigitDemo : Window
    {
        public SevenSegmentDigitDemo()
        {
            InitializeComponent();
        }

        private void ClockButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.HexDigitWithRegisterDemo.Clock();
        }
    }
}
