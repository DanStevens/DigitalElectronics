using System.Collections;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.Memory.Tests
{
    class TestNBitRegister
    {
        NBitRegister _4bitRegister;

        [SetUp]
        public void SetUp()
        {
            _4bitRegister = new NBitRegister(4);
        }

        [Test]
        public void BitCount_ShouldBe4()
        {
            _4bitRegister.BitCount.Should().Be(4);
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
            for (int x = 0; x < _4bitRegister.BitCount; x++)
            {
                _4bitRegister.GetOutputQx(x).Should().Be(expectedOutputs[x]);
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
    }
}
