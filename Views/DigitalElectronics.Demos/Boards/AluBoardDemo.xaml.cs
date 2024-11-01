using System;
using System.Windows;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Demos.Boards
{
    /// <summary>
    /// Interaction logic for BusTransferDemoBoard.xaml
    /// </summary>
    public partial class AluBoardDemo : Window
    {
        public AluBoardDemo()
        {
            InitializeComponent();
        }

        private void ClockButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.Clock();
            }
            catch (BusContentionException ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.BusContention, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
