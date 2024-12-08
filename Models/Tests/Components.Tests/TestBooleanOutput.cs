using System;
using System.Linq;
using DigitalElectronics.Components.FlipFlops;
using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using NUnit.Framework;

namespace DigitalElectronics.Components.Tests;

public class TestBooleanOutput
{
    [Test]
    [TestCase(new bool[] { }, 0)]
    [TestCase(new bool[] { false }, 0)]
    [TestCase(new bool[] { true }, 1)]
    [TestCase(new bool[] { true, false }, 1)]
    [TestCase(new bool[] { true, true }, 3)]
    [TestCase(new bool[] { false, false, false, false }, 0)]
    [TestCase(new bool[] { true, false, false, false }, 1)]
    [TestCase(new bool[] { false, true, false, false }, 2)]
    [TestCase(new bool[] { false, false, true, false }, 4)]
    [TestCase(new bool[] { false, false, false, true }, 8)]
    [TestCase(new bool[] { false, true, true, true }, 14)]
    public void ToBitArray_ShouldCreateBitArrayFromArrayOfBooleanOutputComponents(bool[] componentOutputs, int expected)
    {
        using (new AssertionScope())
        {
            var components = componentOutputs.Select(CreateMockComponent).ToArray();
            var result = components.ToBitArray();
            result.ToInt32().Should().Be(expected);
            result.Length.Should().Be(componentOutputs.Length); 
        }
    }

    [Test]
    public void ToBitArray_ShouldThrowWhenArgHasMoreThanThan32Items()
    {
        using (new AssertionScope())
        {
            var components = Enumerable.Repeat(false, 33).Select(CreateMockComponent).ToArray();
            var ex = Assert.Throws<ArgumentException>(() => components.ToBitArray());
            ex.ParamName.Should().Be("components");
            ex.Message.Should().StartWith("Argument cannot contain more than 32 items.");
        }
    }

    private static IBooleanOutput CreateMockComponent(bool output)
    {
        var mock = Substitute.For<IBooleanOutput>();
        mock.Output.Returns(output);
        return mock;
    }
}
