using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DigitalElectronics.Concepts;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Modules;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace DigitalElectronics.Boards.Tests
{
    public class BusTransferBoardTests
    {
        private readonly BitConverter _bitConverter = new();

        [Test]
        public void InitialState()
        {
            var registerAMock = Substitute.For<IRegisterViewModel>();
            var registerBMock = Substitute.For<IRegisterViewModel>();
            using var objUT = new BusTransferBoard(registerAMock, registerBMock);
            objUT.RegisterA.Should().BeSameAs(registerAMock);
            objUT.RegisterB.Should().BeSameAs(registerBMock);
            objUT.BusState.Should().BeNull();
        }

        [Test]
        public void Clock_ShouldCallClockOnRegisterAAndRegisterB()
        {
            var registerAMock = Substitute.For<IRegisterViewModel>();
            var registerBMock = Substitute.For<IRegisterViewModel>();
            using var objUT = new BusTransferBoard(registerAMock, registerBMock);

            objUT.Clock();

            registerAMock.Received(1).Clock();
            registerBMock.Received(1).Clock();
        }

        [Test]
        public void Clock_ShouldBusCollisionException_WhenRegisterAAndRegisterBAreBothEnabled()
        {
            var registerAMock = Substitute.For<IRegisterViewModel>();
            var registerBMock = Substitute.For<IRegisterViewModel>();
            using var objUT = new BusTransferBoard(registerAMock, registerBMock);

            registerAMock.Enable = true;
            registerBMock.Enable = true;

            var ex = Assert.Throws<BusContentionException>(() => objUT.Clock());
            ex!.Message.Should().Be("Register A and Register B should not be enabled at the same time.");
        }

        [Test]
        public void BusState_ShouldMatchOutputOfRegisterA_WhenRegisterAIsEnabled()
        {
            var registerAMock = Substitute.For<IRegisterViewModel>();
            var registerBMock = Substitute.For<IRegisterViewModel>();
            using var objUT = new BusTransferBoard(registerAMock, registerBMock);
            registerAMock.Enable.Should().Be(false);
            var binary42 = _bitConverter.GetBits((byte)42);
            registerAMock.Output.Returns((IReadOnlyList<bool>?) binary42.ToArray<bool>());
            objUT.BusState.Should().BeNull();

            registerAMock.Enable = true;
            registerAMock.EnableChanged += Raise.Event();
            objUT.BusState.Should().BeEquivalentTo(registerAMock.Output);

            registerAMock.Enable = false;
            registerAMock.EnableChanged += Raise.Event();
            objUT.BusState.Should().BeNull();
        }

        [Test]
        public void BusState_ShouldMatchOutputOfRegisterB_WhenRegisterBIsEnabled()
        {
            var registerAMock = Substitute.For<IRegisterViewModel>();
            var registerBMock = Substitute.For<IRegisterViewModel>();
            using var objUT = new BusTransferBoard(registerAMock, registerBMock);
            registerBMock.Enable.Should().Be(false);
            var binary42 = _bitConverter.GetBits((byte)42);
            registerBMock.Output.Returns((IReadOnlyList<bool>?) binary42.ToArray<bool>());
            objUT.BusState.Should().BeNull();

            registerBMock.Enable = true;
            registerBMock.EnableChanged += Raise.Event();
            objUT.BusState.Should().BeEquivalentTo(registerBMock.Output);

            registerBMock.Enable = false;
            registerBMock.EnableChanged += Raise.Event();
            objUT.BusState.Should().BeNull();
        }


        [Test]
        public void PropertyChanged_IsRaisedForBusStateProperty_WhenBusStateChanges()
        {
            bool raised = false;
            var registerAMock = Substitute.For<IRegisterViewModel>();
            var registerBMock = Substitute.For<IRegisterViewModel>();
            using var objUT = new BusTransferBoard(registerAMock, registerBMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.BusState);
            registerAMock.Enable.Should().Be(false);
            var binary42 = _bitConverter.GetBits((byte)42);
            registerAMock.Output.Returns((IReadOnlyList<bool>?) binary42.ToArray<bool>());
            objUT.BusState.Should().BeNull();

            registerAMock.Enable = true;
            registerAMock.EnableChanged += Raise.Event();
            raised.Should().Be(true);
            raised = false;

            registerAMock.Enable = true;
            registerAMock.EnableChanged += Raise.Event();
            raised.Should().Be(true);
        }

        [Test]
        public void Load42IntoRegisterA_TransferRegisterAToRegisterB_TransferRegisterBToRegisterA()
        {
            var registerA = new EightBitRegisterViewModel();
            var registerB = new EightBitRegisterViewModel();
            using var objUT = new BusTransferBoard(registerA, registerB);
            var binary42 = _bitConverter.GetBits((byte)42);

            LoadBinary42IntoRegisterA();
            TransferRegisterAToRegisterB();
            ResetRegisterAToZero();
            TransferRegisterBToRegisterA();

            void LoadBinary42IntoRegisterA()
            {
                registerA.Load = true;
                registerA.Data = new ObservableCollection<Bit>(binary42.AsEnumerable<Bit>());
                objUT.Clock();
                registerA.Probe.Should().BeEquivalentTo(binary42.ToArray());
                registerA.Load = false;
            }

            void TransferRegisterAToRegisterB()
            {
                registerA.Enable = true;
                registerB.Load = true;
                objUT.Clock();
                registerB.Load = false;
                registerA.Enable = false;
                registerB.Probe.Should().BeEquivalentTo(binary42.ToArray());
            }

            void ResetRegisterAToZero()
            {
                registerA.Load = true;
                var binary0 = _bitConverter.GetBits((byte) 0);
                var registerAData = new ObservableCollection<Bit>(binary0.AsEnumerable<Bit>());
                registerA.Data = registerAData;
                objUT.Clock();
                registerA.Load = false;
                registerA.Probe.Should().BeEquivalentTo(binary0.ToArray());
                registerB.Probe.Should().BeEquivalentTo(binary42.ToArray());
            }

            void TransferRegisterBToRegisterA()
            {
                registerB.Enable = true;
                registerA.Load = true;
                objUT.Clock();
                registerA.Probe.Should().BeEquivalentTo(binary42.ToArray());
                registerA.Load = false;
                registerB.Enable = false;
            }
        }
    }
}
