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

    [Test]
    public void HexDigitDemoSegmentLines_ShouldShowDigit1()
    {
        var objUT = new SevenSegmentDigitDemoViewModel();
        objUT.HexDigitDemo.Value[0].Value = true; // Sets value to 1
        objUT.HexDigitDemo.SegmentLines.ToBitArray().ToByte().Should().Be(0x06);
    }


    [Test]
    public void HexDigitDemoSegmentLines_ShouldShowDigit8()
    {
        var objUT = new SevenSegmentDigitDemoViewModel();
        objUT.HexDigitDemo.Value[3].Value = true; // Sets value to 8
        objUT.HexDigitDemo.SegmentLines.ToBitArray().ToByte().Should().Be(0x7F);
    }

    [Test]
    public void HexDigitDemoSegmentLines_ShouldShowDigitF()
    {
        var objUT = new SevenSegmentDigitDemoViewModel();
        objUT.HexDigitDemo.Value[0].Value = true; // Set value to 1 (0x1)
        objUT.HexDigitDemo.Value[1].Value = true; // Set value to 3 (0x3)
        objUT.HexDigitDemo.Value[2].Value = true; // Set value to 7 (0x7)
        objUT.HexDigitDemo.Value[3].Value = true; // Set value to 15 (0xF)
        objUT.HexDigitDemo.SegmentLines.ToBitArray().ToByte().Should().Be(0x71);
    }
}
