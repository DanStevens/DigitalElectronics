using System;
using System.Windows;

namespace DigitalElectronics.Demos.Components
{
    /// <summary>
    /// Interaction logic for GatedSRLatchDemo.xaml
    /// </summary>
    public partial class OneBitRegisterDemo : Window
    {
        public OneBitRegisterDemo()
        {
            InitializeComponent();
        }

        private void ClockButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.Clock();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
