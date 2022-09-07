using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DigitalElectronics.Concepts;
using DP = System.Windows.DependencyProperty;
using DPMetadata = System.Windows.FrameworkPropertyMetadata;
using DPMetadataOptions = System.Windows.FrameworkPropertyMetadataOptions;

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

        [Category("Layout")]
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DP OrientationProperty =
            DP.Register(nameof(Orientation), typeof(Orientation), typeof(DipSwitch),
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
            DP.Register(nameof(BitOrder), typeof(BitOrder), typeof(DipSwitch),
                new DPMetadata(BitOrder.MsbFirst, UpdateDock));

        #endregion

        #region Dock private read-only dependency property

        private Dock Dock
        {
            get { return (Dock)GetValue(DockProperty); }
            set { SetValue(DockPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey DockPropertyKey =
            DP.RegisterReadOnly(nameof(Dock), typeof(Dock), typeof(DipSwitch), new DPMetadata(Dock.Left, DPMetadataOptions.AffectsArrange));

        public static DP DockProperty = DockPropertyKey.DependencyProperty;

        #endregion

        #region Lines read-only dependency property

        [Category("Common")]
        public ObservableCollection<Bit> Lines
        {
            get { return (ObservableCollection<Bit>)GetValue(LinesProperty); }
            set { SetValue(LinesProperty, value); }
        }

        public static readonly DP LinesProperty =
            DP.Register(nameof(Lines), typeof(ObservableCollection<Bit>), typeof(DipSwitch),
                new DPMetadata(null, OnLinesPropertyChanged));

        private static void OnLinesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is IEnumerable<Bit> newValue)
            {
                var @this = (DipSwitch)d;

                var oldValue = (e.OldValue as IEnumerable<Bit>) ?? Enumerable.Empty<Bit>();
                foreach (var item in oldValue)
                {
                    item.PropertyChanged += OnLineBitChanged;
                }

                foreach (var item in newValue)
                {
                    item.PropertyChanged += OnLineBitChanged;
                }

                @this.SetValue(newValue);

                void OnLineBitChanged(object? sender, PropertyChangedEventArgs _e)
                {
                    if (_e.PropertyName == nameof(Bit.Value))
                        @this.SetValue(newValue);
                }
            }
        }


        #endregion

        #region Value dependency property

        [Category("Common")]
        public BitArray Value
        {
            get { return (BitArray)GetValue(ValueProperty); }
            private set { SetValue(ValuePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey ValuePropertyKey =
            DP.RegisterReadOnly(nameof(Value), typeof(BitArray), typeof(DipSwitch), new DPMetadata());

        public static DP ValueProperty = ValuePropertyKey.DependencyProperty;

        private void SetValue(IEnumerable<Bit> newValue)
        {
            Value = new BitArray(newValue.Select(bit => bit.Value));
        }

        #endregion

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

        private void ToolTip_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
