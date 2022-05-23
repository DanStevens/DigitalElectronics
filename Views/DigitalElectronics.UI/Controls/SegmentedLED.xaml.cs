using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;

namespace DigitalElectronics.UI.Controls
{
    /// <summary>
    /// Interaction logic for LED.xaml
    /// </summary>
    public partial class SegmentedLED : UserControl
    {
        public SegmentedLED()
        {
            InitializeComponent();
            _layoutRoot.DataContext = this;
        }

        #region SegmentAIsLit dependency property

        public bool SegmentAIsLit
        {
            get => (bool)GetValue(SegmentAIsLitProperty);
            set => SetValue(SegmentAIsLitProperty, value);
        }

        public static readonly DependencyProperty SegmentAIsLitProperty = DependencyProperty.Register(
            name: nameof(SegmentAIsLit),
            propertyType: typeof(bool),
            ownerType: typeof(SegmentedLED),
            new PropertyMetadata(false));

        #endregion

        #region SegmentBIsLit dependency property

        public bool SegmentBIsLit
        {
            get => (bool)GetValue(SegmentBIsLitProperty);
            set => SetValue(SegmentBIsLitProperty, value);
        }

        public static readonly DependencyProperty SegmentBIsLitProperty = DependencyProperty.Register(
            name: nameof(SegmentBIsLit),
            propertyType: typeof(bool),
            ownerType: typeof(SegmentedLED),
            new PropertyMetadata(false));

        #endregion

        #region SegmentCIsLit dependency property

        public bool SegmentCIsLit
        {
            get => (bool)GetValue(SegmentCIsLitProperty);
            set => SetValue(SegmentCIsLitProperty, value);
        }

        public static readonly DependencyProperty SegmentCIsLitProperty = DependencyProperty.Register(
            name: nameof(SegmentCIsLit),
            propertyType: typeof(bool),
            ownerType: typeof(SegmentedLED),
            new PropertyMetadata(false));

        #endregion

        #region SegmentDIsLit dependency property

        public bool SegmentDIsLit
        {
            get => (bool)GetValue(SegmentDIsLitProperty);
            set => SetValue(SegmentDIsLitProperty, value);
        }

        public static readonly DependencyProperty SegmentDIsLitProperty = DependencyProperty.Register(
            name: nameof(SegmentDIsLit),
            propertyType: typeof(bool),
            ownerType: typeof(SegmentedLED),
            new PropertyMetadata(false));

        #endregion

        #region SegmentEIsLit dependency property

        public bool SegmentEIsLit
        {
            get => (bool)GetValue(SegmentEIsLitProperty);
            set => SetValue(SegmentEIsLitProperty, value);
        }

        public static readonly DependencyProperty SegmentEIsLitProperty = DependencyProperty.Register(
            name: nameof(SegmentEIsLit),
            propertyType: typeof(bool),
            ownerType: typeof(SegmentedLED),
            new PropertyMetadata(false));

        #endregion

        #region SegmentFIsLit dependency property

        public bool SegmentFIsLit
        {
            get => (bool)GetValue(SegmentFIsLitProperty);
            set => SetValue(SegmentFIsLitProperty, value);
        }

        public static readonly DependencyProperty SegmentFIsLitProperty = DependencyProperty.Register(
            name: nameof(SegmentFIsLit),
            propertyType: typeof(bool),
            ownerType: typeof(SegmentedLED),
            new PropertyMetadata(false));

        #endregion

        #region SegmentGIsLit dependency property

        public bool SegmentGIsLit
        {
            get => (bool)GetValue(SegmentGIsLitProperty);
            set => SetValue(SegmentGIsLitProperty, value);
        }

        public static readonly DependencyProperty SegmentGIsLitProperty = DependencyProperty.Register(
            name: nameof(SegmentGIsLit),
            propertyType: typeof(bool),
            ownerType: typeof(SegmentedLED),
            new PropertyMetadata(false));

        #endregion

        #region Stroke dependency property

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(SegmentedLED), new PropertyMetadata(default));

        #endregion

        #region SegmentsUnlitColor dependency property

        public Color SegmentsUnlitColor
        {
            get { return (Color)GetValue(SegmentsUnlitColorProperty); }
            set { SetValue(SegmentsUnlitColorProperty, value); }
        }

        public static readonly DependencyProperty SegmentsUnlitColorProperty =
            DependencyProperty.Register("SegmentsUnlitColor", typeof(Color), typeof(SegmentedLED), new PropertyMetadata(Colors.Transparent));

        #endregion

        #region SegmentsLitColor dependency property

        public Color SegmentsLitColor
        {
            get { return (Color)GetValue(SegmentsLitColorProperty); }
            set { SetValue(SegmentsLitColorProperty, value); }
        }

        public static readonly DependencyProperty SegmentsLitColorProperty =
            DependencyProperty.Register("SegmentsLitColor", typeof(Color), typeof(SegmentedLED), new PropertyMetadata(Colors.OrangeRed));

        #endregion

    }
}
