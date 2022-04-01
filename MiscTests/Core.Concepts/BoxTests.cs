using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Concepts.Tests
{

    public class BoxTests
    {
        [Test]
        public void CreateBoxContainingInt()
        {
            var box42 = new Box<int>(42);
            box42.Value.Should().Be(42);
        }

        [Test]
        public void BoxOfInt_ShouldBeImplicitlyCastableToInt()
        {
            int i42 = new Box<int>(42);
            i42.Should().Be(42);
        }

        [Test]
        public void BoxOfInt_ShouldBeExplicitlyCastableToInt()
        {
            var i42 = (int)new Box<int>(42);
            i42.Should().Be(42);
        }

        [Test]
        public void BoxOfInt_ShouldBeCreatableByImplicitCastFromInt()
        {
            Box<int> box42 = 42;
            box42.Value.Should().Be(42);
        }

        [Test]
        public void BoxOfInt_ShouldBeCreatableByExplicitCastFromInt()
        {
            var box42 = (Box<int>)42;
            box42.Value.Should().Be(42);
        }

        [Test]
        public void BoxOfInt_ShouldBeComparableWithInt()
        {
            var box42 = new Box<int>(42);
            box42.CompareTo(42).Should().Be(0);
            box42.CompareTo(41).Should().Be(1);
            box42.CompareTo(43).Should().Be(-1);
        }

        [Test]
        public void BoxOfDefaultIntValue_ShouldTBD_WhenComparedToNull()
        {
            var boxDefault = new Box<int>(default);
            boxDefault.Value.Should().Be(0);
            boxDefault.CompareTo(null).Should().Be(1); // Or should it be 0?
        }

        [Test]
        public void BoxOfInt_ShouldBeComparableWithOtherBoxOfInt()
        {
            var box42 = new Box<int>(42);
            box42.CompareTo(new Box<int>(42)).Should().Be(0);
            box42.CompareTo(new Box<int>(41)).Should().Be(1);
            box42.CompareTo(new Box<int>(43)).Should().Be(-1);
        }

        [Test]
        public void BoxOfInt_ShouldEqualsInt()
        {
            var box42 = new Box<int>(42);
            box42.Equals(42).Should().Be(true);
            box42.Equals(41).Should().Be(false);
            box42.Equals(43).Should().Be(false);
        }

        [Test]
        public void BoxOfInt_ShouldEqualsOtherBoxOfInt()
        {
            var box42 = new Box<int>(42);
            box42.Equals(new Box<int>(42)).Should().Be(true);
            box42.Equals(new Box<int>(41)).Should().Be(false);
            box42.Equals(new Box<int>(43)).Should().Be(false);
        }
    }
}
