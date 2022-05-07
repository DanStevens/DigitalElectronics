using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Counters;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Modules.Tests.Counters.Tests
{
    public class TestProgramCounter
    {
        private const int AddressSize = 4;

        [Test]
        public void Ctor_ShouldThrowArgumentOutOfRangeException_WhenSizeInBitsArgIsNegative()
        {
            var ex = Assert.Throws<System.ArgumentOutOfRangeException>(() => new ProgramCounter(-1));
            ex!.ParamName.Should().Be("addressSize");
            ex.Message.Should().StartWithEquivalentOf("Argument must be greater than 0");
        }

        [Test]
        public void Ctor_ShouldThrowArgumentOutOfRangeException_WhenSizeInBitsArgIsZero()
        {
            var ex = Assert.Throws<System.ArgumentOutOfRangeException>(() => new ProgramCounter(0));
            ex!.ParamName.Should().Be("addressSize");
            ex.Message.Should().StartWithEquivalentOf("Argument must be greater than 0");
        }

        [Test]
        public void InitialState()
        {
            var programCounter = new ProgramCounter(AddressSize);
            programCounter.AddressSize.Should().Be(AddressSize);
            programCounter.Output.Should().BeNull();
            programCounter.ProbeState().ToByte().Should().Be(15);
        }

        [Test]
        public void SetInputCE_ShouldIncrementCount_WhenSetToTrue_UponClockCalled()
        {
            var programCounter = new ProgramCounter(AddressSize);
            programCounter.SetInputCE(true);
            programCounter.ProbeState().ToByte().Should().Be(15);
            programCounter.Clock();
            programCounter.ProbeState().ToByte().Should().Be(0);
        }

        [Test]
        public void SetInputCE_ShouldNotIncrementCount_WhenSetToFalse_UponClockCalled()
        {
            var programCounter = new ProgramCounter(AddressSize);
            programCounter.SetInputCE(false);
            programCounter.ProbeState().ToByte().Should().Be(15);
            programCounter.Clock();
            programCounter.ProbeState().ToByte().Should().Be(15);
        }

        [Test]
        public void SetInputL_ShouldSetCount_WhenSetToTrue_UponClockCalled()
        {
            var programCounter = new ProgramCounter(AddressSize);
            programCounter.ProbeState().ToByte().Should().Be(15);
            programCounter.SetInputL(true);
            programCounter.SetInputD(new BitArray((byte)5));
            programCounter.ProbeState().ToByte().Should().Be(15);
            programCounter.Clock();
            programCounter.ProbeState().ToByte().Should().Be(5);
        }

        [Test]
        public void SetInputL_ShouldNotSetCount_WhenSetToFalse_UponClockCalled()
        {
            var programCounter = new ProgramCounter(AddressSize);
            programCounter.ProbeState().ToByte().Should().Be(15);
            programCounter.SetInputL(false);
            programCounter.SetInputD(new BitArray((byte)5));
            programCounter.ProbeState().ToByte().Should().Be(15);
            programCounter.Clock();
            programCounter.ProbeState().ToByte().Should().Be(15);
        }

        [Test]
        public void Clock_ShouldUpdateAddressRegisterState_UsingFirst4BitsOfDataOnly_WhenSetInputLAIsTrue_AndSetInputIsCalledWithArgRepresentingValueGreaterThan15()
        {
            var programCounter = new ProgramCounter(AddressSize);
            programCounter.ProbeState().ToByte().Should().Be(15);
            programCounter.SetInputL(true);
            programCounter.SetInputD(new BitArray((byte)205));
            programCounter.Clock();
            programCounter.ProbeState().Length.Should().Be(4);
            programCounter.ProbeState().ToByte().Should().Be(13); // 13 is the result of truncating binary 205 to 4 bits
            programCounter.ProbeState().ToByte().Should().NotBe(205);
        }


        [Test]
        public void TestIncrement3Times()
        {
            var programCounter = new ProgramCounter(AddressSize);
            programCounter.SetInputE(true);
            programCounter.SetInputCE(true);
            programCounter.Clock();
            programCounter.ProbeState().ToByte().Should().Be(0);
            programCounter.Output!.ToByte().Should().Be(0);
            programCounter.Clock();
            programCounter.ProbeState().ToByte().Should().Be(1);
            programCounter.Output!.ToByte().Should().Be(1);
            programCounter.Clock();
            programCounter.ProbeState().ToByte().Should().Be(2);
            programCounter.Output!.ToByte().Should().Be(2);
        }

        [Test]
        public void Output_ShouldNotBeNull_WhenOutputIsEnabled()
        {
            var programCounter = new ProgramCounter(AddressSize);
            programCounter.Output.Should().BeNull();
            programCounter.SetInputE(true);
            programCounter.Output.Should().NotBeNull();
            programCounter.Output!.ToByte().Should().Be(15);
        }
    }
}
