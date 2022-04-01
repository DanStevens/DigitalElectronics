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
            new Box<int>(42).CompareTo(42).Should().Be(0);
            new Box<int>(42).CompareTo(41).Should().Be(1);
            new Box<int>(42).CompareTo(43).Should().Be(-1);
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
            new Box<int>(42).CompareTo(new Box<int>(42)).Should().Be(0);
            new Box<int>(42).CompareTo(new Box<int>(41)).Should().Be(1);
            new Box<int>(42).CompareTo(new Box<int>(43)).Should().Be(-1);
        }

        [Test]
        public void BoxOfInt_ShouldEqualsInt()
        {
            new Box<int>(42).Equals(42).Should().Be(true);
            new Box<int>(42).Equals(41).Should().Be(false);
            new Box<int>(42).Equals(43).Should().Be(false);
        }

        [Test]
        public void BoxOfInt_ShouldEqualsOtherBoxOfInt()
        {
            new Box<int>(42).Equals(new Box<int>(42)).Should().Be(true);
            new Box<int>(42).Equals(new Box<int>(41)).Should().Be(false);
            new Box<int>(42).Equals(new Box<int>(43)).Should().Be(false);
        }

        [Test]
        public void BoxOfInt_ShouldEqualsOtherBoxOfIntUsingObjectEquals()
        {
            Equals(new Box<int>(42), new Box<int>(42)).Should().Be(true);
            Equals(new Box<int>(42), new Box<int>(41)).Should().Be(false);
            Equals(new Box<int>(42), new Box<int>(43)).Should().Be(false);
        }

        [Test]
        public void BoxOfInt_ShouldEqualsOtherBoxOfIntUsingEqualityOperator()
        {
            (new Box<int>(42) == new Box<int>(42)).Should().Be(true);
            (new Box<int>(42) == new Box<int>(41)).Should().Be(false);
            (new Box<int>(42) == new Box<int>(43)).Should().Be(false);
        }

        [Test]
        public void BoxOfInt_ShouldNotEqualsOtherBoxOfIntUsingInequalityOperator()
        {
            (new Box<int>(42) != new Box<int>(42)).Should().Be(false);
            (new Box<int>(42) != new Box<int>(41)).Should().Be(true);
            (new Box<int>(42) != new Box<int>(43)).Should().Be(true);
        }

        [Test]
        public void BoxOfInt_ShouldEqualsIntUsingEqualityOperator()
        {
            (new Box<int>(42) == 42).Should().Be(true);
            (new Box<int>(42) == 41).Should().Be(false);
            (new Box<int>(42) == 43).Should().Be(false);
        }

        [Test]
        public void BoxOfInt_ShouldNotEqualsUsingInequalityOperator()
        {
            (new Box<int>(42) != 42).Should().Be(false);
            (new Box<int>(42) != 41).Should().Be(true);
            (new Box<int>(42) != 43).Should().Be(true);
        }


        [Test]
        public void BoxOfInt_ShouldEqualsIntUsingObjectEquals()
        {
            Equals(new Box<int>(42), 42).Should().Be(true);
            Equals(new Box<int>(42), 41).Should().Be(false);
            Equals(new Box<int>(42), 43).Should().Be(false);
        }

        [Test]
        public void BoxOfInt_ShouldEqualItSelf()
        {
            var box42 = new Box<int>(42);
            box42.Equals(box42).Should().Be(true);
        }

        [Test]
        public void Bit_ShouldBeComparableUsingFluentAssertions()
        {
            new Box<int>(42).Should().Be(new Box<int>(42));
            new Box<int>(42).Should().Be(42);
            new Box<int>(42).Should().NotBe(null);
            new Box<int>(42).Should().NotBe("fish");
            new Box<int>(42).Should().NotBe(41);
            new Box<int>(42).Should().NotBe("42");
            new Box<int>(42).Should().NotBe(new Box<int>(41));
        }
    }
}
