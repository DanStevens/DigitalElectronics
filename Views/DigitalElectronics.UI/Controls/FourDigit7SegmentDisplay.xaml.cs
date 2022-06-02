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
    /// Interaction logic for FourDigit7SegmentDisplay.xaml
    /// </summary>
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


        #region Value dependency property

        public ICollection<bool> Value
        {
            get => (ICollection<bool>)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DP ValueProperty = DP.Register(
            name: nameof(Value),
            propertyType: typeof(ICollection<bool>),
            ownerType: typeof(FourDigit7SegmentDisplay),
            new DPMetadata(default(ICollection<bool>), ValuePropertyChanged));

        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // TODO optional
        }

        #endregion
    }
}
