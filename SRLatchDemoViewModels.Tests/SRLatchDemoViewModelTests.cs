using System;
using DigitalElectronics.Components.FlipFlops;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace SRLatchDemoViewModels.Tests
{
    public class SRLatchDemoViewModelTests
    {
        #region Helper methods

        private static SRLatchDemoViewModel CreateObjectUnderTest()
        {
            return CreateObjectUnderTest(Substitute.For<ISRLatch>());
        }
        private static SRLatchDemoViewModel CreateObjectUnderTest(ISRLatch srLatchMock)
        {
            return new SRLatchDemoViewModel(srLatchMock);
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
            new SRLatchDemoViewModel();
        }

        [Test]
        public void Ctor_ShouldAcceptSRLatchParameter()
        {
            CreateObjectUnderTest();
        }

        [Test]
        public void Ctor_ShouldThrowArgumentNullException_WhenSRLatchParameterIsNull()
        {
            var ex=  Assert.Throws<ArgumentNullException>(() => new SRLatchDemoViewModel(null));
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
            objUT.PropertyChanged += (s, e) => raised |= true;

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
            var srLatchMock = Substitute.For<ISRLatch>();
            var objUT = CreateObjectUnderTest(srLatchMock);

            objUT.Reset = true;
            srLatchMock.Received(1).SetInputR(true);

            objUT.Reset = false;
            srLatchMock.Received(1).SetInputR(false);
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
            var srLatchMock = Substitute.For<ISRLatch>();
            var objUT = CreateObjectUnderTest(srLatchMock);

            objUT.Set = true;
            srLatchMock.Received(1).SetInputS(true);

            objUT.Set = false;
            srLatchMock.Received(1).SetInputS(false);
        }

        [Test]
        public void OutputQ_ShouldBeFalse_WhenObjectUnderTestCreated()
        {
            var objUT = CreateObjectUnderTest();
            objUT.OutputQ.Should().Be(false);
        }

        [Test]
        public void OutputQ_ShouldMatchOutputQOnSRLatch()
        {
            var srLatchMock = Substitute.For<ISRLatch>();
            srLatchMock.OutputQ.Returns(true);
            var objUT = CreateObjectUnderTest(srLatchMock);
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
            var srLatchMock = Substitute.For<ISRLatch>();
            srLatchMock.OutputNQ.Returns(true);
            var objUT = CreateObjectUnderTest(srLatchMock);
            objUT.OutputNQ.Should().Be(true);
        }


        #endregion
    }
}
