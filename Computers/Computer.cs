using System;
using DigitalElectronics.Components.Memory;

namespace DigitalElectronics.Computers
{
    public class Computer
    {
        private NBitRegister _registerA;
        private NBitRegister _registerB;

        public Computer(NBitRegister registerA, NBitRegister registerB)
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
