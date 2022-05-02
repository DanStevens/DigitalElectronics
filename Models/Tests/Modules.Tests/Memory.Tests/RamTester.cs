using System;
using System.Linq;
using DigitalElectronics.Concepts;
using FluentAssertions;
using BitConverter = DigitalElectronics.Utilities.BitConverter;

namespace DigitalElectronics.Modules.Memory.Tests
{
    /// <summary>
    /// Tests any class that implements <see cref="IRAM"/> by writing to and verifying every
    /// memory location
    /// </summary>
    public class RamTester
    {
        private readonly BitConverter _bitConverter;
        private readonly IRAM _ram;
        private readonly int _addressSize;
        private BitArray[] _testData;

        public RamTester(IRAM ram, int addressSize)
        {
            _ram = ram ?? throw new ArgumentNullException(nameof(ram));

            if (addressSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(addressSize), "Argument must be greater than zero");
            
            _addressSize = addressSize;
            _bitConverter = new BitConverter();
        }

        public void DoTest()
        {
            _testData = Enumerable.Range(0, _ram.Capacity)
                .Select(a => _bitConverter.GetBits(32 + a, _ram.WordSize))
                .ToArray();

            _ram.Output.Should().BeNull();

            WriteUniqueDataToEachMemoryLocation();
            ReadBackDataFromEachMemoryLocation();
            _ram.ProbeState().Should().BeEquivalentTo(_testData);
            WriteZeroToEachMemoryLocation();
            ReadBackTheZerosFromEachMemoryLocation();

            _ram.SetInputE(false);
            _ram.Output.Should().BeNull();
        }

        private void ReadBackTheZerosFromEachMemoryLocation()
        {
            for (int address = 0; address < _ram.Capacity; address++)
            {
                var addressBits = _bitConverter.GetBits(address, _addressSize);
                var expectedData = _bitConverter.GetBits(0, 8);
                VerifyMemoryLocation(addressBits, expectedData);
            }
        }

        private void WriteZeroToEachMemoryLocation()
        {
            for (int address = 0; address < _ram.Capacity; address++)
            {
                var addressBits = _bitConverter.GetBits(address, _addressSize);
                var data = _bitConverter.GetBits(0, 8);
                WriteToMemoryLocation(addressBits, data);
            }
        }

        private void ReadBackDataFromEachMemoryLocation()
        {
            for (int address = 0; address < _ram.Capacity; address++)
            {
                var addressBits = _bitConverter.GetBits(address, _addressSize);
                var expectedData = _testData[address];
                VerifyMemoryLocation(addressBits, expectedData);
            }
        }

        private void WriteUniqueDataToEachMemoryLocation()
        {
            for (int address = 0; address < _ram.Capacity; address++)
            {
                var addressBits = _bitConverter.GetBits(address, _addressSize);
                var data = _testData[address];
                WriteToMemoryLocation(addressBits, data);
            }
        }

        private void VerifyMemoryLocation(BitArray addressBits, BitArray expectedData)
        {
            SetAddress(addressBits);
            _ram.SetInputE(true);
            _ram.Output.Should().BeEquivalentTo(expectedData);
            _ram.SetInputE(false);
        }

        private void WriteToMemoryLocation(BitArray addressBits, BitArray data)
        {
            SetAddress(addressBits);
            _ram.SetInputLD(true);
            _ram.SetInputD(data);
            _ram.Clock();
            _ram.SetInputE(true);
            _ram.Output.Should().BeEquivalentTo(data);
            _ram.SetInputE(false);
            _ram.Output.Should().BeNull();
            _ram.ProbeState(addressBits).Should().BeEquivalentTo(data);
            _ram.SetInputLD(false);
        }

        private void SetAddress(BitArray addressBits)
        {
            if (_ram is ISharedAddrDataInput iaram)
            {
                iaram.SetInputLA(true);
                iaram.SetInputS(addressBits);
                _ram.Clock();
                iaram.SetInputLA(false);
                return;
            }

            if (_ram is IDedicatedAddrInput daram)
                daram.SetInputA(addressBits);
        }
    }
}
