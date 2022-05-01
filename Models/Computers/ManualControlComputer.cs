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
        private const int WordSize = 8;

        private readonly ArithmeticLogicUnit _alu;
        private readonly Register _aRegister;
        private readonly Register _bRegister;
        private readonly Register _outRegister;

        public ParallelBus Bus { get; }
        public IRegister ARegister => _aRegister;
        public IRegister BRegister => _bRegister;
        public IArithmeticLogicUnit ALU => _alu;
        public IRAM RAM { get; }
        ////private IRegister AddressRegister { get; }
        ////private IRegister InstructionRegister { get; }
        public IRegister OutRegister => _outRegister;

        public ManualControlComputer()
        {
            RAM = new SixteenByteRAM();
            _aRegister = new Register(WordSize);
            _bRegister = new Register(WordSize);
            _alu = new ArithmeticLogicUnit(WordSize);
            _outRegister = new Register(WordSize);

            Bus = new ParallelBus(WordSize,
                RAM, _aRegister, _bRegister, _alu, _outRegister);
        }

        public void Clock()
        {
            Bus.Transfer();
            RAM.Clock();
            ARegister.Clock();
            BRegister.Clock();
            OutRegister.Clock();
        }
    }
}
