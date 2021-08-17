using System;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Modules.ALUs;

namespace DigitalElectronics.Computers
{
    public class Computer
    {
        private Register _registerA;
        private Register _registerB;
        private ArithmeticLogicUnit _alu;

        public Computer(Register registerA, Register registerB, ArithmeticLogicUnit alu)
        {
            _registerA = registerA ?? throw new ArgumentNullException(nameof(registerA));
            _registerB = registerB ?? throw new ArgumentNullException(nameof(registerB));
            _alu = alu ?? throw new ArgumentNullException(nameof(alu));
        }

        public void Clock()
        {
            _registerA.Clock();
            _registerB.Clock();

            // ALU is wired directly to register A and B, so we can bypass the buffered output and
            // access the registers' states directly
            _alu.SetInputA(_registerA.ProbeState());
            _alu.SetInputB(_registerB.ProbeState());
        }
    }
}
