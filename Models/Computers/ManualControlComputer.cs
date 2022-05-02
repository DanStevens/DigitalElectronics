using DigitalElectronics.Components.Memory;
using DigitalElectronics.Modules;
using DigitalElectronics.Modules.ALUs;
using DigitalElectronics.Modules.Comms;
using DigitalElectronics.Modules.Memory;

namespace DigitalElectronics.Computers
{

    /// <summary>
    /// A bare-bones 8-bit manual control computer, meaning it doesn't have a control logic module.
    /// The operator must manually enable/disable module input/output and advance the clock.
    /// </summary>
    public class ManualControlComputer
    {
        private const int AddressSize = 4;
        private const int WordSize = 8;

        private readonly Register _addressRegister;
        private readonly SixteenByteDARAM _ram;
        private readonly ArithmeticLogicUnit _alu;
        private readonly Register _aRegister;
        private readonly Register _bRegister;
        private readonly Register _outRegister;
        private readonly ParallelBus _bus;

        public IWritableRegister AddressRegister => _addressRegister;
        public IDARAM RAM => _ram;
        public IReadWriteRegister ARegister => _aRegister;
        public IReadWriteRegister BRegister => _bRegister;
        public IArithmeticLogicUnit ALU => _alu;
        ////private IRegister InstructionRegister { get; }
        public IWritableRegister OutRegister => _outRegister;
        public ParallelBus Bus => _bus;

        public ManualControlComputer()
        {
            _addressRegister = new Register(AddressSize);
            _ram = new SixteenByteDARAM();
            _aRegister = new Register(WordSize);
            _bRegister = new Register(WordSize);
            _alu = new ArithmeticLogicUnit(WordSize);
            _outRegister = new Register(WordSize);

            _bus = new ParallelBus(WordSize,
                _ram, _aRegister, _bRegister, _alu, _outRegister);
        }

        public void Clock()
        {
            Bus.Transfer();
            _addressRegister.Clock();
            RAM.Clock();
            ARegister.Clock();
            BRegister.Clock();
            OutRegister.Clock();
        }
    }
}
