using System;
using System.Collections.ObjectModel;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.BenEater.Computers.ViewModels
{
    public class BE801ComputerViewModel
    {
        private readonly BE801Computer _computer;
        
        public ClockModuleViewModel ClockModule { get; }
        public BusViewModel BusModule { get; }
        public ProgramCounterViewModel ProgramCounterModule { get; }
        public MemoryModuleViewModel MemoryModule { get; }
        public ALUViewModel ALUModule { get; }

        public BE801ComputerViewModel()
        {
            _computer = new();
            ClockModule = new();
            BusModule = new(_computer);
            ProgramCounterModule = new(_computer);
            MemoryModule = new(_computer);
            ALUModule = new(_computer);
        }

        public void Clock()
        {
            _computer.Clock();
            ClockModule.Clock();
            BusModule.Clock();
            ProgramCounterModule.Clock();
            MemoryModule.Clock();
            ALUModule.Clock();
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

        public class MemoryModuleViewModel : ModuleViewModel
        {
            private readonly BE801Computer _computer;

            public MemoryModuleViewModel(BE801Computer computer)
            {
                _computer = computer ?? throw new ArgumentNullException(nameof(computer));
            }

            /// <summary>
            /// Contents of Memory Address Register (MAR)
            /// </summary>
            public BitArray MAR => _computer.ProbeMAR();

            /// <summary>
            /// Contents of the 16-byte RAM
            /// </summary>
            public IList<BitArray> MemoryContents => _computer.ProbeRAM();

            public override void Clock()
            {
                RaisePropertyChanged(nameof(MAR));
                RaisePropertyChanged(nameof(MemoryContents));
            }
        }

        public class ALUViewModel : ModuleViewModel
        {
            private readonly BE801Computer _computer;

            public ALUViewModel(BE801Computer computer)
            {
                _computer = computer ?? throw new ArgumentNullException(nameof(computer));
            }

            public BitArray ARegister => _computer.ProbeARegister();

            public BitArray ALU => _computer.ProbeALU();

            public BitArray BRegister => _computer.ProbeBRegister();

            public override void Clock()
            {
                RaisePropertyChanged(nameof(ARegister));
                RaisePropertyChanged(nameof(ALU));
                RaisePropertyChanged(nameof(BRegister));
            }
        }
    }
}
