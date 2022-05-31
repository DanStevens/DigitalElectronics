using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DP = System.Windows.DependencyProperty;
using DPMetadata = System.Windows.FrameworkPropertyMetadata;
using DPMetadataOptions = System.Windows.FrameworkPropertyMetadataOptions;

namespace DigitalElectronics.UI.Controls
{
    /// <summary>
    /// Models a 7 segmented LED with a common cathode for displaying a decimal or hexadecimal digit
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

    }
}
