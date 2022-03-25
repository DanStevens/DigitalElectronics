using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DigitalElectronics.Utilities;

namespace DigitalElectronics.UI.Controls
{
    /// <summary>
    /// Interaction logic for LedStrip.xaml
    /// </summary>
    public partial class LedStrip : UserControl
    {
        public LedStrip()
        {
            InitializeComponent();
            _layoutRoot.DataContext = this;
        }

        #region Orientation dependency property

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(LedStrip), new PropertyMetadata(default));

        #endregion

        #region LEDs dependency property

        public ICollection<bool> Lines
        {
            get { return (ICollection<bool>)GetValue(LinesProperty); }
            set { SetValue(LinesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinesProperty =
            DependencyProperty.Register("Lines", typeof(ICollection<bool>), typeof(LedStrip), new PropertyMetadata(null));

        #endregion

        #region Spacing dependency property

        public Thickness Spacing
        {
            get { return (Thickness)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Spacing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register("Spacing", typeof(Thickness), typeof(LedStrip), new PropertyMetadata(new Thickness(0.5)));

        #endregion

        #region LedSize dependency property

        public double LedSize
        {
            get { return (double)GetValue(LedSizeProperty); }
            set { SetValue(LedSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LedSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LedSizeProperty =
            DependencyProperty.Register("LedSize", typeof(double), typeof(LedStrip), new PropertyMetadata(20d));



        public Color LedColorWhenLit
        {
            get { return (Color)GetValue(LedColorWhenLitProperty); }
            set { SetValue(LedColorWhenLitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LedColorWhenLit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LedColorWhenLitProperty =
            DependencyProperty.Register("LedColorWhenLit", typeof(Color), typeof(LedStrip), new PropertyMetadata(Colors.OrangeRed));



        #endregion
    }
}
