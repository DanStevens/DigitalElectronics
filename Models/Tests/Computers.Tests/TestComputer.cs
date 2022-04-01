using DigitalElectronics.Components.Memory;
using NUnit.Framework;
using FluentAssertions;
using DigitalElectronics.Utilities;
using DigitalElectronics.Modules.ALUs;
using System.Diagnostics;
using DigitalElectronics.Concepts;

[assembly: DebuggerDisplay("BitArray={DigitalElectronics.Utilities.Extensions.ToString(this)}", Target = typeof(BitArray))]

namespace DigitalElectronics.Computers.Tests
{
    public class TestComputer
    {
        private const int NumberOfBits = 8;

        private BitConverter _bitConverter;
        private Register _registerA;
        private Register _registerB;
        private ArithmeticLogicUnit _alu;
        private Computer _computer;

        [SetUp]
        public void SetUp()
        {
            _bitConverter = new BitConverter(Endianness.Little);
            _registerA = new Register(NumberOfBits);
            _registerB = new Register(NumberOfBits);
            _alu = new ArithmeticLogicUnit(NumberOfBits);
            _computer = new Computer(_registerA, _registerB, _alu);
        }

        [Test]
        public void TestBusTransferBetweenTwo8bitRegisters()
        {
            BitArray data = _bitConverter.GetBits(47, NumberOfBits);

            _registerA.ProbeState().Should().BeEquivalentTo(new BitArray(NumberOfBits, true).AsList<bool>());
            _registerB.ProbeState().Should().BeEquivalentTo(new BitArray(NumberOfBits, true).AsList<bool>());

            // Load data into register A
            _registerA.SetInputL(true);
            _registerA.SetInputD(data);
             _computer.Clock();
            _registerA.SetInputL(false);
            _registerA.ProbeState().Should().BeEquivalentTo(data.AsList<bool>());

            // Enable register A for reading
            _registerA.SetInputE(true);
            _registerA.Output.Should().BeEquivalentTo(data.AsList<bool>());

            // Load data into register B from register A
            _registerB.SetInputL(true);
            _registerB.SetInputD(_registerA.Output);
             _computer.Clock();
            _registerB.SetInputL(false);
            _registerB.ProbeState().Should().BeEquivalentTo(data.AsList<bool>());
        }

        [Test]
        public void TestAdding20And23()
        {
            BitArray binary20 = _bitConverter.GetBits(20, NumberOfBits);
            BitArray binary23 = _bitConverter.GetBits(23, NumberOfBits);
            BitArray result1 = _bitConverter.GetBits(20 + 23, NumberOfBits);

            // Load 20 into register A
            _registerA.SetInputL(true);
            _registerA.SetInputD(binary20);
            _computer.Clock();
            _registerA.ProbeState().Should().BeEquivalentTo(binary20.AsList<bool>());

            // Load 23 into register B
            _registerB.SetInputL(true);
            _registerB.SetInputD(binary23);
            _computer.Clock();
            _registerB.ProbeState().Should().BeEquivalentTo(binary23.AsList<bool>());

            // Verify state of ALU
            _alu.ProbeState().Should().BeEquivalentTo(result1.AsList<bool>());
            _alu.SetInputEO(true);
            _alu.OutputE.Should().BeEquivalentTo(result1.AsList<bool>());

            // Load ALU result into register A
            _registerA.SetInputL(true);
            _registerA.SetInputD(_alu.OutputE);
            _computer.Clock();
            _registerA.ProbeState().Should().BeEquivalentTo(result1.AsList<bool>());
            _registerA.SetInputL(false);

            // Verify state of ALU
            BitArray result2 = _bitConverter.GetBits(20 + 23 + 23, NumberOfBits);
            _alu.ProbeState().Should().BeEquivalentTo(result2.AsList<bool>());
            _alu.OutputE.Should().BeEquivalentTo(result2.AsList<bool>());
        }
    }
}
