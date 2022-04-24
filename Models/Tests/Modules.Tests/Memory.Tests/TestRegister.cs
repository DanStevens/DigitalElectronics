using System;
using FluentAssertions;
using NUnit.Framework;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Components.Memory.Tests
{
    public class TestRegister
    {
        // Number of bits or 'N'
        private const int SizeInBits = 4;

        Register _4bitRegister;

        [SetUp]
        public void SetUp()
        {
            _4bitRegister = new Register(SizeInBits);
            AssertOutputIsNull();
            _4bitRegister.SetInputE(true);
        }

        [Test]
        public void SizeInBits_ShouldBe4()
        {
            _4bitRegister.SizeInBits.Should().Be(SizeInBits);
        }

        [Test]
        public void Constructor_GivenZeroNumberOfBits_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Register(0));
        }

        [Test]
        public void SetAllInputsD_UsingBitArrayOfSizeN()
        {
            BitArray data = new BitArray(new bool[] { true, false, true, false });
            PushL();
            _4bitRegister.SetInputD(data);
            Clock();
            AssertOutputs(true, false, true, false);
        }

        [Test]
        public void SetAllInputsD_UsingBitArrayOfSizeNMinus1()
        {
            // Initialize all inputs to false
            var inputs = new BitArray(new bool[] { false, false, false, false });
            PushL();
            _4bitRegister.SetInputD(inputs);
            Clock();
            AssertOutputs(false, false, false, false);

            BitArray data = new BitArray(new bool[] { true, false, true });
            PushL();
            _4bitRegister.SetInputD(data);
           Clock();
            AssertOutputs(true, false, true, false);
        }

        [Test]
        public void SetAllInputsD_UsingBitArrayOfSizeNPlus1()
        {
            BitArray data = new BitArray(new bool[] { true, false, true, false, true });
            PushL();
            _4bitRegister.SetInputD(data);
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
        public void OutputsNull_WhenInputEIsLow()
        {
            ReleaseE();
            AssertOutputIsNull();
            PushE();
            AssertOutputs(true, true, true, true);
            ReleaseE();
            AssertOutputIsNull();

            PushL();
            AssertOutputIsNull();
            SetInputsD(false, true, true, false);
            AssertOutputIsNull();
            Clock();
            AssertOutputIsNull();
            PushE();
            AssertOutputs(false, true, true, false);
        }

        [Test]
        public void Ouput_ShouldReturnBitArray_WhenInputEIsHigh()
        {
            BitArray data = new BitArray(new bool[] { true, false, true, false });
            PushE();
            PushL();
            _4bitRegister.SetInputD(data);
            Clock();
            _4bitRegister.Output.Should().BeEquivalentTo(data.AsReadOnlyList<bool>());
        }

        [Test]
        public void OuputsShouldReturnNull_WhenInputEIsLow()
        {
            BitArray data = new BitArray(new bool[] { true, false, true, false });
            PushE();
            PushL();
            _4bitRegister.SetInputD(data);
            Clock();
            ReleaseE();
            _4bitRegister.Output.Should().BeNull();
        }

        [Test]
        public void ProbeState_ReturnsInternalState()
        {
            BitArray data = new BitArray(new bool[] { true, false, true, false });
            PushE();
            PushL();
            _4bitRegister.SetInputD(data);
            Clock();
            ReleaseE();
            _4bitRegister.Output.Should().BeNull();
            _4bitRegister.ProbeState().Should().BeEquivalentTo(data.AsReadOnlyList<bool>());
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

        private void AssertOutputs(params bool[] expectedOutputs)
        {
            var expectedData = new BitArray(expectedOutputs).AsReadOnlyList<bool>();
            _4bitRegister.Output.Should().BeEquivalentTo(expectedData);
        }

        private void AssertOutputIsNull()
        {
            _4bitRegister.Output.Should().BeNull();
        }

        void SetInputsD(params bool[] bits)
        {
            SetInputsD(new BitArray(bits));
        }

        void SetInputsD(BitArray data)
        {
            _4bitRegister.SetInputD(data);
        }
        
        void SetInputL(bool value) => _4bitRegister.SetInputL(value);

        void PushL() => SetInputL(true);

        void ReleaseL() => SetInputL(false);

        void Clock() => _4bitRegister.Clock();

        private void PushE() => _4bitRegister.SetInputE(true);

        private void ReleaseE() => _4bitRegister.SetInputE(false);
    }
}
