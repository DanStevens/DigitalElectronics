using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for LedStrip.xaml
    /// </summary>
    public partial class LedStrip : UserControl
    {
        public LedStrip()
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
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(LedStrip),
                new PropertyMetadata(
                    Orientation.Horizontal,
                    UpdateDock));

        #endregion

        #region BitOrder dependency property

        public BitOrder BitOrder
        {
            get { return (BitOrder)GetValue(BitOrderProperty); }
            set { SetValue(BitOrderProperty, value); }
        }

        public static readonly DependencyProperty BitOrderProperty =
            DependencyProperty.Register("BitOrder", typeof(BitOrder), typeof(LedStrip),
                new PropertyMetadata(BitOrder.MsbFirst, UpdateDock));

        #endregion

        #region Dock private read-only dependency property

        private Dock Dock
        {
            get { return (Dock)GetValue(DockProperty); }
            set { SetValue(DockPropertyKey, value); }
        }

        private static DependencyPropertyKey DockPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Dock), typeof(Dock), typeof(LedStrip), new PropertyMetadata(Dock.Left));

        public static DependencyProperty DockProperty = DockPropertyKey.DependencyProperty;

        #endregion

        #region Lines dependency property

        public ICollection<bool> Lines
        {
            get { return (ICollection<bool>)GetValue(LinesProperty); }
            set { SetValue(LinesProperty, value); }
        }

        public static readonly DependencyProperty LinesProperty =
            DependencyProperty.Register("Lines", typeof(ICollection<bool>), typeof(LedStrip),
                new PropertyMetadata(null, OnLinesPropertyChanged));

        private static void OnLinesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is IEnumerable<bool> newValue)
            {
                ((LedStrip)d).Value = new BitArray(newValue);
            }
        }

        #endregion

        #region Spacing dependency property

        public Thickness Spacing
        {
            get { return (Thickness)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register("Spacing", typeof(Thickness), typeof(LedStrip), new PropertyMetadata(new Thickness(0.5)));

        #endregion

        #region LedSize dependency property

        public double LedSize
        {
            get { return (double)GetValue(LedSizeProperty); }
            set { SetValue(LedSizeProperty, value); }
        }

        public static readonly DependencyProperty LedSizeProperty =
            DependencyProperty.Register("LedSize    ", typeof(double), typeof(LedStrip), new PropertyMetadata(20d));

        #endregion

        #region LitLedColor dependency property

        public Color LitLedColor
        {
            get { return (Color)GetValue(LitLedColorProperty); }
            set { SetValue(LitLedColorProperty, value); }
        }

        public static readonly DependencyProperty LitLedColorProperty =
            DependencyProperty.Register("LitLedColor", typeof(Color), typeof(LedStrip), new PropertyMetadata(Colors.OrangeRed));

        #endregion

        #region Value dependency property

        public BitArray Value
        {
            get { return (BitArray)GetValue(ValueProperty); }
            private set { SetValue(ValuePropertyKey, value); }
        }

        private static DependencyPropertyKey ValuePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Value), typeof(BitArray), typeof(LedStrip), new PropertyMetadata());

        public static DependencyProperty ValueProperty = ValuePropertyKey.DependencyProperty;

        #endregion

        private static void UpdateDock(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LedStrip)d).Dock = ((LedStrip)d).GetDock();
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
