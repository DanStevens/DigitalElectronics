using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using DP = System.Windows.DependencyProperty;
using DPChangedEventArgs = System.Windows.DependencyPropertyChangedEventArgs;
using DPKey = System.Windows.DependencyPropertyKey;
using DPMetadata = System.Windows.FrameworkPropertyMetadata;
using DPMetadataOptions = System.Windows.FrameworkPropertyMetadataOptions;

namespace DigitalElectronics.UI.Controls
{
    /// <summary>
    /// Interaction logic for LED.xaml
    /// </summary>
    public partial class LED : UserControl
    {
        DispatcherTimer _decayTimer;

        public LED()
        {
            InitializeComponent();
            _layoutRoot.DataContext = this;
            _decayTimer = new DispatcherTimer(DispatcherPriority.Send, Dispatcher);
            _decayTimer.IsEnabled = false;
            _decayTimer.Interval = DecayDuration;
            _decayTimer.Tick += DecayTimerOnElapsed;
        }

        #region IsLit dependency property

        /// <summary>
        /// A value indicating the 'lit' state of the LED
        /// </summary>
        /// <remarks>When `True`, the LED is in the 'lit' state and is colored according to the
        /// <see cref="LitColor"/> property. When `False`, the LED is in the 'unlit` state and is
        /// colored according to <see cref="UnlitColor"/> property.</remarks>
        [Category("Appearance")]
        public bool IsLit
        {
            get => (bool)GetValue(IsLitProperty);
            set => SetValue(IsLitProperty, value);
        }

        public static readonly DP IsLitProperty = DP.Register(
            name: nameof(IsLit),
            propertyType: typeof(bool),
            ownerType: typeof(LED),
            new DPMetadata(default(bool),
                DPMetadataOptions.AffectsRender | DPMetadataOptions.SubPropertiesDoNotAffectRender,
                IsLitPropertyChanged));

        private static void IsLitPropertyChanged(DependencyObject d, DPChangedEventArgs e)
        {
            ((LED)d).IsLitPropertyChanged(e);
        }

        private void IsLitPropertyChanged(DPChangedEventArgs e)
        {
            if (DecayDuration > TimeSpan.Zero && IsSwitchingOff())
            {
                _decayTimer.Stop();
                IsDecaying = true;
                _decayTimer.Interval = DecayDuration;
                _decayTimer.Start();
            }

            bool IsSwitchingOff() => (bool)e.OldValue && !(bool)e.NewValue;
        }

        private void DecayTimerOnElapsed(object? sender, EventArgs e)
        {
            _decayTimer.Stop();
            IsDecaying = false;
        }

        #endregion

        #region Stroke depdency property

        [Category(nameof(Brush))]
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DP StrokeProperty =
            DP.Register(nameof(Stroke),
                typeof(Brush),
                typeof(LED),
                new DPMetadata(Brushes.Black,
                    DPMetadataOptions.AffectsRender | DPMetadataOptions.SubPropertiesDoNotAffectRender));

        #endregion

        #region UnlitColor dependency property

        [Category("Brush")]
        public Color UnlitColor
        {
            get { return (Color)GetValue(UnlitColorProperty); }
            set { SetValue(UnlitColorProperty, value); }
        }

        public static readonly DP UnlitColorProperty =
            DP.Register(nameof(UnlitColor),
                typeof(Color),
                typeof(LED),
                new DPMetadata(Colors.Transparent,
                    DPMetadataOptions.AffectsRender | DPMetadataOptions.SubPropertiesDoNotAffectRender));

        #endregion

        #region LitColor dependency property

        [Category("Brush")]
        public Color LitColor
        {
            get { return (Color)GetValue(LitColorProperty); }
            set { SetValue(LitColorProperty, value); }
        }

        public static readonly DP LitColorProperty =
            DP.Register(nameof(LitColor),
                typeof(Color),
                typeof(LED),
                new DPMetadata(Colors.OrangeRed,
                    DPMetadataOptions.AffectsRender | DPMetadataOptions.SubPropertiesDoNotAffectRender));

        #endregion

        #region Text dependency propery

        [Category("Appearance")]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DP TextProperty =
            DP.Register(nameof(Text), typeof(string), typeof(LED), new PropertyMetadata(string.Empty));

        #endregion

        #region DecayDuration dependency property

        /// <summary>
        /// The duration of the 'decaying' state
        /// </summary>
        [Category("Appearance")]
        public TimeSpan DecayDuration
        {
            get => (TimeSpan)GetValue(DecayDurationProperty);
            set => SetValue(DecayDurationProperty, value);
        }

        public static readonly DP DecayDurationProperty = DP.Register(
            name: nameof(DecayDuration),
            propertyType: typeof(TimeSpan),
            ownerType: typeof(LED),
            new DPMetadata(TimeSpan.Zero, DPMetadataOptions.None));

        #endregion

        #region IsDecaying read-only dependency property

        /// <summary>
        /// A value indicating whether the digit is in the 'decaying' state
        /// </summary>
        /// <remarks>When the <see cref="IsLit"/> property is changed from `True` to `False`,
        /// and the value of the <see cref="DecayDuration"/> property is greater than 0,
        /// this property becomes `true`, indicating the LED is in the 'decaying' state.
        /// While in this state, the LED appears lit for a short time (defined by the <see cref="DecayDuration"/>
        /// property), after which, all the segments become unlit and the property becomes false.
        /// </remarks>
        public bool IsDecaying
        {
            get => (bool)GetValue(IsDecayingPropertyKey.DependencyProperty);
            private set => SetValue(IsDecayingPropertyKey, value);
        }

        public static readonly DPKey IsDecayingPropertyKey = DP.RegisterReadOnly(
            name: nameof(IsDecaying),
            propertyType: typeof(bool),
            ownerType: typeof(LED),
            new DPMetadata(default(bool), DPMetadataOptions.None));

        #endregion
    }
}
