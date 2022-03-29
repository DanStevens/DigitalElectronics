using System;
using System.Collections;
using DigitalElectronics.Concepts;
using FluentAssertions;
using NUnit.Framework;
using BitArray = DigitalElectronics.Concepts.BitArray;
using BitConverter = DigitalElectronics.Utilities.BitConverter;

namespace DigitalElectronics.Modules.Memory.Tests
{
    public class TestSixteenByteRAM
    {
        private BitConverter _bitConverter;
        private SixteenByteRAM _16ByteRAM;

        [SetUp]
        public void SetUp()
        {
            _bitConverter = new BitConverter(Endianness.Little);
            _16ByteRAM = new SixteenByteRAM();
        }

        [Test]
        public void SetInputA_ShouldThrowWhenLengthOfAddressParameterIsGreaterThan4()
        {
            var bitArray = _bitConverter.GetBits(0, 5);
            var ex = Assert.Throws<System.ArgumentOutOfRangeException>(() => _16ByteRAM.SetInputA(bitArray));
            ex.ParamName.Should().Be("address");
            ex.Message.Should().Be("Argument length cannot be greater than 4 (Parameter 'address')");
        }

        [Test]
        public void TestWriteAndReadEveryByteOfMemory()
        {
            _16ByteRAM.Output.Should().BeNull();

            WriteUniqueDataToEachMemoryLocation();
            ReadBackDataFromEachMemoryLocation();
            WriteZeroToEachMemoryLocation();
            ReadBackTheZerosFromEachMemoryLocation();

            _16ByteRAM.SetInputE(false);
            _16ByteRAM.Output.Should().BeNull();

            void WriteUniqueDataToEachMemoryLocation()
            {
                for (int address = 0; address < 16; address++)
                {
                    var addressBits = _bitConverter.GetBits(address, 4);
                    var data = _bitConverter.GetBits(32 + address, 8);
                    WriteToMemoryLocation(addressBits, data);
                }
            }

            void ReadBackDataFromEachMemoryLocation()
            {
                for (int address = 0; address < 16; address++)
                {
                    var addressBits = _bitConverter.GetBits(address, 4);
                    var expectedData = _bitConverter.GetBits(32 + address, 8);
                    VerifyMemoryLocation(addressBits, expectedData);
                }
            }

            void WriteZeroToEachMemoryLocation()
            {
                for (int address = 0; address < 16; address++)
                {
                    var addressBits = _bitConverter.GetBits(address, 4);
                    var data = _bitConverter.GetBits(0, 8);
                    WriteToMemoryLocation(addressBits, data);
                }
            }

            void ReadBackTheZerosFromEachMemoryLocation()
            {
                for (int address = 0; address < 16; address++)
                {
                    var addressBits = _bitConverter.GetBits(address, 4);
                    var expectedData = _bitConverter.GetBits(0, 8);
                    VerifyMemoryLocation(addressBits, expectedData);
                }
            }
        }

        private void VerifyMemoryLocation(BitArray addressBits, BitArray expectedData)
        {
            _16ByteRAM.SetInputA(addressBits);
            _16ByteRAM.SetInputE(true);
            _16ByteRAM.Output.Should().BeEquivalentTo(expectedData);
        }

        private void WriteToMemoryLocation(BitArray addressBits, BitArray data)
        {
            _16ByteRAM.SetInputA(addressBits);
            _16ByteRAM.SetInputL(true);
            _16ByteRAM.SetInputD(data);
            _16ByteRAM.Clock();
            _16ByteRAM.SetInputE(true);
            _16ByteRAM.Output.Should().BeEquivalentTo(data);
            _16ByteRAM.SetInputE(false);
            _16ByteRAM.Output.Should().BeNull();
        }
    }
}
