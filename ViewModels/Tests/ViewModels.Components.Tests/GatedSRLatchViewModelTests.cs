using System;
using DigitalElectronics.Components.FlipFlops;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace DigitalElectronics.ViewModels.Components.Tests
{
    public class GatedSRLatchViewModelTests
    {
        #region Helper methods

        private static GatedSRLatchViewModel CreateObjectUnderTest()
        {
            return new GatedSRLatchViewModel();
        }
        private static GatedSRLatchViewModel CreateObjectUnderTest(IGatedSRLatch srLatchMock)
        {
            return new GatedSRLatchViewModel(srLatchMock);
        }


        #endregion

        [SetUp]
        public void Setup()
        {
        }

        #region Constructor tests

        [Test]
        public void Ctor_ShouldAcceptNoParameters()
        {
            new GatedSRLatchViewModel();
        }

        [Test]
        public void Ctor_ShouldAcceptSRLatchParameter()
        {
            CreateObjectUnderTest();
        }

        [Test]
        public void Ctor_ShouldThrowArgumentNullException_WhenSRLatchParameterIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new SRLatchViewModel(null));
            ex.ParamName.Should().Be("srLatch");
        }

        #endregion

        #region Property tests

        [Test]
        public void Reset_ShouldBeFalse_WhenObjectUnderTestCreated()
        {
            var objUT = CreateObjectUnderTest();
            objUT.Reset.Should().Be(false);
        }

        [Test]
        public void Reset_ShouldPropertyChangedNotification_WhenChanged()
        {
            bool raised = false;
            var objUT = CreateObjectUnderTest();
            objUT.PropertyChanged += (s, e) => raised = true;

            objUT.Reset = false;
            objUT.Reset.Should().Be(false);
            raised.Should().Be(false);

            objUT.Reset = true;
            objUT.Reset.Should().Be(true);
            raised.Should().Be(true);
            raised = false;

            objUT.Reset = true;
            objUT.Reset.Should().Be(true);
            raised.Should().Be(false);

            objUT.Reset = false;
            objUT.Reset.Should().Be(false);
            raised.Should().Be(true);
        }

        [Test]
        public void Reset_WhenSet_ShouldSetInputROnSRLatchToSameValue()
        {
            var gatedSRLatchMock = Substitute.For<GatedSRLatch>();
            var objUT = CreateObjectUnderTest(gatedSRLatchMock);

            objUT.Reset = true;
            gatedSRLatchMock.Received(1).SetInputR(true);

            objUT.Reset = false;
            gatedSRLatchMock.Received(1).SetInputR(false);
        }

        [Test]
        public void Set_ShouldBeFalse_WhenObjectUnderTestCreated()
        {
            var objUT = CreateObjectUnderTest();
            objUT.Set.Should().Be(false);
        }

        [Test]
        public void Set_ShouldPropertyChangedNotification_WhenChanged()
        {
            bool raised = false;
            var objUT = CreateObjectUnderTest();
            objUT.PropertyChanged += (s, e) => raised |= true;

            objUT.Set = false;
            objUT.Set.Should().Be(false);
            raised.Should().Be(false);

            objUT.Set = true;
            objUT.Set.Should().Be(true);
            raised.Should().Be(true);
            raised = false;

            objUT.Set = true;
            objUT.Set.Should().Be(true);
            raised.Should().Be(false);

            objUT.Set = false;
            objUT.Set.Should().Be(false);
            raised.Should().Be(true);
        }

        [Test]
        public void Set_WhenSet_ShouldSetInputROnSRLatchToSameValue()
        {
            var gatedSRLatchMock = Substitute.For<GatedSRLatch>();
            var objUT = CreateObjectUnderTest(gatedSRLatchMock);

            objUT.Set = true;
            gatedSRLatchMock.Received(1).SetInputS(true);

            objUT.Set = false;
            gatedSRLatchMock.Received(1).SetInputS(false);
        }

        [Test]
        public void Enable_ShouldBeFalse_WhenObjectUnderTestCreated()
        {
            var objUT = CreateObjectUnderTest();
            objUT.Enable.Should().Be(false);
        }

        [Test]
        public void Enable_ShouldPropertyChangedNotification_WhenChanged()
        {
            bool raised = false;
            var objUT = CreateObjectUnderTest();
            objUT.PropertyChanged += (s, e) => raised |= true;

            objUT.Enable = false;
            objUT.Enable.Should().Be(false);
            raised.Should().Be(false);

            objUT.Enable = true;
            objUT.Enable.Should().Be(true);
            raised.Should().Be(true);
            raised = false;

            objUT.Enable = true;
            objUT.Enable.Should().Be(true);
            raised.Should().Be(false);

            objUT.Enable = false;
            objUT.Enable.Should().Be(false);
            raised.Should().Be(true);
        }

        [Test]
        public void Enable_WhenTrue_SRLatchStateShouldBeChangeable()
        {
            var objUT = CreateObjectUnderTest();

            objUT.OutputQ.Should().Be(true);
            objUT.OutputNQ.Should().Be(false);

            objUT.Enable.Should().Be(false);
            objUT.Reset = true;
            objUT.Reset = false;
            objUT.OutputQ.Should().Be(true);
            objUT.OutputNQ.Should().Be(false);

            objUT.Enable = true;
            objUT.Reset = true;
            objUT.Reset = false;
            objUT.OutputQ.Should().Be(false);
            objUT.OutputNQ.Should().Be(true);

            objUT.Enable = false;
            objUT.Set = true;
            objUT.Set = false;
            objUT.OutputQ.Should().Be(false);
            objUT.OutputNQ.Should().Be(true);

            objUT.Enable = true;
            objUT.Set = true;
            objUT.Set = false;
            objUT.OutputQ.Should().Be(true);
            objUT.OutputNQ.Should().Be(false);
        }

        [Test]
        public void OutputQ_ShouldBeFalse_WhenObjectUnderTestCreated()
        {
            var objUT = CreateObjectUnderTest();
            objUT.OutputQ.Should().Be(true);
        }

        [Test]
        public void OutputQ_ShouldMatchOutputQOnSRLatch()
        {
            var gatedSRLatchMock = Substitute.For<IGatedSRLatch>();
            gatedSRLatchMock.OutputQ.Returns(true);
            var objUT = CreateObjectUnderTest(gatedSRLatchMock);
            objUT.OutputQ.Should().Be(true);
        }

        [Test]
        public void OutputNQ_ShouldBeFalse_WhenObjectUnderTestCreated()
        {
            var objUT = CreateObjectUnderTest();
            objUT.OutputNQ.Should().Be(false);
        }

        [Test]
        public void OutputNQ_ShouldMatchOutputQOnSRLatch()
        {
            var gatedSRLatchMock = Substitute.For<IGatedSRLatch>();
            gatedSRLatchMock.OutputNQ.Returns(true);
            var objUT = CreateObjectUnderTest(gatedSRLatchMock);
            objUT.OutputNQ.Should().Be(true);
        }


        #endregion
    }
}
