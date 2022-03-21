using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DigitalElectronics.Demos
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
