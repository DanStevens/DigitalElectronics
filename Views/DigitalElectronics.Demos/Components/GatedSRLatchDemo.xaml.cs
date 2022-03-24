using System.Windows;
using System.Windows.Input;

namespace DigitalElectronics.Demos.Components
{
    /// <summary>
    /// Interaction logic for GatedSRLatchDemo.xaml
    /// </summary>
    public partial class GatedSRLatchDemo : Window
    {
        public GatedSRLatchDemo()
        {
            InitializeComponent();
        }

        private void ResetButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _viewModel.Reset = true;
        }

        private void ResetButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _viewModel.Reset = false;
        }

        private void SetButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _viewModel.Set = true;
        }

        private void SetButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _viewModel.Set = false;
        }
    }
}
