using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DP = System.Windows.DependencyProperty;
using DPMetadata = System.Windows.FrameworkPropertyMetadata;
using DPMetadataOptions = System.Windows.FrameworkPropertyMetadataOptions;

namespace DigitalElectronics.UI.Controls
{
    /// <summary>
    /// A 4 digit 7 segment display.
    /// </summary>
    /// <note>Digits are indexed from 0, starting with the rightmost digit.</note>
    public partial class FourDigit7SegmentDisplay : UserControl
    {
        public FourDigit7SegmentDisplay()
        {
            InitializeComponent();
            _layoutRoot.DataContext = this;
        }


        #region SegmentsLitColor dependency property

        public Color SegmentsLitColor
        {
            get => (Color)GetValue(SegmentsLitColorProperty);
            set => SetValue(SegmentsLitColorProperty, value);
        }

        public static readonly DP SegmentsLitColorProperty = DP.Register(
            name: nameof(SegmentsLitColor),
            propertyType: typeof(Color),
            ownerType: typeof(FourDigit7SegmentDisplay),
            new DPMetadata(LED.DefaultLitColor, DPMetadataOptions.None));

        #endregion


        #region SegmentsUnlitColor dependency property

        public Color SegmentsUnlitColor
        {
            get => (Color)GetValue(SegmentsUnlitColorProperty);
            set => SetValue(SegmentsUnlitColorProperty, value);
        }

        public static readonly DP SegmentsUnlitColorProperty = DP.Register(
            name: nameof(SegmentsUnlitColor),
            propertyType: typeof(Color),
            ownerType: typeof(FourDigit7SegmentDisplay),
            new DPMetadata(LED.DefaultUnlitColor, DPMetadataOptions.None));

        #endregion

        #region DigitPadding dependency property

        /// <summary>
        /// Padding for all digits in the display
        /// </summary>
        public Thickness DigitPadding
        {
            get => (Thickness)GetValue(DigitPaddingProperty);
            set => SetValue(DigitPaddingProperty, value);
        }

        public static readonly DP DigitPaddingProperty = DP.Register(
            name: nameof(DigitPadding),
            propertyType: typeof(Thickness),
            ownerType: typeof(FourDigit7SegmentDisplay),
            new DPMetadata(new Thickness(5)));

        #endregion

        #region LinesForDigit0 dependency property

        /// <summary>
        /// Input lines for the rightmost 7 segment digit
        /// </summary>
        public ICollection<bool> LinesForDigit0
        {
            get => (ICollection<bool>)GetValue(LinesForDigit0Property);
            set => SetValue(LinesForDigit0Property, value);
        }

        public static readonly DP LinesForDigit0Property = DP.Register(
            name: nameof(LinesForDigit0),
            propertyType: typeof(ICollection<bool>),
            ownerType: typeof(FourDigit7SegmentDisplay),
            new DPMetadata(default(ICollection<bool>)));

        #endregion

        #region LinesForDigit1 dependency property

        /// <summary>
        /// Input lines for the second digit from the right
        /// </summary>
        public ICollection<bool> LinesForDigit1
        {
            get => (ICollection<bool>)GetValue(LinesForDigit1Property);
            set => SetValue(LinesForDigit1Property, value);
        }

        public static readonly DP LinesForDigit1Property = DP.Register(
            name: nameof(LinesForDigit1),
            propertyType: typeof(ICollection<bool>),
            ownerType: typeof(FourDigit7SegmentDisplay),
            new DPMetadata(default(ICollection<bool>)));

        #endregion

        #region LinesForDigit2 dependency property

        /// <summary>
        /// Input lines for the third digit from the right
        /// </summary>
        public ICollection<bool> LinesForDigit2
        {
            get => (ICollection<bool>)GetValue(LinesForDigit2Property);
            set => SetValue(LinesForDigit2Property, value);
        }

        public static readonly DP LinesForDigit2Property = DP.Register(
            name: nameof(LinesForDigit2),
            propertyType: typeof(ICollection<bool>),
            ownerType: typeof(FourDigit7SegmentDisplay),
            new DPMetadata(default(ICollection<bool>)));

        #endregion

        #region LinesForDigit3 dependency property

        /// <summary>
        /// Input lines for the fourth digit from the right
        /// </summary>
        public ICollection<bool> LinesForDigit3
        {
            get => (ICollection<bool>)GetValue(LinesForDigit3Property);
            set => SetValue(LinesForDigit3Property, value);
        }

        public static readonly DP LinesForDigit3Property = DP.Register(
            name: nameof(LinesForDigit3),
            propertyType: typeof(ICollection<bool>),
            ownerType: typeof(FourDigit7SegmentDisplay),
            new DPMetadata(default(ICollection<bool>)));

        #endregion

        #region Digit0IsActive dependency property

        /// <summary>
        /// <see cref="SevenSegmentDigit.IsActive"/> property for the rightmost
        /// 7 segment digit.
        /// </summary>
        public bool Digit0IsActive
        {
            get => (bool)GetValue(Digit0IsActiveProperty);
            set => SetValue(Digit0IsActiveProperty, value);
        }

        public static readonly DP Digit0IsActiveProperty = DP.Register(
            name: nameof(Digit0IsActive),
            propertyType: typeof(bool),
            ownerType: typeof(FourDigit7SegmentDisplay),
            new DPMetadata(true));

        #endregion

        #region Digit1IsActive dependency property

        /// <summary>
        /// <see cref="SevenSegmentDigit.IsActive"/> property for second digit from the right
        /// </summary>
        public bool Digit1IsActive
        {
            get => (bool)GetValue(Digit1IsActiveProperty);
            set => SetValue(Digit1IsActiveProperty, value);
        }

        public static readonly DP Digit1IsActiveProperty = DP.Register(
            name: nameof(Digit1IsActive),
            propertyType: typeof(bool),
            ownerType: typeof(FourDigit7SegmentDisplay),
            new DPMetadata(true));

        #endregion

        #region Digit2IsActive dependency property

        /// <summary>
        /// <see cref="SevenSegmentDigit.IsActive"/> property for third digit from the right
        /// </summary>
        public bool Digit2IsActive
        {
            get => (bool)GetValue(Digit2IsActiveProperty);
            set => SetValue(Digit2IsActiveProperty, value);
        }

        public static readonly DP Digit2IsActiveProperty = DP.Register(
            name: nameof(Digit2IsActive),
            propertyType: typeof(bool),
            ownerType: typeof(FourDigit7SegmentDisplay),
            new DPMetadata(true));

        #endregion

        #region Digit3IsActive dependency property

        /// <summary>
        /// <see cref="SevenSegmentDigit.IsActive"/> property for the fourth digit from the right
        /// </summary>
        public bool Digit3IsActive
        {
            get => (bool)GetValue(Digit3IsActiveProperty);
            set => SetValue(Digit3IsActiveProperty, value);
        }

        public static readonly DP Digit3IsActiveProperty = DP.Register(
            name: nameof(Digit3IsActive),
            propertyType: typeof(bool),
            ownerType: typeof(FourDigit7SegmentDisplay),
            new DPMetadata(true));

        #endregion
    }
}
