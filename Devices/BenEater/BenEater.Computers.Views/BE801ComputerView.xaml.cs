using System;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;

namespace DigitalElectronics.BenEater.Computers.Views
{
    /// <summary>
    /// Interaction logic for BE801ComputerView.xaml
    /// </summary>
    public sealed partial class BE801ComputerView : UserControl, IDisposable
    {
        private readonly Timer _clock = new();

        public BE801ComputerView()
        {
            InitializeComponent();
        }

        public static readonly RoutedEvent ClockTickEvent = EventManager.RegisterRoutedEvent(
            name: "ClockTick",
            routingStrategy: RoutingStrategy.Direct,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(BE801ComputerView));

        public event RoutedEventHandler ClockTick
        {
            add { AddHandler(ClockTickEvent, value); }
            remove { RemoveHandler(ClockTickEvent, value); }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            _clock.Interval = _viewModel.ClockModule.ClockCycleDuration.TotalMilliseconds;
            _clock.Elapsed += OnClockTick;
            _viewModel.ClockModule.IsRunningChanged += OnComputerRunningStateChanged;
            _viewModel.ClockModule.ClockSpeedChanged += OnClockSpeedChanged;
        }

        private void OnClockSpeedChanged(object? sender, EventArgs e)
        {
            _clock.Interval = _viewModel.ClockModule.ClockCycleDuration.TotalMilliseconds;
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
            Clock();
        }

        private void ClockButton_Click(object sender, RoutedEventArgs e)
        {
            Clock();
        }

        private void Clock()
        {
            Dispatcher.InvokeAsync(() => RaiseEvent(new RoutedEventArgs(ClockTickEvent)));
            _viewModel.Clock();
        }

        public void Dispose()
        {
            _clock.Elapsed -= OnClockTick;
            _viewModel.ClockModule.IsRunningChanged -= OnComputerRunningStateChanged;
        }

        private void LoadRAMButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*",
                RestoreDirectory = true,
            };

            if (dialog.ShowDialog() == true)
            {
                byte[] data = File.ReadAllBytes(dialog.FileName);
                _viewModel.LoadRAM(data);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ClockModule.ApplyNewClockSpeed();
        }
    }
}
