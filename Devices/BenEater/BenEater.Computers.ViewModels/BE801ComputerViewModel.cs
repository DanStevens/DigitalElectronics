using System;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.BenEater.Computers.ViewModels
{
    public class BE801ComputerViewModel
    {
        private readonly BE801Computer _computer;
        
        public ClockModuleViewModel ClockModule { get; }
        public BusViewModel BusModule { get; }
        public ProgramCounterViewModel ProgramCounterModule { get; }

        public BE801ComputerViewModel()
        {
            _computer = new();
            ClockModule = new();
            BusModule = new(_computer);
            ProgramCounterModule = new(_computer);
        }

        public void Clock()
        {
            _computer.Clock();
            ProgramCounterModule.Clock();
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
                        RaisePropertyChanged(nameof(ClockSpeed));
                        ClockSpeedChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }

            public event EventHandler? IsRunningChanged;

            public event EventHandler? ClockSpeedChanged;
        }

        public class ProgramCounterViewModel : ModuleViewModel
        {
            private readonly BE801Computer _computer;
            
            public ProgramCounterViewModel(BE801Computer computer)
            {
                _computer = computer ?? throw new ArgumentNullException(nameof(computer));
            }

            public BitArray State => _computer.ProbePC();

            public override void Clock()
            {
                RaisePropertyChanged(nameof(State));
            }
        }

        public class BusViewModel : ModuleViewModel
        {
            private readonly BE801Computer _computer;

            public BusViewModel(BE801Computer computer)
            {
                _computer = computer ?? throw new ArgumentNullException(nameof(computer));
            }

            public BitArray? State => _computer.ProbeBus();

            public override void Clock()
            {
                RaisePropertyChanged(nameof(State));
            }
        }
    }
}
