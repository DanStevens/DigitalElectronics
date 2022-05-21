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

        #region IsLit dependency property

        public bool SegmentAIsLit
        {
            get { return (bool)GetValue(SegmentAIsLitProperty); }
            set { SetValue(SegmentAIsLitProperty, value); }
        }

        public static readonly DependencyProperty SegmentAIsLitProperty =
            DependencyProperty.Register("SegmentAIsLit", typeof(bool), typeof(SegmentedLED), new PropertyMetadata(false));

        #endregion

        #region Stroke depdency property

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(SegmentedLED), new PropertyMetadata(Brushes.Black));

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
