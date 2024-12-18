using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using DigitalElectronics.Concepts;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Utilities;
using FluentAssertions;
using NUnit.Framework;
using static DigitalElectronics.ViewModels.Components.SevenSegmentDigitDemoViewModel;

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
        ToByte(objUT.HexDigitDemo.SegmentLines).Should().Be(0x3F);
    }

    [Test]
    public void HexDigitDemoSegmentLines_ShouldShowDigit1()
    {
        var objUT = new SevenSegmentDigitDemoViewModel();
        objUT.HexDigitDemo.Value[0].Value = true; // Sets value to 1
        ToByte(objUT.HexDigitDemo.SegmentLines).Should().Be(0x06);
    }

    [Test]
    public void HexDigitDemoSegmentLines_ShouldShowDigit8()
    {
        var objUT = new SevenSegmentDigitDemoViewModel();
        objUT.HexDigitDemo.Value[3].Value = true; // Sets value to 8
        ToByte(objUT.HexDigitDemo.SegmentLines).Should().Be(0x7F);
    }

    [Test]
    public void HexDigitDemoSegmentLines_ShouldShowDigitF()
    {
        var objUT = new SevenSegmentDigitDemoViewModel();
        objUT.HexDigitDemo.Value[0].Value = true; // Set value to 1 (0x1)
        objUT.HexDigitDemo.Value[1].Value = true; // Set value to 3 (0x3)
        objUT.HexDigitDemo.Value[2].Value = true; // Set value to 7 (0x7)
        objUT.HexDigitDemo.Value[3].Value = true; // Set value to 15 (0xF)
        ToByte(objUT.HexDigitDemo.SegmentLines).Should().Be(0x71);
    }

    [Test]
    public void HexDigitWithRegisterDemo_ShouldBeOfTypeSingleHexDigitWithRegisterDemoViewModel()
    {
        var objUT = new SevenSegmentDigitDemoViewModel();
        objUT.HexDigitWithRegisterDemo.Should().BeOfType<SingleHexDigitWithRegisterDemoViewModel>();
    }

    private static byte ToByte(ICollection<bool> lines) => new BitArray(lines).ToByte();
}

public class SingleHexDigitWithRegisterDemoViewModelTests
{
    [Test]
    public void RegisterData_ShouldBeZero_WhenObjectUnderTestIsCreated()
    {
        var objUT = new SingleHexDigitWithRegisterDemoViewModel();
        objUT.Load.Should().Be(true);
        BitArray.FromList(objUT.Value).ToByte().Should().Be(0);
    }

    [Test]
    public void SegmentLines_ShouldBeInitializedTo0x3F_WhenObjectUnderTestIsCreated()
    {
        var objUT = new SingleHexDigitWithRegisterDemoViewModel();
        objUT.SegmentLines.Should().BeAssignableTo<ICollection<bool>>();
        objUT.SegmentLines.Count.Should().Be(8);
        ToByte(objUT.SegmentLines).Should().Be(0x3F);
    }

    [Test]
    public void SegmentLines_ShouldNotChangeWhenChangingValue()
    {
        var objUT = new SingleHexDigitWithRegisterDemoViewModel();
        objUT.Load.Should().Be(true);
        objUT.Value[0].Value = true; // Sets Value to 1
        ToByte(objUT.SegmentLines).Should().Be(0x3F);
    }

    [Test]
    public void SegmentLines_ShouldShowDigit1_WhenClockIsCalled()
    {
        var objUT = new SingleHexDigitWithRegisterDemoViewModel();
        objUT.Load.Should().Be(true);
        objUT.Value[0].Value = true; // Sets Value to 1
        objUT.Clock();
        ToByte(objUT.SegmentLines).Should().Be(0x06);
    }

    private static byte ToByte(ICollection<bool> lines) => new BitArray(lines).ToByte();
}
