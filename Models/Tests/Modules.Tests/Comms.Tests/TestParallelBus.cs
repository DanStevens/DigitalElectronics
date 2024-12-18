using System;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Comms;
using DigitalElectronics.Modules.Counters;
using DigitalElectronics.Utilities;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

#nullable enable

namespace DigitalElectronics.Modules.Tests.Comms.Tests
{
    public class TestParallelBus
    {
        [Test]
        public void Constructor_AcceptsAModuleAsArgument()
        {
            new ParallelBus(8, Substitute.For<IModule>());
        }

        [Test]
        public void Constructor_AcceptsAnOutputModuleAsArgument()
        {
            new ParallelBus(8, Substitute.For<IOutputModule>());
        }

        [Test]
        public void Constructor_AcceptsMultipleModulesAsArguments()
        {
            new ParallelBus(8,
                Substitute.For<IModule>(),
                Substitute.For<IModule>(),
                Substitute.For<IOutputModule>());
        }

        [Test]
        public void Constructor_GivenZeroNumberOfBits_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ParallelBus(0, Substitute.For<IModule>()));
        }

        [Test]
        public void Constructor_GivenNegativeNumberOfBits_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ParallelBus(-1, Substitute.For<IModule>()));
        }

        [Test]
        public void NumberOfChannels_ShouldEqualCorrespondingConstructorArg()
        {
            var bus = new ParallelBus(8, Substitute.For<IModule>());
            bus.NumberOfChannels.Should().Be(8);
        }

        [Test]
        public void Output_ShouldBeNull_WhenModuleIsNotEnabledForOutput()
        {
            var module = Substitute.For<IOutputModule>();
            module.SetInputE(false);
            module.Output.Should().BeNull();
            var bus = new ParallelBus(8, module);
            bus.Output.Should().BeNull();
        }

        [Test]
        public void Output_ShouldEqualOutputOfEnabledModule_WhenOnlyOneModuleIsEnabledForOutput()
        {
            var module1 = Substitute.For<IOutputModule>();
            var module2 = Substitute.For<IOutputModule>();
            var module3 = Substitute.For<IOutputModule>();
            var bus = new ParallelBus(8, module1, module2, module3);
            module2.Output.Returns(new BitArray((byte)2));
            bus.Output!.Value.ToByte().Should().Be(2);
        }

        [Test]
        public void Output_ShouldThrow_WhenMoreThanOneModuleIsEnabledForOutput()
        {
            var module1 = Substitute.For<IOutputModule>();
            var module2 = Substitute.For<IOutputModule>();
            var module3 = Substitute.For<IOutputModule>();
            var bus = new ParallelBus(8, module1, module2, module3);
            module2.Output.Returns(new BitArray((byte)2));
            module3.Output.Returns(new BitArray((byte)3));
            Assert.Throws<BusContentionException>(() => _ = bus.Output);
        }

        [Test]
        public void Output_ShouldReturnBitArrayOfLengthEqualToNumberOfChannels_WhenModulesOutputIsGreaterThanNumberOfChannels()
        {
            var module = Substitute.For<IOutputModule>();
            var bus = new ParallelBus(4, module);
            module.Output.Returns(new BitArray((byte)255));
            bus.Output!.Value.Length.Should().Be(4);
            bus.Output!.Value.ToByte().Should().Be(15);
        }

        [Test]
        public void Output_ShouldReturnBitArrayOfLengthEqualToNumberOfChannels_WhenModulesOutputIsLessThanNumberOfChannels()
        {
            var module = Substitute.For<IOutputModule>();
            var bus = new ParallelBus(8, module);
            var ba = new BitArray(15, length: 4);
            module.Output.Returns(ba);
            bus.Output!.Value.Length.Should().Be(8);
            bus.Output!.Value.ToByte().Should().Be(15);
        }

        [Test]
        public void Transfer_ShouldWriteBusOutputToAllInputModules_WhenAboutIsNotNull()
        {
            var module1 = Substitute.For<IInputModule>();
            var module2 = Substitute.For<IReadWriteRegister>();
            var module3 = Substitute.For<IReadWriteRegister>();
            var module4 = Substitute.For<IProgramCounter>();
            var binary42 = new BitArray((byte)42);
            var bus = new ParallelBus(8, module1, module2, module3, module4);

            module3.Output.Returns(binary42);

            bus.Transfer();

            module1.Received(1).SetInputD(binary42);
            module2.Received(1).SetInputD(binary42);
            module3.Received(1).SetInputD(binary42);
        }

        [Test]
        public void Transfer_ShouldWriteBusOutputToAllInputModules_WhenAboutIsNull()
        {
            var module1 = Substitute.For<IInputModule>();
            var module2 = Substitute.For<IReadWriteRegister>();
            var module3 = Substitute.For<IReadWriteRegister>();
            var module4 = Substitute.For<IProgramCounter>();
            var bus = new ParallelBus(8, module1, module2, module3, module4);

            module3.Output.Should().BeNull();

            bus.Transfer();

            module1.DidNotReceiveWithAnyArgs().SetInputD(default!);
            module2.DidNotReceiveWithAnyArgs().SetInputD(default!);
            module3.DidNotReceiveWithAnyArgs().SetInputD(default!);
        }
    }
}
