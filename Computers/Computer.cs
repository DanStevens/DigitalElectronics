using System;
using DigitalElectronics.Components.Memory;

namespace DigitalElectronics.Computers
{
    public class Computer
    {
        private Register _registerA;
        private Register _registerB;

        public Computer(Register registerA, Register registerB)
        {
            _registerA = registerA ?? throw new ArgumentNullException(nameof(registerA));
            _registerB = registerB ?? throw new ArgumentNullException(nameof(registerB));
        }

        public void Clock()
        {
            _registerA.Clock();
            _registerB.Clock();
        }
    }
}
