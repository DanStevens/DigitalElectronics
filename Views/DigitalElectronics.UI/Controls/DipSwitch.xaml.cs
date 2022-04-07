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
using DigitalElectronics.Concepts;
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

        #region Orientation dependency property

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(DipSwitch),
                new FrameworkPropertyMetadata(
                    Orientation.Horizontal,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    UpdateDock));

        #endregion

        #region BitOrder dependency property

        public BitOrder BitOrder
        {
            get { return (BitOrder)GetValue(BitOrderProperty); }
            set { SetValue(BitOrderProperty, value); }
        }

        public static readonly DependencyProperty BitOrderProperty =
            DependencyProperty.Register("BitOrder", typeof(BitOrder), typeof(DipSwitch),
                new PropertyMetadata(BitOrder.MsbFirst, UpdateDock));

        #endregion

        #region Dock private read-only dependency property

        private Dock Dock
        {
            get { return (Dock)GetValue(DockProperty); }
            set { SetValue(DockPropertyKey, value); }
        }

        private static DependencyPropertyKey DockPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Dock), typeof(Dock), typeof(DipSwitch), new PropertyMetadata(Dock.Left));

        public static DependencyProperty DockProperty = DockPropertyKey.DependencyProperty;

        #endregion

        public ObservableCollection<Bit> Lines
        {
            get { return (ObservableCollection<Bit>)GetValue(LinesProperty); }
            set { SetValue(LinesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinesProperty =
            DependencyProperty.Register("Lines", typeof(ObservableCollection<Bit>), typeof(DipSwitch), new PropertyMetadata(null));

        private static void UpdateDock(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DipSwitch)d).Dock = ((DipSwitch)d).GetDock();
        }

        private Dock GetDock()
        {
            if (Orientation == Orientation.Vertical)
            {
                if (BitOrder == BitOrder.LsbFirst)
                    return Dock.Bottom;
                return Dock.Top;
            }

            if (BitOrder == BitOrder.LsbFirst)
                return Dock.Right;
            return Dock.Left;
        }
    }
}
