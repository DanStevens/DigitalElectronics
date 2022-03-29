using System;
using System.Collections;
using DigitalElectronics.Components.Memory;
using NUnit.Framework;
using FluentAssertions;
using DigitalElectronics.Utilities;
using DigitalElectronics.Modules.ALUs;
using System.Diagnostics;
using DigitalElectronics.Concepts;
using BitConverter = DigitalElectronics.Utilities.BitConverter;
using BitArray = DigitalElectronics.Concepts.BitArray;

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

            _registerA.ProbeState().Should().BeEquivalentTo(new BitArray(NumberOfBits, true));
            _registerB.ProbeState().Should().BeEquivalentTo(new BitArray(NumberOfBits, true));

            // Load data into register A
            _registerA.SetInputL(true);
            _registerA.SetInputD(data);
             _computer.Clock();
            _registerA.SetInputL(false);
            _registerA.ProbeState().Should().BeEquivalentTo(data);

            // Enable register A for reading
            _registerA.SetInputE(true);
            _registerA.Output.Should().BeEquivalentTo(data);

            // Load data into register B from register A
            _registerB.SetInputL(true);
            _registerB.SetInputD(_registerA.Output);
             _computer.Clock();
            _registerB.SetInputL(false);
            _registerB.ProbeState().Should().BeEquivalentTo(data);
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
            _registerA.ProbeState().Should().BeEquivalentTo(binary20);

            // Load 23 into register B
            _registerB.SetInputL(true);
            _registerB.SetInputD(binary23);
            _computer.Clock();
            _registerB.ProbeState().Should().BeEquivalentTo(binary23);

            // Verify state of ALU
            _alu.ProbeState().Should().BeEquivalentTo(result1);
            _alu.SetInputEO(true);
            _alu.OutputE.Should().BeEquivalentTo(result1);

            // Load ALU result into register A
            _registerA.SetInputL(true);
            _registerA.SetInputD(_alu.OutputE);
            _computer.Clock();
            _registerA.ProbeState().Should().BeEquivalentTo(result1);
            _registerA.SetInputL(false);

            // Verify state of ALU
            BitArray result2 = _bitConverter.GetBits(20 + 23 + 23, NumberOfBits);
            _alu.ProbeState().Should().BeEquivalentTo(result2);
            _alu.OutputE.Should().BeEquivalentTo(result2);
        }
    }
}
