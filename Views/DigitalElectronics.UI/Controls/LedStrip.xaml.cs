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

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(LedStrip), new PropertyMetadata(default));

        #endregion

        #region LEDs dependency property

        public ICollection<bool> Lines
        {
            get { return (ICollection<bool>)GetValue(LinesProperty); }
            set { SetValue(LinesProperty, value); }
        }

        public static readonly DependencyProperty LinesProperty =
            DependencyProperty.Register("Lines", typeof(ICollection<bool>), typeof(LedStrip), new PropertyMetadata(null));

        #endregion

        #region Spacing dependency property

        public Thickness Spacing
        {
            get { return (Thickness)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register("Spacing", typeof(Thickness), typeof(LedStrip), new PropertyMetadata(new Thickness(0.5)));

        #endregion

        #region LedSize dependency property

        public double LedSize
        {
            get { return (double)GetValue(LedSizeProperty); }
            set { SetValue(LedSizeProperty, value); }
        }

        public static readonly DependencyProperty LedSizeProperty =
            DependencyProperty.Register("LedSize", typeof(double), typeof(LedStrip), new PropertyMetadata(20d));

        #endregion

        #region LedColorWhenLit dependency property

        public Color LedColorWhenLit
        {
            get { return (Color)GetValue(LedColorWhenLitProperty); }
            set { SetValue(LedColorWhenLitProperty, value); }
        }

        public static readonly DependencyProperty LedColorWhenLitProperty =
            DependencyProperty.Register("LedColorWhenLit", typeof(Color), typeof(LedStrip), new PropertyMetadata(Colors.OrangeRed));

        #endregion

        #region MarkLsb dependency property

        /// <summary>
        /// Marks the LED for the least-significant bit with a spot.
        /// </summary>
        public bool MarkLsb
        {
            get { return (bool)GetValue(MarkLsbProperty); }
            set { SetValue(MarkLsbProperty, value); }
        }

        public static readonly DependencyProperty MarkLsbProperty =
            DependencyProperty.Register("MarkLsb", typeof(bool), typeof(LedStrip), new PropertyMetadata(true));

        #endregion
    }
}
