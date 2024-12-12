using System.Collections.Generic;
using System.Collections.ObjectModel;
using DigitalElectronics.Concepts;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Utilities;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DigitalElectronics.ViewModels.Modules.Tests
{
    public class FourDigit7SegmentDisplayViewModelTests
    {
        [Test]
        public void Creation()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            objUT.Should().NotBeNull();
        }

        [Test]
        public void InitialState()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();

            objUT.Value.Should().BeOfType<FullyObservableCollection<Bit>>();
            objUT.Value.Count.Should().Be(8);
            objUT.Value.ToBitArray().ToByte().Should().Be(0);

            // All segments of all digits should be lit in initial state
            objUT.LinesForDigit0.Should().BeAssignableTo<IList<bool>>();
            ToByte(objUT.LinesForDigit0).Should().Be(0xFF);
            objUT.LinesForDigit1.Should().BeAssignableTo<IList<bool>>();
            ToByte(objUT.LinesForDigit1).Should().Be(0xFF);
            objUT.LinesForDigit2.Should().BeAssignableTo<IList<bool>>();
            ToByte(objUT.LinesForDigit2).Should().Be(0xFF);
            objUT.LinesForDigit3.Should().BeAssignableTo<IList<bool>>();
            ToByte(objUT.LinesForDigit3).Should().Be(0xFF);
        }

        [Test]
        public void LinesForDigit0_ShouldUpdateEveryFirstClock()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            ToByte(objUT.LinesForDigit0).Should().Be(0xFF);
            objUT.Clock();
            ToByte(objUT.LinesForDigit0).Should().Be(0x3F);
            objUT.Clock();
            ToByte(objUT.LinesForDigit0).Should().Be(0x3F);
            objUT.Clock();
            ToByte(objUT.LinesForDigit0).Should().Be(0x3F);
            objUT.Clock();
            ToByte(objUT.LinesForDigit0).Should().Be(0x3F);
            objUT.Clock();
            ToByte(objUT.LinesForDigit0).Should().Be(0x3F);
        }

        [Test]
        public void LinesForDigit1_ShouldUpdateEverySecondClock()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            ToByte(objUT.LinesForDigit1).Should().Be(0xFF);
            objUT.Clock();
            ToByte(objUT.LinesForDigit1).Should().Be(0xFF);
            objUT.Clock();
            ToByte(objUT.LinesForDigit1).Should().Be(0x3F);
            objUT.Clock();
            ToByte(objUT.LinesForDigit1).Should().Be(0x3F);
            objUT.Clock();
            ToByte(objUT.LinesForDigit1).Should().Be(0x3F);
            objUT.Clock();
            ToByte(objUT.LinesForDigit1).Should().Be(0x3F);
        }

        [Test]
        public void LinesForDigit2_ShouldUpdateEveryThirdClock()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            ToByte(objUT.LinesForDigit2).Should().Be(0xFF);
            objUT.Clock();
            ToByte(objUT.LinesForDigit2).Should().Be(0xFF);
            objUT.Clock();
            ToByte(objUT.LinesForDigit2).Should().Be(0xFF);
            objUT.Clock();
            ToByte(objUT.LinesForDigit2).Should().Be(0x3F);
            objUT.Clock();
            ToByte(objUT.LinesForDigit2).Should().Be(0x3F);
            objUT.Clock();
            ToByte(objUT.LinesForDigit2).Should().Be(0x3F);
        }

        [Test]
        public void LinesForDigit3_ShouldUpdateEveryFourthClock()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            ToByte(objUT.LinesForDigit3).Should().Be(0xFF);
            objUT.Clock();
            ToByte(objUT.LinesForDigit3).Should().Be(0xFF);
            objUT.Clock();
            ToByte(objUT.LinesForDigit3).Should().Be(0xFF);
            objUT.Clock();
            ToByte(objUT.LinesForDigit3).Should().Be(0xFF);
            objUT.Clock();
            ToByte(objUT.LinesForDigit3).Should().Be(0x0);
            objUT.Clock();
            ToByte(objUT.LinesForDigit3).Should().Be(0x0);
        }

        [Test]
        public void Clock_ShouldUpdateLinesToTheNextDecimalDigit()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            objUT.Value[7].Value = true; // Sets Value to 128
            objUT.Clock();
            ToByte(objUT.LinesForDigit0).Should().Be(0x7F); // '8' digit
            objUT.Clock();
            ToByte(objUT.LinesForDigit1).Should().Be(0x5B); // '2' digit
            objUT.Clock();
            ToByte(objUT.LinesForDigit2).Should().Be(0x06); // '1' digit
            objUT.Clock();
            ToByte(objUT.LinesForDigit3).Should().Be(0x0); // blank digit
            objUT.Clock();
            ToByte(objUT.LinesForDigit0).Should().Be(0x7F); // '8' digit
        }

        [Test]
        public void PropertyChanged_EventShouldBeRaisedForLinesProperty_WhenClockIsCalled()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            objUT.Value[0].Value = true; // Sets Value to 128
            using var monitor = objUT.Monitor();
            objUT.Clock();
            monitor.Should().RaisePropertyChangeFor(_ => _.LinesForDigit0);
        }

        [Ignore("To review")]
        public void PropertyChanged_EventShouldBeRaisedForDigitIsActiveProperties_WhenClockIsCalled()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            using var monitor = objUT.Monitor();
            objUT.Clock();
            //monitor.Should().RaisePropertyChangeFor(_ => _.Digit0IsActive);
            //monitor.Should().RaisePropertyChangeFor(_ => _.Digit1IsActive);
            //monitor.Should().RaisePropertyChangeFor(_ => _.Digit2IsActive);
            //monitor.Should().RaisePropertyChangeFor(_ => _.Digit3IsActive);
        }

        private static byte ToByte(IList<bool> lines) => new BitArray(lines).ToByte();
    }
}
