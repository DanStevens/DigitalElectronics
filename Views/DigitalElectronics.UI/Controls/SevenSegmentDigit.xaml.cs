using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DigitalElectronics.UI.Controls
{
    /// <summary>
    /// Interaction logic for LED.xaml
    /// </summary>
    public partial class SevenSegmentDigit : UserControl
    {
        private static readonly bool[] LinesDefault = Enumerable.Repeat(true, 7).ToArray();

        public SevenSegmentDigit()
        {
            InitializeComponent();
            _layoutRoot.DataContext = this;
        }

        #region Lines dependency property

        public ICollection<bool> Lines
        {
            get { return (ICollection<bool>)GetValue(LinesProperty); }
            set { SetValue(LinesProperty, value); }
        }

        public static readonly DependencyProperty LinesProperty =
            DependencyProperty.Register("Lines", typeof(ICollection<bool>), typeof(SevenSegmentDigit),
                new PropertyMetadata(                    LinesDefault));

        #endregion

        #region Stroke dependency property

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(SevenSegmentDigit), new PropertyMetadata(default));

        #endregion

        #region SegmentsUnlitColor dependency property

        public Color SegmentsUnlitColor
        {
            get { return (Color)GetValue(SegmentsUnlitColorProperty); }
            set { SetValue(SegmentsUnlitColorProperty, value); }
        }

        public static readonly DependencyProperty SegmentsUnlitColorProperty =
            DependencyProperty.Register("SegmentsUnlitColor", typeof(Color), typeof(SevenSegmentDigit), new PropertyMetadata(Colors.Transparent));

        #endregion

        #region SegmentsLitColor dependency property

        public Color SegmentsLitColor
        {
            get { return (Color)GetValue(SegmentsLitColorProperty); }
            set { SetValue(SegmentsLitColorProperty, value); }
        }

        public static readonly DependencyProperty SegmentsLitColorProperty =
            DependencyProperty.Register("SegmentsLitColor", typeof(Color), typeof(SevenSegmentDigit), new PropertyMetadata(Colors.OrangeRed));

        #endregion

    }
}
