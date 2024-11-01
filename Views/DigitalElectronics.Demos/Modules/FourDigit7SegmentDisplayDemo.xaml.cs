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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DigitalElectronics.Demos.Modules
{
    /// <summary>
    /// Interaction logic for FourDigit7SegmentDisplayDemo.xaml
    /// </summary>
    public partial class FourDigit7SegmentDisplayDemo : Window
    {
        private DispatcherTimer _displayTimer = new();

        public FourDigit7SegmentDisplayDemo()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            SetDisplayTimerInterval(ViewModel.UpdateInterval);
            ViewModel.UpdateIntervalChanged += OnUpdateIntervalChanged;
            _displayTimer.Tick += DisplayTimerOnTick;
            _displayTimer.Start();
        }

        private void OnUpdateIntervalChanged(object? sender, EventArgs e)
        {
            SetDisplayTimerInterval(ViewModel.UpdateInterval);
        }

        private void SetDisplayTimerInterval(double intervalMilliseconds)
        {
            _displayTimer.Interval = TimeSpan.FromMilliseconds(intervalMilliseconds);
        }

        private void DisplayTimerOnTick(object? sender, EventArgs e)
        {
            ViewModel.Clock();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _displayTimer.Stop();
            _displayTimer.Tick -= DisplayTimerOnTick;
            ViewModel.UpdateIntervalChanged -= OnUpdateIntervalChanged;
        }
    }
}
