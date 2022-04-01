using System.Diagnostics;
using DigitalElectronics.Concepts;
using FluentAssertions;
using NUnit.Framework;
using DigitalElectronics.Utilities;

[assembly: DebuggerDisplay("BitArray={DigitalElectronics.Utilities.Extensions.ToString(this)}", Target = typeof(BitArray))]

namespace DigitalElectronics.Modules.ALUs.Tests
{
    public class TestArithmeticLogicUnit
    {
        // Number of bits or 'N'
        private const int N = 4;

        private BitConverter _bitConverter;
        private ArithmeticLogicUnit _4bitAlu;

        [SetUp]
        public void SetUp()
        {
            _bitConverter = new BitConverter(Endianness.Little);
            _4bitAlu = new ArithmeticLogicUnit(N);
        }

        [Test]
        public void Constructor_GivenZeroNumberOfBits_ShouldThrow()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => new ArithmeticLogicUnit(0));
        }

        [Test]
        public void BitCount_ShouldBeN()
        {
            _4bitAlu.BitCount.Should().Be(N);
        }

        [Test]
        public void OutputIsNull_WhenInputEOIsLow()
        {
            _4bitAlu.SetInputEO(false);
            _4bitAlu.OutputE.Should().BeNull();
            _4bitAlu.SetInputEO(true);
            _4bitAlu.OutputE.Should().BeEquivalentTo(new BitArray(N).AsList<bool>());
        }

        [Test]
        public void ProbeState_ReturnsInternalState()
        {
            _4bitAlu.ProbeState().Should().BeEquivalentTo(new BitArray(N).AsList<bool>());
            _4bitAlu.SetInputA(_bitConverter.GetBits(3, N));
            _4bitAlu.SetInputB(_bitConverter.GetBits(5, N));
            _4bitAlu.ProbeState().Should().BeEquivalentTo(_bitConverter.GetBits(8, N).AsList<bool>());
        }

        [Test]
        public void TestAdditionFull()
        {
            _4bitAlu.SetInputEO(true);
            _4bitAlu.SetInputSu(false);

            for (int a = -8; a < 8; a++)
                for (int b = -8; b < 8; b++)
                    AssertSumOfAAndB(a, b, a + b);
        }

        [Test]
        public void TestSubtractionFull()
        {
            _4bitAlu.SetInputEO(true);
            _4bitAlu.SetInputSu(true);

            for (int a = -8; a < 8; a++)
                for (int b = -8; b < 8; b++)
                    AssertSumOfAAndB(a, b, a - b);
        }

        private void AssertSumOfAAndB(int a, int b, int expectedSum)
        {
            var dataA = _bitConverter.GetBits(a, N);
            _4bitAlu.SetInputA(dataA);
            var dataB = _bitConverter.GetBits(b, N);
            _4bitAlu.SetInputB(dataB);
            var expectation = _bitConverter.GetBits(expectedSum, N).AsList<bool>();
            _4bitAlu.OutputE.Should().BeEquivalentTo(expectation, $"a = {a}; b = {b}");
        }
    }
}
