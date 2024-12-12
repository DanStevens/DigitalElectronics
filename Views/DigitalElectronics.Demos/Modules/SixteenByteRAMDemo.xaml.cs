using System;
using System.Windows;

namespace DigitalElectronics.Demos.Modules
{
    /// <summary>
    /// Interaction logic for SixteenByteRAMDemo.xaml
    /// </summary>
    public partial class SixteenByteRAMDemo : Window
    {
        public SixteenByteRAMDemo()
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
