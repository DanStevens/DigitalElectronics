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

            objUT.Lines.Should().BeAssignableTo<IList<bool>>();
            objUT.Lines.ToBitArray().ToByte().Should().Be(0x3F); // '0' digit
        }

        [Test]
        public void Lines_ShouldReturnTheCurrentDecimalDigitBasedOnValue()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            objUT.Value[0].Value = true;                         // Sets Value to 1
            objUT.Lines.ToBitArray().ToByte().Should().Be(0x06); // '1' digit
            objUT.Value[1].Value = true; ;                       // Sets Value to 3
            objUT.Lines.ToBitArray().ToByte().Should().Be(0x4F); // '3' digit
            objUT.Value[2].Value = true;                         // Sets Value to 7
            objUT.Lines.ToBitArray().ToByte().Should().Be(0x07); // '7' digit
            objUT.Value[3].Value = true;                         // Sets Value to 15
            objUT.Lines.ToBitArray().ToByte().Should().Be(0x6D); // '5' digit
            objUT.Value[4].Value = true;                         // Sets Value to 31
            objUT.Lines.ToBitArray().ToByte().Should().Be(0x06); // '1' digit
        }

        [Test]
        public void Clock_ShouldUpdateLinesToTheNextDecimalDigit()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            objUT.Value[7].Value = true; // Sets Value to 128
            objUT.Lines.ToBitArray().ToByte().Should().Be(0x7F); // '8' digit
            objUT.Clock();
            objUT.Lines.ToBitArray().ToByte().Should().Be(0x5B); // '2' digit
            objUT.Clock();
            objUT.Lines.ToBitArray().ToByte().Should().Be(0x06); // '1' digit
            objUT.Clock();
            objUT.Lines.ToBitArray().ToByte().Should().Be(0x0); // blank digit
            objUT.Clock();
            objUT.Lines.ToBitArray().ToByte().Should().Be(0x7F); // '8' digit
        }

        [Test]
        public void PropertyChanged_EventShouldBeRaisedForLinesProperty_WhenValueBitIsChanged()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            using var monitor = objUT.Monitor();
            objUT.Value[0].Value = true;
            monitor.Should().RaisePropertyChangeFor(_ => _.Lines);
        }

        [Test]
        public void PropertyChanged_EventShouldBeRaisedForLinesProperty_WhenClockIsCalled()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            objUT.Value[0].Value = true; // Sets Value to 128
            using var monitor = objUT.Monitor();
            objUT.Clock();
            monitor.Should().RaisePropertyChangeFor(_ => _.Lines);
        }

        [Test]
        public void SetValueTo11AndObserveDisplayAndPropertyChangedEvents()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            SetValueTo11();
            objUT.Value.ToBitArray().ToByte().Should().Be(11);
            objUT.Lines.ToBitArray().ToByte().Should().Be(0x06);

            using var monitor1 = objUT.Monitor();
            {
                objUT.Clock();
                objUT.Lines.ToBitArray().ToByte().Should().Be(0x06);
                monitor1.Should().RaisePropertyChangeFor(_ => _.Lines);
            }

            using var monitor2 = objUT.Monitor();
            {
                objUT.Clock();
                objUT.Lines.ToBitArray().ToByte().Should().Be(0x3F);
                monitor2.Should().RaisePropertyChangeFor(_ => _.Lines);
            }

            using var monitor3 = objUT.Monitor();
            {
                objUT.Clock();
                objUT.Lines.ToBitArray().ToByte().Should().Be(0x0);
                monitor3.Should().RaisePropertyChangeFor(_ => _.Lines);
            }

            using var monitor4 = objUT.Monitor();
            {
                objUT.Clock();
                objUT.Lines.ToBitArray().ToByte().Should().Be(0x06);
                monitor4.Should().RaisePropertyChangeFor(_ => _.Lines);
            }

            void SetValueTo11()
            {
                objUT.Value[0].Value = true;
                objUT.Value[1].Value = true;
                objUT.Value[3].Value = true;
            }
        }
    }
}
