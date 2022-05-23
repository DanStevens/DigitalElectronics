using System.Globalization;
using DigitalElectronics.UI.Converters;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.UI.Tests.Converters
{
    public class Tests
    {

        [Test]
        public void Convert_ShouldReturnBoolOfTrue_WhenGivenNullableBoolOfTrue()
        {
            var objUT = new NullableBoolToBoolConverter();
            var result = objUT.Convert((bool?)true, typeof(bool?), null, CultureInfo.InvariantCulture);
            result.Should().BeOfType<bool>();
            result.Should().Be(true);
        }

        [Test]
        public void Convert_ShouldReturnBoolOfFalse_WhenGivenNullableBoolOfFalse()
        {
            var objUT = new NullableBoolToBoolConverter();
            var result = objUT.Convert((bool?)false, typeof(bool?), null, CultureInfo.InvariantCulture);
            result.Should().BeOfType<bool>();
            result.Should().Be(false);
        }

        [Test]
        public void Convert_ShouldReturnBoolOfFalse_WhenGivenNullableBoolOfNull()
        {
            var objUT = new NullableBoolToBoolConverter();
            var result = objUT.Convert((bool?)null, typeof(bool?), null, CultureInfo.InvariantCulture);
            result.Should().BeOfType<bool>();
            result.Should().Be(false);
        }

        [Test]
        public void Convert_ShouldReturnBoolOfTrue_WhenGivenTrue()
        {
            var objUT = new NullableBoolToBoolConverter();
            var result = objUT.Convert(true, typeof(bool), null, CultureInfo.InvariantCulture);
            result.Should().BeOfType<bool>();
            result.Should().Be(true);
        }

        [Test]
        public void Convert_ShouldReturnBoolOfFalse_WhenGivenFalse()
        {
            var objUT = new NullableBoolToBoolConverter();
            var result = objUT.Convert(false, typeof(bool), null, CultureInfo.InvariantCulture);
            result.Should().BeOfType<bool>();
            result.Should().Be(false);
        }
    }
}
