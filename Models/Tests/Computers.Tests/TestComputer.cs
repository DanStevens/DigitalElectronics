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
        private const int SizeInBits = 8;

        private BitConverter _bitConverter;
        private Register _registerA;
        private Register _registerB;
        private ArithmeticLogicUnit _alu;
        private Computer _computer;

        [SetUp]
        public void SetUp()
        {
            _bitConverter = new BitConverter(Endianness.Little);
            _registerA = new Register(SizeInBits);
            _registerB = new Register(SizeInBits);
            _alu = new ArithmeticLogicUnit(SizeInBits);
            _computer = new Computer(_registerA, _registerB, _alu);
        }

        [Test]
        public void TestBusTransferBetweenTwo8bitRegisters()
        {
            BitArray data = _bitConverter.GetBits(47, SizeInBits);

            _registerA.ProbeState().Should().BeEquivalentTo(new BitArray(SizeInBits, true).AsReadOnlyList<bool>());
            _registerB.ProbeState().Should().BeEquivalentTo(new BitArray(SizeInBits, true).AsReadOnlyList<bool>());

            // Load data into register A
            _registerA.SetInputL(true);
            _registerA.SetInputD(data);
             _computer.Clock();
            _registerA.SetInputL(false);
            _registerA.ProbeState().Should().BeEquivalentTo(data.AsReadOnlyList<bool>());

            // Enable register A for reading
            _registerA.SetInputE(true);
            _registerA.Output.Should().BeEquivalentTo(data.AsReadOnlyList<bool>());

            // Load data into register B from register A
            _registerB.SetInputL(true);
            _registerB.SetInputD(_registerA.Output);
             _computer.Clock();
            _registerB.SetInputL(false);
            _registerB.ProbeState().Should().BeEquivalentTo(data.AsReadOnlyList<bool>());
        }

        [Test]
        public void TestAdding20And23()
        {
            BitArray binary20 = _bitConverter.GetBits(20, SizeInBits);
            BitArray binary23 = _bitConverter.GetBits(23, SizeInBits);
            BitArray result1 = _bitConverter.GetBits(20 + 23, SizeInBits);

            // Load 20 into register A
            _registerA.SetInputL(true);
            _registerA.SetInputD(binary20);
            _computer.Clock();
            _registerA.ProbeState().Should().BeEquivalentTo(binary20.AsReadOnlyList<bool>());

            // Load 23 into register B
            _registerB.SetInputL(true);
            _registerB.SetInputD(binary23);
            _computer.Clock();
            _registerB.ProbeState().Should().BeEquivalentTo(binary23.AsReadOnlyList<bool>());

            // Verify state of ALU
            _alu.ProbeState().Should().BeEquivalentTo(result1.AsReadOnlyList<bool>());
            _alu.SetInputEO(true);
            _alu.OutputE.Should().BeEquivalentTo(result1.AsReadOnlyList<bool>());

            // Load ALU result into register A
            _registerA.SetInputL(true);
            _registerA.SetInputD(_alu.OutputE);
            _computer.Clock();
            _registerA.ProbeState().Should().BeEquivalentTo(result1.AsReadOnlyList<bool>());
            _registerA.SetInputL(false);

            // Verify state of ALU
            BitArray result2 = _bitConverter.GetBits(20 + 23 + 23, SizeInBits);
            _alu.ProbeState().Should().BeEquivalentTo(result2.AsReadOnlyList<bool>());
            _alu.OutputE.Should().BeEquivalentTo(result2.AsReadOnlyList<bool>());
        }
    }
}
