using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DigitalElectronics.BenEater.Computers.ViewModels
{
    public class BE801ComputerViewModel
    {
        public ClockModuleViewModel ClockModule { get; } = new ClockModuleViewModel();

        public void Clock()
        {
            // TODO
        }

        public class ClockModuleViewModel : INotifyPropertyChanged
        {
            private bool isRunning;

            public bool IsRunning
            {
                get => isRunning;
                set
                {
                    if (isRunning != value)
                    {
                        isRunning = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanStep)));
                        IsRunningChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }

            public bool CanStep => !IsRunning;

            private double clockSpeed = 1;

            /// <summary>
            /// Clock speed in ticks per second (Hertz)
            /// </summary>
            public double ClockSpeed
            {
                get => clockSpeed;
                set
                {
                    if (clockSpeed != value)
                    {
                        clockSpeed = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ClockSpeed)));
                        ClockSpeedChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            public event EventHandler? IsRunningChanged;

            public event EventHandler? ClockSpeedChanged;
        }
    }
}
