using DigitalElectronics.Concepts;
using FluentAssertions;
using NUnit.Framework;
using BitConverter = DigitalElectronics.Utilities.BitConverter;

namespace DigitalElectronics.Modules.Memory.Tests
{
    public class TestSixteenByteRAM
    {
        private SixteenByteDARAM _16ByteRAM;
        private RamTester _ramTester;

        [SetUp]
        public void SetUp()
        {
            _16ByteRAM = new SixteenByteDARAM();
        }

        [Test]
        public void SetInputA_ShouldThrowWhenLengthOfAddressParameterIsGreaterThan4()
        {
            var bitArray = new BitConverter(Endianness.Little).GetBits(0, 5);
            var ex = Assert.Throws<System.ArgumentOutOfRangeException>(() => _16ByteRAM.SetInputA(bitArray));
            ex.ParamName.Should().Be("address");
            ex.Message.Should().Be("Argument length cannot be greater than 4 (Parameter 'address')");
        }

        [Test]
        public void TestWriteAndReadEveryByteOfMemory()
        {
            _ramTester = new RamTester(_16ByteRAM, 4);
            _ramTester.DoTest();
        }
    }
}
