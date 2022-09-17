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
using System.Windows.Threading;

namespace DigitalElectronics.BenEater.Computers.Views
{
    /// <summary>
    /// Interaction logic for BE801ComputerView.xaml
    /// </summary>
    public sealed partial class BE801ComputerView : UserControl, IDisposable
    {
        private DispatcherTimer _clock = new();

        public BE801ComputerView()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            _clock.Interval = TimeSpan.FromSeconds(_viewModel.ClockModule.ClockSpeed);
            _clock.Tick += OnClockTick;
            _viewModel.ClockModule.IsRunningChanged += OnComputerRunningStateChanged;
            _viewModel.ClockModule.ClockSpeedChanged += OnClockSpeedChanged;
        }

        private void OnClockSpeedChanged(object? sender, EventArgs e)
        {
            _clock.Interval = TimeSpan.FromSeconds(_viewModel.ClockModule.ClockSpeed);
        }

        private void OnComputerRunningStateChanged(object? sender, EventArgs e)
        {
            if (_viewModel.ClockModule.IsRunning)
                _clock.Start();
            else
                _clock.Stop();
        }

        private void OnClockTick(object? sender, EventArgs e)
        {
            _viewModel.Clock();
        }

        private void ClockButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Clock();
        }

        public void Dispose()
        {
            _clock.Tick -= OnClockTick;
            _viewModel.ClockModule.IsRunningChanged -= OnComputerRunningStateChanged;
        }
    }
}
