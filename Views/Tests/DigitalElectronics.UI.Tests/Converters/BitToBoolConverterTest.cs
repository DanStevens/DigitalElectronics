using System.Globalization;
using DigitalElectronics.Concepts;
using DigitalElectronics.UI.Converters;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.UI.Tests.Converters;

public class BitToBoolConverterTest
{
    [Test]
    public void Convert_ShouldReturnFalse_WhenGivenFalseBit()
    {
        var objUT = new BitToBoolConverter();
        var result = objUT.Convert(new Bit(false), typeof(bool), null, CultureInfo.InvariantCulture);
        result.Should().BeOfType<bool>();
        result.Should().Be(false);
    }

    [Test]
    public void Convert_ShouldReturnTrue_WhenGivenTrueBit()
    {
        var objUT = new BitToBoolConverter();
        var result = objUT.Convert(new Bit(true), typeof(bool), null, CultureInfo.InvariantCulture);
        result.Should().BeOfType<bool>();
        result.Should().Be(true);
    }


    [Test]
    public void Convert_ShouldReturnFalse_WhenGivenNull()
    {
        var objUT = new BitToBoolConverter();
        var result = objUT.Convert(null, typeof(bool), null, CultureInfo.InvariantCulture);
        result.Should().BeOfType<bool>();
        result.Should().Be(false);
    }
}
