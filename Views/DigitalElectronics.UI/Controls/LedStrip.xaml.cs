using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DigitalElectronics.Concepts;
using DP = System.Windows.DependencyProperty;
using DPMetadata = System.Windows.FrameworkPropertyMetadata;
using DPMetadataOptions = System.Windows.FrameworkPropertyMetadataOptions;

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

        [Category("Layout")]
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DP OrientationProperty =
            DP.Register(nameof(Orientation), typeof(Orientation), typeof(LedStrip),
                new DPMetadata(
                    Orientation.Horizontal,
                    DPMetadataOptions.AffectsArrange,
                    UpdateDock));

        #endregion

        #region BitOrder dependency property

        [Category("Appearance")]
        public BitOrder BitOrder
        {
            get { return (BitOrder)GetValue(BitOrderProperty); }
            set { SetValue(BitOrderProperty, value); }
        }

        public static readonly DP BitOrderProperty =
            DP.Register(nameof(BitOrder), typeof(BitOrder), typeof(LedStrip),
                new DPMetadata(BitOrder.MsbFirst, UpdateDock));

        #endregion

        #region Dock private read-only dependency property

        private Dock Dock
        {
            get { return (Dock)GetValue(DockProperty); }
            set { SetValue(DockPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey DockPropertyKey =
            DP.RegisterReadOnly(nameof(Dock), typeof(Dock), typeof(LedStrip), new DPMetadata(Dock.Left));

        public static DP DockProperty = DockPropertyKey.DependencyProperty;

        #endregion

        #region Lines dependency property

        [Category("Common")]
        public ICollection<bool> Lines
        {
            get { return (ICollection<bool>)GetValue(LinesProperty); }
            set { SetValue(LinesProperty, value); }
        }

        public static readonly DP LinesProperty =
            DP.Register(nameof(Lines), typeof(ICollection<bool>), typeof(LedStrip),
                new DPMetadata(null, OnLinesPropertyChanged));

        private static void OnLinesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is IEnumerable<bool> newValue)
            {
                ((LedStrip)d).Value = new BitArray(newValue);
            }
        }

        #endregion

        #region Spacing dependency property

        [Category("Layout")]
        public Thickness Spacing
        {
            get { return (Thickness)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public static readonly DP SpacingProperty =
            DP.Register(nameof(Spacing), typeof(Thickness), typeof(LedStrip), new DPMetadata(new Thickness(0.5)));

        #endregion

        #region LedSize dependency property

        [Category("Appearance")]
        public double LedSize
        {
            get { return (double)GetValue(LedSizeProperty); }
            set { SetValue(LedSizeProperty, value); }
        }

        public static readonly DP LedSizeProperty =
            DP.Register(nameof(LedSize), typeof(double), typeof(LedStrip), new DPMetadata(20d));

        #endregion

        #region LitLedColor dependency property

        [Category("Brush")]
        public Color LitLedColor    
        {
            get { return (Color)GetValue(LitLedColorProperty); }
            set { SetValue(LitLedColorProperty, value); }
        }

        public static readonly DP LitLedColorProperty =
            DP.Register(nameof(LitLedColor), typeof(Color), typeof(LedStrip), new DPMetadata(Colors.OrangeRed, DPMetadataOptions.AffectsRender));

        #endregion

        #region Value dependency property

        [Category("Common")]
        public BitArray Value
        {
            get { return (BitArray)GetValue(ValueProperty); }
            private set { SetValue(ValuePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey ValuePropertyKey =
            DP.RegisterReadOnly(nameof(Value), typeof(BitArray), typeof(LedStrip), new DPMetadata());

        public static DP ValueProperty = ValuePropertyKey.DependencyProperty;

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
