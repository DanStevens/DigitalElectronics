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
        public void InitialState_4Bits()
        {
            var fourBitBinaryCounter = new BinaryCounter(4);
            fourBitBinaryCounter.NumberOfBits.Should().Be(4);
            fourBitBinaryCounter.Output.ToByte().Should().Be(15);
            fourBitBinaryCounter.Output.Length.Should().Be(4);
        }

        [Test]
        public void InitialState_8Bits()
        {
            var fourBitBinaryCounter = new BinaryCounter(8);
            fourBitBinaryCounter.NumberOfBits.Should().Be(8);
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
    }
}
