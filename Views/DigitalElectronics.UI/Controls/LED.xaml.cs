using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;

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

        public bool IsLit
        {
            get { return (bool)GetValue(IsLitProperty); }
            set { SetValue(IsLitProperty, value); }
        }

        public static readonly DependencyProperty IsLitProperty =
            DependencyProperty.Register("IsLit", typeof(bool), typeof(LED), new PropertyMetadata(false));

        #endregion

        #region Stroke depdency property

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(LED), new PropertyMetadata(Brushes.Black));

        #endregion

        #region UnlitColor dependency property

        public Color UnlitColor
        {
            get { return (Color)GetValue(UnlitColorProperty); }
            set { SetValue(UnlitColorProperty, value); }
        }

        public static readonly DependencyProperty UnlitColorProperty =
            DependencyProperty.Register("UnlitColor", typeof(Color), typeof(LED), new PropertyMetadata(Colors.Transparent));

        #endregion

        #region LitColor dependency property

        public Color LitColor
        {
            get { return (Color)GetValue(LitColorProperty); }
            set { SetValue(LitColorProperty, value); }
        }

        public static readonly DependencyProperty LitColorProperty =
            DependencyProperty.Register("LitColor", typeof(Color), typeof(LED), new PropertyMetadata(Colors.OrangeRed));

        #endregion

        #region Text dependency propery

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(LED), new PropertyMetadata(string.Empty));

        #endregion

    }
}
