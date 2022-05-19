using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Memory;
using DigitalElectronics.Modules.Memory.Tests;
using DigitalElectronics.Utilities;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Modules.Tests.Memory.Tests
{
    public class TestSixteenByteIARAM
    {
        private BitConverter _bitConverter;

        [SetUp]
        public void SetUp()
        {
            _bitConverter = new BitConverter(Endianness.Little);
        }

        [Test]
        public void ProbeAddress_ShouldReturnProbeStateOfAddressRegister()
        {
            var ramModule = new SixteenByteIARAM();
            ramModule.ProbeAddress().Length.Should().Be(4);
            ramModule.ProbeAddress().ToByte().Should().Be(15);
        }

        [Test]
        public void Clock_ShouldUpdateAddressRegisterState_WhenSetInputLAIsTrue()
        {
            var ramModule = new SixteenByteIARAM();
            ramModule.ProbeAddress().ToByte().Should().Be(15);
            ramModule.SetInputLA(true);
            ramModule.SetInputLD(false);
            ramModule.SetInputS(_bitConverter.GetBits((byte)5, 4));
            
            ramModule.Clock();

            ramModule.ProbeAddress().Length.Should().Be(4);
            ramModule.ProbeAddress().ToByte().Should().Be(5);
        }

        [Test]
        public void Clock_ShouldUpdateAddressRegisterState_UsingFirst4BitsOfDataOnly_WhenSetInputLAIsTrue_AndSetInputIsCalledWithArgRepresentingValueGreaterThan15()
        {
            var ramModule = new SixteenByteIARAM();
            ramModule.ProbeAddress().ToByte().Should().Be(15);
            ramModule.SetInputLA(true);
            ramModule.SetInputLD(false);
            ramModule.SetInputS(_bitConverter.GetBits((byte)205, 8));

            ramModule.Clock();

            ramModule.ProbeAddress().Length.Should().Be(4);
            ramModule.ProbeAddress().ToByte().Should().Be(13); // 13 is the result of truncating binary 205 to 4 bits
            ramModule.ProbeAddress().ToByte().Should().NotBe(205);
        }


        [Test]
        public void Clock_ShouldUpdateRamState_WhenSetInputLIsTrue()
        {
            var address0x0 = _bitConverter.GetBits((byte)0, 4);
            var ramModule = new SixteenByteIARAM();

            // Set address to 0
            ramModule.SetInputLA(true);
            ramModule.SetInputS(address0x0);
            ramModule.Clock();
            ramModule.SetInputLA(false);
            ramModule.ProbeAddress().ToByte().Should().Be(0);

            ramModule.ProbeState(address0x0).ToByte().Should().Be(byte.MaxValue);
            ramModule.SetInputLA(false);
            ramModule.SetInputLD(true);
            ramModule.SetInputS(_bitConverter.GetBits((byte)7, 8));

            ramModule.Clock();

            ramModule.ProbeAddress().ToByte().Should().Be(0);
            ramModule.ProbeAddress().ToByte().Should().NotBe(7);
            ramModule.ProbeState(address0x0).ToByte().Should().Be(7);
        }

        // This tests documents the fact that it's possible for both the address register and
        // RAM module state to be set in a single call to `Clock`, if both the 'load address'
        // and 'load' inputs are enabled. While this is possible, it's likely not desirable to
        // do this in practice.
        [Test]
        public void Clock_CanSetBothAddressRegisterAndRamInSingleCall()
        {
            var address0x0 = _bitConverter.GetBits((byte)0, 4);
            var ramModule = new SixteenByteIARAM();

            // Set address to 0
            ramModule.SetInputLA(true);
            ramModule.SetInputS(address0x0);
            ramModule.Clock();
            ramModule.SetInputLA(false);
            ramModule.ProbeAddress().ToByte().Should().Be(0);

            ramModule.ProbeState(address0x0).ToByte().Should().Be(byte.MaxValue);
            ramModule.SetInputLA(true);
            ramModule.SetInputLD(true);
            ramModule.SetInputS(_bitConverter.GetBits((byte)205, 8));

            ramModule.Clock();

            ramModule.ProbeAddress().ToByte().Should().Be(13); // 13 is the result of truncating binary 205 to 4 bits
            ramModule.ProbeState(address0x0).ToByte().Should().Be(205);
        }

        [Test]
        public void TestWriteAndReadEveryByteOfMemory()
        {
            var ramModule = new SixteenByteIARAM();
            var ramTester = new RamTester(ramModule, 4);
            ramTester.DoTest();
        }

        [Test]
        public void MaxAddress_ShouldBeZeroTo16()
        {
            var ramModule = new SixteenByteIARAM();
            ramModule.MaxAddress.Should().Be(15);
        }
    }
}
