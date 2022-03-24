using System.Windows;

namespace DigitalElectronics.Demos.Components
{
    /// <summary>
    /// Interaction logic for GatedSRLatchDemo.xaml
    /// </summary>
    public partial class DFlipFlopDemo : Window
    {
        public DFlipFlopDemo()
        {
            InitializeComponent();
        }

        private void ClockButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Clock();
        }
    }
}
