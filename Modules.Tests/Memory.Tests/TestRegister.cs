using System;
using System.Collections;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.Memory.Tests
{
    public class TestRegister
    {
        // Number of bits or 'N'
        private const int NumberOfBits = 4;

        Register _4bitRegister;

        [SetUp]
        public void SetUp()
        {
            _4bitRegister = new Register(NumberOfBits);
            AssertOutputs(null, null, null, null);
            _4bitRegister.SetInputE(true);
        }

        [Test]
        public void BitCount_ShouldBe4()
        {
            _4bitRegister.BitCount.Should().Be(NumberOfBits);
        }

        [Test]
        public void Constructor_GivenZeroNumberOfBits_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Register(0));
        }

        [Test]
        public void SetAllInputsD_UsingArrayOfBoolsOfSizeN()
        {
            bool[] inputs = new bool[] { false, false, false, false };
            PushL();
            _4bitRegister.SetAllInputsD(inputs);
            Clock();
            AssertOutputs(false, false, false, false);
        }

        [Test]
        public void SetAllInputsD_UsingArrayOfBoolsOfSizeNMinus1()
        {
            // Initialize all inputs to false
            bool[] inputs = new bool[] { false, false, false, false };
            PushL();
            _4bitRegister.SetAllInputsD(inputs);
            Clock();
            AssertOutputs(false, false, false, false);

            bool[] inputs2 = new bool[] { true, false, true };
            PushL();
            _4bitRegister.SetAllInputsD(inputs2);
            Clock();
            AssertOutputs(true, false, true, false);
        }

        [Test]
        public void SetAllInputsD_UsingArrayOfBoolsOfSizeNPlus1()
        {
            bool[] inputs = new bool[] { true, false, true, false, true };
            PushL();
            _4bitRegister.SetAllInputsD(inputs);
            Clock();
            AssertOutputs(true, false, true, false);
        }

        [Test]
        public void SetAllInputsD_UsingBitArrayOfSizeN()
        {
            BitArray data = new BitArray(new bool[] { true, false, true, false });
            PushL();
            _4bitRegister.SetAllInputsD(data);
            Clock();
            AssertOutputs(true, false, true, false);
        }

        [Test]
        public void SetAllInputsD_UsingBitArrayOfSizeNMinus1()
        {
            // Initialize all inputs to false
            bool[] inputs = new bool[] { false, false, false, false };
            PushL();
            _4bitRegister.SetAllInputsD(inputs);
           Clock();
            AssertOutputs(false, false, false, false);

            BitArray data = new BitArray(new bool[] { true, false, true });
            PushL();
            _4bitRegister.SetAllInputsD(data);
           Clock();
            AssertOutputs(true, false, true, false);
        }

        [Test]
        public void SetAllInputsD_UsingBitArrayOfSizeNPlus1()
        {
            BitArray data = new BitArray(new bool[] { true, false, true, false, true });
            PushL();
            _4bitRegister.SetAllInputsD(data);
            Clock();
            AssertOutputs(true, false, true, false);
        }

        [Test]
        public void AllOutputsShouldBeHigh_OnInitialization()
        {
            AssertOutputs(true, true, true, true);
        }

        [Test]
        public void TestLatchInZero()
        {
            SetInputsD(false, false, false, false);
            AssertOutputs(true, true, true, true);

            SetInputL(true);
            AssertOutputs(true, true, true, true);

            Clock();
            AssertOutputs(false, false, false, false);
        }

        [Test]
        public void TestLatchInOne()
        {
            SetInputsD(false, false, false, true);
            AssertOutputs(true, true, true, true);
            
            PushL();
            AssertOutputs(true, true, true, true);

            Clock();
            AssertOutputs(false, false, false, true);
        }

        [Test]
        public void OutputOnlyChanges_WhenClocking_IfInputLIsHigh()
        {
            ReleaseL();
            SetInputsD(false, false, false, false);
            AssertOutputs(true, true, true, true);
            Clock();
            AssertOutputs(true, true, true, true);
            SetInputsD(false, false, false, true);
            Clock();
            AssertOutputs(true, true, true, true);

            PushL();
            SetInputsD(false, false, true, false);
            Clock();
            AssertOutputs(false, false, true, false);
            SetInputsD(false, false, true, true);
            Clock();
            AssertOutputs(false, false, true, true);
            
            ReleaseL();
            SetInputsD(false, true, false, false);
            Clock();
            AssertOutputs(false, false, true, true);
            PushL();
            Clock();
            AssertOutputs(false, true, false, false);
        }

        [Test]
        public void AllOutputsNull_WhenInputEIsLow()
        {
            ReleaseE();
            AssertOutputs(null, null, null, null);
            PushE();
            AssertOutputs(true, true, true, true);
            ReleaseE();
            AssertOutputs(null, null, null, null);

            PushL();
            AssertOutputs(null, null, null, null);
            SetInputsD(false, true, true, false);
            AssertOutputs(null, null, null, null);
            Clock();
            AssertOutputs(null, null, null, null);
            PushE();
            AssertOutputs(false, true, true, false);
        }

        [Test]
        public void AllOuputs_ShouldReturnBitArray_WhenInputEIsHigh()
        {
            BitArray data = new BitArray(new bool[] { true, false, true, false });
            PushE();
            PushL();
            _4bitRegister.SetAllInputsD(data);
            Clock();
            _4bitRegister.AllOutputs.Should().BeEquivalentTo(data);
        }

        [Test]
        public void AllOuputs_ShouldReturnNull_WhenInputEIsLow()
        {
            BitArray data = new BitArray(new bool[] { true, false, true, false });
            PushE();
            PushL();
            _4bitRegister.SetAllInputsD(data);
            Clock();
            ReleaseE();
            _4bitRegister.AllOutputs.Should().BeNull();
        }

        [Test]
        public void ProbeState_ReturnsInternalState()
        {
            BitArray data = new BitArray(new bool[] { true, false, true, false });
            PushE();
            PushL();
            _4bitRegister.SetAllInputsD(data);
            Clock();
            ReleaseE();
            _4bitRegister.AllOutputs.Should().BeNull();
            _4bitRegister.ProbeState().Should().BeEquivalentTo(data);
        }

        [Test]
        public void TestFrom0CountTo15()
        {
            for (byte i = 0; i <= 15; i++)
            {
                var iBits = new BitArray(new byte[] { i });
                SetInputsD(iBits);
                Clock();
            }
        }

        private void AssertOutputs(params bool?[] expectedOutputs)
        {
            for (int x = 0; x < _4bitRegister.BitCount; x++)
            {
                _4bitRegister.GetOutputQx(x).Should().Be(expectedOutputs[x], $"Bit #{x}");
            }
        }

        void SetInputDx(int x, bool value) => _4bitRegister.SetInputDx(x, value);

        void SetInputsD(params bool[] values)
        {
            for (int x = 0; x < values.Length; x++) SetInputDx(x, values[x]);
        }

        void SetInputsD(BitArray bitArray)
        {
            for (int x = 0; x < _4bitRegister.BitCount; x++) SetInputDx(x, bitArray.Get(x));
        }
        
        void SetInputL(bool value) => _4bitRegister.SetInputL(value);

        void PushL() => SetInputL(true);

        void ReleaseL() => SetInputL(false);

        void Clock() => _4bitRegister.Clock();

        private void PushE() => _4bitRegister.SetInputE(true);

        private void ReleaseE() => _4bitRegister.SetInputE(false);
    }
}
