using System.Collections.Generic;
using System.Collections.ObjectModel;
using DigitalElectronics.Concepts;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Utilities;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.ViewModels.Components.Tests;

public class SevenSegmentDigitViewModelTests
{
    [Test]
    public void HexDigitDemoValue_WhenObjectUnderTestIsCreated()
    {
        var objUT = new SevenSegmentDigitDemoViewModel();
        objUT.HexDigitDemo.Value.Should().BeOfType<FullyObservableCollection<Bit>>();
        objUT.HexDigitDemo.Value.Count.Should().Be(4);
    }

    [Test]
    public void HexDigitDemoSegmentLines_WhenObjectUnderTestIsCreated()
    {
        var objUT = new SevenSegmentDigitDemoViewModel();
        objUT.HexDigitDemo.SegmentLines.Should().BeAssignableTo<ICollection<bool>>();
        objUT.HexDigitDemo.SegmentLines.Count.Should().Be(8);
        objUT.HexDigitDemo.SegmentLines.ToBitArray().ToByte().Should().Be(0x3F);
    }
}
