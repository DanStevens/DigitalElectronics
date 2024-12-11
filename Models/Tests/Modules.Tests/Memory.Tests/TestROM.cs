using System;
using System.Linq;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Memory;
using FluentAssertions;
using NUnit.Framework;
using BitConverter = DigitalElectronics.Utilities.BitConverter;

namespace DigitalElectronics.Modules.Tests.Memory.Tests
{
    public class TestROM
    {
        private static byte[] bytes = Enumerable.Repeat(byte.MaxValue, 16).ToArray();

        private readonly BitConverter _bitConverter = new();

        [Test]
        public void ROM_ImplementsIOutputModule()
        {
            new ROM(bytes).Should().BeAssignableTo<IOutputModule>();
        }

        [Test]
        public void ROM_ImplementsIDedicatedAddressable()
        {
            new ROM(bytes).Should().BeAssignableTo<IDedicatedAddressable>();
        }

        [Test]
        public void Ctor_ShouldThrowArgumentNullException_WhenDataArgIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ROM(null));
            ex.ParamName.Should().Be("data");
        }

        [Test]
        public void Ctor_ShouldThrowArgumentException_WhenDataArgIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => new ROM(Enumerable.Empty<byte>()));
            ex.ParamName.Should().Be("data");
            ex.Message.Should().StartWithEquivalentOf("Argument must contain at least one byte");
        }

        [Test]
        public void WordSize_ShouldBeEight()
        {
            new ROM(bytes).WordSize.Should().Be(8);
        }

        [Test]
        public void Capacity()
        {
            for (int i = 1; i < 8; i++)
            {
                new ROM(Enumerable.Repeat(byte.MaxValue, i)).Capacity.Should().Be(i);
            }
        }

        [Test]
        public void MaxAddress()
        {
            for (int i = 1; i < 8; i++)
            {
                new ROM(Enumerable.Repeat(byte.MaxValue, i)).MaxAddress.Should().Be(i - 1);
            }
        }

        [Test]
        public void ProbeState()
        {
            for (int i = 1; i < 8; i++)
            {
                var data = Enumerable.Repeat(byte.MaxValue, i).ToArray();
                var objUT = new ROM(data);
                objUT.ProbeState().Should().BeEquivalentTo(data.Select(i => new BitArray(i)));
            }
        }

        [Test]
        public void ProbeAddress_ShouldBeEquivalentToZeroInitially()
        {
            new ROM(bytes).ProbeAddress().ToInt32().Should().Be(0);
        }

        [Test]
        public void SetInputA_ShouldChangeInternalAddress()
        {
            var objUT = new ROM(bytes);

            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                objUT.SetInputA(new BitArray(i, 8));
                objUT.ProbeAddress().ToInt32().Should().Be(i);
            }
        }

        [Test]
        public void SetInputA_ShouldThrowArgumentOutOfRangeException_WhenAddressIsGreaterThanMaxAddress()
        {
            var objUT = new ROM(bytes);
            objUT.MaxAddress.Should().Be(15);
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => objUT.SetInputA(_bitConverter.GetBits(-1)));
            ex.ParamName.Should().Be("address");
            ex.Message.Should().StartWithEquivalentOf("Address must be within range defined by AddressRange property");
        }

        [Test]
        public void SetInputA_ShouldThrowArgumentOutOfRangeException_WhenAddressIsLessThanZero()
        {
            var objUT = new ROM(bytes);
            objUT.MaxAddress.Should().Be(15);
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => objUT.SetInputA(_bitConverter.GetBits(-1)));
            ex.ParamName.Should().Be("address");
            ex.Message.Should().StartWithEquivalentOf("Address must be within range defined by AddressRange property");
        }

        [Test]
        public void SetInputA_ShouldChangeIndividualBitOfInternalAddress_WhenCalledWithLineIndex()
        {
            var objUT = new ROM(bytes);
            objUT.SetInputA(new BitArray(false, false, false, false));
            objUT.ProbeAddress().ToInt32().Should().Be(0);
            objUT.SetInputA(lineIndex: 0, true);
            objUT.ProbeAddress().ToInt32().Should().Be(1);
            objUT.SetInputA(lineIndex: 1, true);
            objUT.ProbeAddress().ToInt32().Should().Be(3);
            objUT.SetInputA(lineIndex: 2, true);
            objUT.ProbeAddress().ToInt32().Should().Be(7);
            objUT.SetInputA(lineIndex: 3, true);
            objUT.ProbeAddress().ToInt32().Should().Be(15);
            objUT.SetInputA(lineIndex: 4, true);
            objUT.ProbeAddress().ToInt32().Should().Be(31);
            objUT.SetInputA(lineIndex: 0, false);
            objUT.ProbeAddress().ToInt32().Should().Be(30);
            objUT.SetInputA(lineIndex: 1, false);
            objUT.ProbeAddress().ToInt32().Should().Be(28);
            objUT.SetInputA(lineIndex: 2, false);
            objUT.ProbeAddress().ToInt32().Should().Be(24);
            objUT.SetInputA(lineIndex: 3, false);
            objUT.ProbeAddress().ToInt32().Should().Be(16);
            objUT.SetInputA(lineIndex: 4, false);
            objUT.ProbeAddress().ToInt32().Should().Be(0);
        }

        [Test]
        public void Output_ShouldBeNullInitially()
        {
            new ROM(bytes).Output.Should().BeNull();
        }

        [Test]
        public void Output_ShouldBecomeNotNull_WhenSetInputEIsCalledWithTrue()
        {
            var objUT = new ROM(bytes);
            objUT.SetInputE(true);
            objUT.Output.Should().NotBeNull();
        }

        [Test]
        public void Output_ShouldBecomeNull_WhenSetInputEIsCalledWithFalse()
        {
            var objUT = new ROM(bytes);
            objUT.SetInputE(true);
            objUT.Output.Should().NotBeNull();
            objUT.SetInputE(false);
            objUT.Output.Should().BeNull();
        }

        [Test]
        public void Output_ShouldOutputWhateverValueIsStoredInTheCurrentAddress()
        {
            var primes = new byte[] {2, 3, 5, 7, 11, 13, 17, 19};
            var objUT = new ROM(primes);
            objUT.SetInputE(true);

            for (int a = 0; a < primes.Length; a++)
            {
                objUT.SetInputA(_bitConverter.GetBits(a, objUT.WordSize));
                objUT.Output!.Value.ToByte().Should().Be(primes[a]);
            }
        }
    }
}
