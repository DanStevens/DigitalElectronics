using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Concepts.Tests
{
    public class BitTests
    {

        [Test]
        public void Bit_ShouldBeImplicitlyCastableToBoolean()
        {
            bool b = new Bit(true);
            b.Should().BeTrue();
        }

        [Test]
        public void Bit_ShouldBeExplicitlyCastableToBoolean()
        {
            var b = (bool)new Bit(true);
            b.Should().BeTrue();
        }

        [Test]
        public void Bit_ShouldBeCreatableByImplicitCastFromBool()
        {
            Bit bit = true;
            bit.Value.Should().Be(true);
        }

        [Test]
        public void Bit_ShouldBeCreatableByExplicitCastFromBool()
        {
            Bit bit = (Bit)true;
            bit.Value.Should().Be(true);
        }

        [Test]
        public void Bit_ShouldBeComparableToBoolean()
        {
            new Bit(false).CompareTo(false).Should().Be(0);
            new Bit(false).CompareTo(true).Should().Be(-1);
            new Bit(true).CompareTo(false).Should().Be(1);
            new Bit(true).CompareTo(true).Should().Be(0);
        }

        [Test]
        [Ignore("Unsure whether or not we want this logic")]
        public void Bit_ShouldBeComparableToNullAsThoughFalse()
        {
            new Bit(false).CompareTo(null).Should().Be(0);
        }

        [Test]
        public void Bit_ShouldEqualsBoolean()
        {
            new Bit(false).Equals(false).Should().Be(true);
            new Bit(false).Equals(true).Should().Be(false);
            new Bit(true).Equals(false).Should().Be(false);
            new Bit(true).Equals(true).Should().Be(true);
        }

        [Test]
        [Ignore("Unsure whether or not we want this logic")]
        public void Bit_ShouldEqualNullAsThoughFalse()
        {
            new Bit(false).Equals(null).Should().Be(true);
        }

        [Test]
        public void Bit_ShouldBeComparableToOtherBit()
        {
            new Bit(false).CompareTo(new Bit(false)).Should().Be(0);
            new Bit(false).CompareTo(new Bit(true)).Should().Be(-1);
            new Bit(true).CompareTo(new Bit(false)).Should().Be(1);
            new Bit(true).CompareTo(new Bit(true)).Should().Be(0);
        }

        [Test]
        public void Bit_ShouldBeComparableToBool()
        {
            new Bit(false).CompareTo(false).Should().Be(0);
            new Bit(false).CompareTo(true).Should().Be(-1);
            new Bit(true).CompareTo(false).Should().Be(1);
            new Bit(true).CompareTo(true).Should().Be(0);
        }


        [Test]
        public void Bit_ShouldEqualsOtherBit()
        {
            new Bit(false).Equals(new Bit(false)).Should().Be(true);
            new Bit(false).Equals(new Bit(true)).Should().Be(false);
            new Bit(true).Equals(new Bit(false)).Should().Be(false);
            new Bit(true).Equals(new Bit(true)).Should().Be(true);
        }

        [Test]
        public void Bit_ShouldEqualsBool()
        {
            new Bit(false).Equals(false).Should().Be(true);
            new Bit(false).Equals(true).Should().Be(false);
            new Bit(true).Equals(false).Should().Be(false);
            new Bit(true).Equals(true).Should().Be(true);
        }

        [Test]
        public void Bit_ShouldEqualsOtherBitUsingObjectEquals()
        {
            Equals(new Bit(false), new Bit(false)).Should().Be(true);
            Equals(new Bit(false), new Bit(true)).Should().Be(false);
            Equals(new Bit(true), new Bit(false)).Should().Be(false);
            Equals(new Bit(true), new Bit(true)).Should().Be(true);
        }

        [Test]
        public void Bit_ShouldEqualsOtherBitUsingEqualityOperator()
        {
            (new Bit(false) == new Bit(false)).Should().Be(true);
            (new Bit(false) == new Bit(true)).Should().Be(false);
            (new Bit(true) == new Bit(false)).Should().Be(false);
            (new Bit(true) == new Bit(true)).Should().Be(true);
        }

        [Test]
        public void Bit_ShouldNotEqualsOtherBitUsingInequalityOperator()
        {
            (new Bit(false) != new Bit(false)).Should().Be(false);
            (new Bit(false) != new Bit(true)).Should().Be(true);
            (new Bit(true) != new Bit(false)).Should().Be(true);
            (new Bit(true) != new Bit(true)).Should().Be(false);
        }

        [Test]
        public void Bit_ShouldEqualsBoolUsingObjectEquals()
        {
            Equals(new Bit(false), false).Should().Be(true);
            Equals(new Bit(false), true).Should().Be(false);
            Equals(new Bit(true), false).Should().Be(false);
            Equals(new Bit(true), true).Should().Be(true);
        }

        [Test]
        public void Bit_ShouldEqualsBoolUsingEqualityOperator()
        {
            (new Bit(false) == false).Should().Be(true);
            (new Bit(false) == true).Should().Be(false);
            (new Bit(true) == false).Should().Be(false);
            (new Bit(true) == true).Should().Be(true);
        }

        [Test]
        public void Bit_ShouldNotEqualsBoolUsingInequalityOperator()
        {
            (new Bit(false) != false).Should().Be(false);
            (new Bit(false) != true).Should().Be(true);
            (new Bit(true) != false).Should().Be(true);
            (new Bit(true) != true).Should().Be(false);
        }

        [Test]
        public void Bit_ShouldEqualItSelf()
        {
            var bit = new Bit(true);
            bit.Equals(bit).Should().Be(true);
        }

        [Test]
        public void Bit_ShouldBeComparableUsingFluentAssertions()
        {
            new Bit(true).Should().Be(new Bit(true));
            new Bit(true).Should().Be(true);
            new Bit(true).Should().NotBe(null);
            new Bit(true).Should().NotBe("fish");
            new Bit(true).Should().NotBe(false);
            new Bit(true).Should().NotBe("true");
            new Bit(true).Should().NotBe(new Bit(false));
        }
    }
}
