using System.Collections.Generic;
using System.ComponentModel;
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

        #region DigitPadding dependency property

        /// <summary>
        /// Padding for all digits in the display
        /// </summary>
        [Category("Layout")]
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
                typeof(FourDigit7SegmentDisplay),
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
                typeof(FourDigit7SegmentDisplay),
                new DPMetadata(Colors.OrangeRed,
                    DPMetadataOptions.AffectsRender | DPMetadataOptions.SubPropertiesDoNotAffectRender));

        #endregion

        #region LinesForDigit0 dependency property

        /// <summary>
        /// Input lines for the rightmost 7 segment digit
        /// </summary>
        [Category("Common")]
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
        [Category("Common")]
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
        [Category("Common")]
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
        [Category("Common")]
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
    }
}
