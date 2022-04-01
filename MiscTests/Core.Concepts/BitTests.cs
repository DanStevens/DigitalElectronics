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
        public void Bit_ShouldEqualsOtherBit()
        {
            new Bit(false).Equals(new Bit(false)).Should().Be(true);
            new Bit(false).Equals(new Bit(true)).Should().Be(false);
            new Bit(true).Equals(new Bit(false)).Should().Be(false);
            new Bit(true).Equals(new Bit(true)).Should().Be(true);
        }
    }
}
