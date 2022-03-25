using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DigitalElectronics.Utilities;

namespace DigitalElectronics.UI.Controls
{
    /// <summary>
    /// Interaction logic for DipSwitch.xaml
    /// </summary>
    public partial class DipSwitch : UserControl
    {
        public DipSwitch()
        {
            InitializeComponent();
            _layoutRoot.DataContext = this;
        }

        public ObservableCollection<Bit> Lines
        {
            get { return (ObservableCollection<Bit>)GetValue(LinesProperty); }
            set { SetValue(LinesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinesProperty =
            DependencyProperty.Register("Lines", typeof(ObservableCollection<Bit>), typeof(DipSwitch), new PropertyMetadata(null));
    }
}
