using System;
using System.Collections.ObjectModel;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.BenEater.Computers.ViewModels
{
    public class BE801ComputerViewModel
    {
        private readonly BE801Computer _computer;
        
        public ClockModuleViewModel ClockModule { get; }
        public OutputModuleViewModel OutputModule { get; }

        public BE801ComputerViewModel()
        {
            _computer = new();
            ClockModule = new();
            OutputModule = new(_computer);
        }

        public void Clock()
        {
            _computer.Clock();
            ClockModule.Clock();
            OutputModule.Clock();
        }

        /// <summary>
        /// Loads the given data into RAM
        /// </summary>
        /// <param name="data">Bytes to load</param>
        /// <remarks>Only the first 16 bytes of of <paramref name="data"/> are loaded into RAM</remarks>
        public void LoadRAM(byte[] data)
        {
            _computer.LoadRAM(data);
        }

        public class ClockModuleViewModel : ModuleViewModel
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
                        RaisePropertyChanged(nameof(IsRunning));
                        RaisePropertyChanged(nameof(CanStep));
                        IsRunningChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }

            public bool CanStep => !IsRunning;

            private double currentClockSpeed = 1;

            /// <summary>
            /// Clock speed in ticks per second (Hertz)
            /// </summary>
            public double CurrentClockSpeed
            {
                get => currentClockSpeed;
                set
                {
                    if (currentClockSpeed != value)
                    {
                        currentClockSpeed = value;
                        RaisePropertyChanged(nameof(CurrentClockSpeed));
                        RaisePropertyChanged(nameof(ClockCycleDuration));
                        ClockSpeedChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }


            private double newClockSpeed = 1;

            public double NewClockSpeed
            {
                get => newClockSpeed;
                set
                {
                    if (newClockSpeed != value)
                    {
                        newClockSpeed = value;
                        RaisePropertyChanged(nameof(NewClockSpeed));
                    }
                }
            }

            public void ApplyNewClockSpeed()
            {
                CurrentClockSpeed = NewClockSpeed;
            }

            /// <summary>
            /// The time duration between clock cycles
            /// </summary>
            public TimeSpan ClockCycleDuration => TimeSpan.FromSeconds(1.0 / CurrentClockSpeed);

            public event EventHandler? IsRunningChanged;

            public event EventHandler? ClockSpeedChanged;
        }

        public class OutputModuleViewModel : ModuleViewModel
        {
            private readonly BE801Computer _computer;

            public OutputModuleViewModel(BE801Computer computer)
            {
                _computer = computer ?? throw new ArgumentNullException(nameof(computer));
            }

            public BitArray OutRegister => _computer.ProbeOutRegister();

            public override void Clock()
            {
                RaisePropertyChanged(nameof(OutRegister));
            }

        }

    }
}
