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
    /// Interaction logic for LED.xaml
    /// </summary>
    public partial class LED : UserControl
    {
        public LED()
        {
            InitializeComponent();
            _layoutRoot.DataContext = this;
        }

        #region IsLit dependency property

        [Category("Appearance")]
        public bool IsLit
        {
            get { return (bool)GetValue(IsLitProperty); }
            set { SetValue(IsLitProperty, value); }
        }

        public static readonly DP IsLitProperty =
            DP.Register(nameof(IsLit),
                typeof(bool),
                typeof(LED),
                new DPMetadata(false,
                    DPMetadataOptions.AffectsRender | DPMetadataOptions.SubPropertiesDoNotAffectRender));

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

    }
}
