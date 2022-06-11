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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
    }
}
