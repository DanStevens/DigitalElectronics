using System.Linq;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Counters;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Modules.Tests.Counters.Tests
{
    [TestFixture]
    public class TestBinaryCounter
    {
        [Test]
        public void Ctor_ShouldThrowArgumentOutOfRangeException_WhenSizeInBitsArgIsNegative()
        {
            var ex = Assert.Throws<System.ArgumentOutOfRangeException>(() => new BinaryCounter(-1));
            ex.ParamName.Should().Be("sizeInBits");
            ex.Message.Should().StartWithEquivalentOf("Argument must be greater than 0");
        }

        [Test]
        public void InitialState_4Bits()
        {
            var fourBitBinaryCounter = new BinaryCounter(4);
            fourBitBinaryCounter.SizeInBits.Should().Be(4);
            fourBitBinaryCounter.Output.ToByte().Should().Be(15);
            fourBitBinaryCounter.Output.Length.Should().Be(4);
        }

        [Test]
        public void InitialState_8Bits()
        {
            var fourBitBinaryCounter = new BinaryCounter(8);
            fourBitBinaryCounter.SizeInBits.Should().Be(8);
            fourBitBinaryCounter.Output.ToByte().Should().Be(255);
            fourBitBinaryCounter.Output.Length.Should().Be(8);
        }


        [Test]
        public void Test4BitCounter()
        {
            var fourBitBinaryCounter = new BinaryCounter(4);

            foreach (int i in Enumerable.Range(0, 16))
            {
                fourBitBinaryCounter.Clock();
                fourBitBinaryCounter.Output.ToByte().Should().Be((byte)i);
                fourBitBinaryCounter.Output.Length.Should().Be(4);
            }
        }

        [Test]
        public void Test8BitCounter()
        {
            var fourBitBinaryCounter = new BinaryCounter(8);

            foreach (int i in Enumerable.Range(0, 256))
            {
                fourBitBinaryCounter.Clock();
                fourBitBinaryCounter.Output.ToByte().Should().Be((byte)i);
                fourBitBinaryCounter.Output.Length.Should().Be(8);
            }
        }

        [Test]
        public void SetAllInputs_UsingBitArrayOfSizeN()
        {
            var binary5 = new BitArray(true, false, true, false);
            var fourBitBinaryCounter = new BinaryCounter(4);
            fourBitBinaryCounter.Set(binary5);
            fourBitBinaryCounter.Output.ToByte().Should().Be(binary5.ToByte());
            fourBitBinaryCounter.Output.Length.Should().Be(4);
        }

        [Test]
        public void SetAllInputs_UsingBitArrayOfSizeNMinus1()
        {
            // Initialize all inputs to false
            var binary0 = new BitArray(byte.MinValue);
            var fourBitBinaryCounter = new BinaryCounter(4);
            fourBitBinaryCounter.Set(binary0);
            fourBitBinaryCounter.Output.ToByte().Should().Be(binary0.ToByte());

            BitArray binary5 = new BitArray(true, false, true);
            fourBitBinaryCounter.Set(binary5);
            fourBitBinaryCounter.Output.ToByte().Should().Be(binary5.ToByte());
        }

        [Test]
        public void SetAllInputs_UsingBitArrayOfSizeNPlus1()
        {
            BitArray binary21 = new BitArray(true, false, true, false, true);
            var fourBitBinaryCounter = new BinaryCounter(4);
            fourBitBinaryCounter.Set(binary21);
            fourBitBinaryCounter.Output.ToByte().Should().Be(5);
        }

    }
}
