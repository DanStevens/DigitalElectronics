using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using DP = System.Windows.DependencyProperty;
using DPMetadata = System.Windows.FrameworkPropertyMetadata;
using DPChangedEventArgs = System.Windows.DependencyPropertyChangedEventArgs;
using DPMetadataOptions = System.Windows.FrameworkPropertyMetadataOptions;

namespace DigitalElectronics.UI.Controls
{
    /// <summary>
    /// Models a 7 segmented LED with a common cathode for displaying a decimal or hexadecimal digit
    /// </summary>
    public partial class SevenSegmentDigit : UserControl
    {
        private static readonly bool[] LinesDefault = Enumerable.Repeat(true, 7).ToArray();
        
        private readonly DispatcherTimer _decayTimer;

        public SevenSegmentDigit()
        {
            InitializeComponent();
            _layoutRoot.DataContext = this;

            _decayTimer = new DispatcherTimer(DispatcherPriority.Send, Dispatcher);
            _decayTimer.IsEnabled = false;
            _decayTimer.Interval = DecayDuration;
            _decayTimer.Tick += DecayTimerOnElapsed;
        }

        #region Lines dependency property

        [Category("Common")]
        public ICollection<bool> Lines
        {
            get { return (ICollection<bool>)GetValue(LinesProperty); }
            set { SetValue(LinesProperty, value); }
        }

        public static readonly DP LinesProperty =
            DP.Register(nameof(Lines),
                typeof(ICollection<bool>),
                typeof(SevenSegmentDigit),
                new DPMetadata(LinesDefault,
                    DPMetadataOptions.AffectsRender | DPMetadataOptions.SubPropertiesDoNotAffectRender));

        #endregion

        #region Stroke dependency property

        [Category("Brush")]
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DP StrokeProperty =
            DP.Register(nameof(Stroke),
                typeof(Brush),
                typeof(SevenSegmentDigit),
                new DPMetadata(default,
                    DPMetadataOptions.AffectsRender | DPMetadataOptions.SubPropertiesDoNotAffectRender));

        #endregion

        #region SegmentsUnlitColor dependency property

        [Category("Brush")]
        public Color SegmentsUnlitColor
        {
            get { return (Color)GetValue(SegmentsUnlitColorProperty); }
            set { SetValue(SegmentsUnlitColorProperty, value); }
        }

        public static readonly DP SegmentsUnlitColorProperty =
            DP.Register(nameof(SegmentsUnlitColor),
                typeof(Color),
                typeof(SevenSegmentDigit),
                new DPMetadata(Colors.Transparent,
                    DPMetadataOptions.AffectsRender | DPMetadataOptions.SubPropertiesDoNotAffectRender));

        #endregion

        #region SegmentsLitColor dependency property

        [Category("Brush")]
        public Color SegmentsLitColor
        {
            get { return (Color)GetValue(SegmentsLitColorProperty); }
            set { SetValue(SegmentsLitColorProperty, value); }
        }

        public static readonly DP SegmentsLitColorProperty =
            DP.Register(nameof(SegmentsLitColor),
                typeof(Color),
                typeof(SevenSegmentDigit),
                new DPMetadata(Colors.OrangeRed,
                    DPMetadataOptions.AffectsRender | DPMetadataOptions.SubPropertiesDoNotAffectRender));

        #endregion

        #region IsActive dependency property

        /// <summary>
        /// A value indicating whether digit is active
        /// </summary>
        /// <remarks>When the digit is active (value is `true`), the lit state of the segments corresponds with the
        /// state of the <see cref="Lines"/> property. When a digit is inactive (value is `false`), all digits are
        /// unlit regardless of the state of the <see cref="Lines"/> property.</remarks>
        [Category("Appearance")]
        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public static readonly DP IsActiveProperty = DP.Register(
            name: nameof(IsActive),
            propertyType: typeof(bool),
            ownerType: typeof(SevenSegmentDigit),
            new DPMetadata(true, DPMetadataOptions.BindsTwoWayByDefault, null, CoerceIsActiveProperty));

        private static object CoerceIsActiveProperty(DependencyObject d, object basevalue)
        {
            return ((SevenSegmentDigit)d).CoercingIsActiveProperty(basevalue);
        }

        private object CoercingIsActiveProperty(object basevalue)
        {
            if (DecayDuration <= TimeSpan.Zero)
                return basevalue;
            
            if (IsActive && (bool)basevalue == false && !_decayTimer.IsEnabled)
            {
                IsDecaying = true;
                _decayTimer.Interval = DecayDuration;
                _decayTimer.Start();
                return true;
            }

            return basevalue;
        }

        private void DecayTimerOnElapsed(object? sender, EventArgs e)
        {
            IsDecaying = false;
            IsActive = false;
            _decayTimer.Stop();
        }

        #endregion

        #region IsDecaying dependency property

        /// <summary>
        /// A value indicating whether the digit is in the 'decaying' state
        /// </summary>
        /// <remarks>When the <see cref="IsActive"/> property is changed from `True` to `False`,
        /// and the value of the <see cref="DecayDuration"/> property is greater than 0,
        /// this property becomes `true`, indicating the digit is in the 'decaying' state.
        /// While in this state, the segments that were lit when the <see cref="IsActive"/>
        /// property is changed remain lit for a short time (defined by the <see cref="DecayDuration"/>
        /// property), after which, all the segments become unlit and the property becomes false.
        /// </remarks>
        [Category("Appearance")]
        public bool IsDecaying
        {
            get { return (bool)GetValue(IsDecayingProperty); }
            private set { SetValue(IsDecayingPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsDecayingPropertyKey = DP.RegisterReadOnly(
            nameof(IsDecaying), 
            typeof(bool), 
            typeof(SevenSegmentDigit),
            new DPMetadata(false, DPMetadataOptions.AffectsRender));

        public static DP IsDecayingProperty = IsDecayingPropertyKey.DependencyProperty;

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
            ownerType: typeof(SevenSegmentDigit),
            new DPMetadata(default(TimeSpan), DPMetadataOptions.AffectsRender));

        #endregion
    }
}
