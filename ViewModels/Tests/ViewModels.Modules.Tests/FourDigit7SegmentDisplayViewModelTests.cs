using System.Collections.Generic;
using System.Collections.ObjectModel;
using DigitalElectronics.Concepts;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Utilities;
using FluentAssertions;
using NUnit.Framework;

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
        public void Lines()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            using var monitor = objUT.Monitor();
            objUT.Value[0].Value = true;
            objUT.Lines.ToBitArray().ToByte().Should().Be(0x06); // '1' digit 
            monitor.Should().RaisePropertyChangeFor(_ => _.Lines);
        }
    }
}
