using System.Collections;
using DigitalElectronics.Components.Memory;
using NUnit.Framework;
using FluentAssertions;

namespace DigitalElectronics.Computers.Tests
{
    public class TestComputer
    {
        private const int NumberOfBits = 8;

        private NBitRegister _registerA;
        private NBitRegister _registerB;
        private Computer _computer;

        [SetUp]
        public void SetUp()
        {
            _registerA = new NBitRegister(NumberOfBits);
            _registerB = new NBitRegister(NumberOfBits);
            _computer = new Computer(_registerA, _registerB);
        }

        [Test]
        public void TestBusTransferBetweenTwo8bitRegisters()
        {
            BitArray data = new BitArray(new byte[] { 47 });

            _registerA.ProbeState().Should().BeEquivalentTo(new BitArray(NumberOfBits, true));
            _registerB.ProbeState().Should().BeEquivalentTo(new BitArray(NumberOfBits, true));

            // Load data into register A
            _registerA.SetInputL(true);
            _registerA.SetAllInputsD(data);
             _computer.Clock();
            _registerA.ProbeState().Should().BeEquivalentTo(data);

            // Enable register A for reading
            _registerA.SetInputE(true);
            _registerA.AllOutputs.Should().BeEquivalentTo(data);

            // Load data into register B from register A
            _registerB.SetInputL(true);
            _registerB.SetAllInputsD(_registerA.AllOutputs);
             _computer.Clock();
            _registerB.ProbeState().Should().BeEquivalentTo(data);
        }
    }
}
