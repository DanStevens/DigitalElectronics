using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Counters;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Modules.Tests.Counters.Tests
{
    public class TestProgramCounter
    {
        [Test]
        public void Ctor_ShouldThrowArgumentOutOfRangeException_WhenSizeInBitsArgIsNegative()
        {
            var ex = Assert.Throws<System.ArgumentOutOfRangeException>(() => new ProgramCounter(-1));
            ex!.ParamName.Should().Be("sizeInBits");
            ex.Message.Should().StartWithEquivalentOf("Argument must be greater than 0");
        }

        [Test]
        public void Ctor_ShouldThrowArgumentOutOfRangeException_WhenSizeInBitsArgIsZero()
        {
            var ex = Assert.Throws<System.ArgumentOutOfRangeException>(() => new ProgramCounter(0));
            ex!.ParamName.Should().Be("sizeInBits");
            ex.Message.Should().StartWithEquivalentOf("Argument must be greater than 0");
        }

        [Test]
        public void InitialState()
        {
            var programCounter = new ProgramCounter(4);
            programCounter.SizeInBits.Should().Be(4);
            programCounter.Output.Should().BeNull();
            programCounter.ProbeState().ToByte().Should().Be(15);
        }

        [Test]
        public void Jump()
        {
            var programCounter = new ProgramCounter(4);
            programCounter.Jump(new BitArray((byte)5));
            programCounter.ProbeState().ToByte().Should().Be(5);
        }

        [Test]
        public void Inc()
        {
            var programCounter = new ProgramCounter(4);
            programCounter.SetInputE(true);
            programCounter.Inc();
            programCounter.ProbeState().ToByte().Should().Be(0);
            programCounter.Output!.ToByte().Should().Be(0);
            programCounter.Inc();
            programCounter.ProbeState().ToByte().Should().Be(1);
            programCounter.Output!.ToByte().Should().Be(1);
            programCounter.Inc();
            programCounter.ProbeState().ToByte().Should().Be(2);
            programCounter.Output!.ToByte().Should().Be(2);
        }

        [Test]
        public void Output_ShouldNotBeNull_WhenOutputIsEnabled()
        {
            var programCounter = new ProgramCounter(4);
            programCounter.Output.Should().BeNull();
            programCounter.SetInputE(true);
            programCounter.Output.Should().NotBeNull();
            programCounter.Output!.ToByte().Should().Be(15);
        }
    }
}
