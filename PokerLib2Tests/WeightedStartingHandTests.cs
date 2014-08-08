using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLib2;

namespace PokerLib2Tests
{
    [TestClass]
    public class WeightedStartingHandTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Null_throws()
        {
            WeightedStartingHand hand = new WeightedStartingHand(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullWithWeight_throws()
        {
            WeightedStartingHand hand = new WeightedStartingHand(null, .5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EmptyString_throws()
        {
            WeightedStartingHand hand = new WeightedStartingHand(String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_WeightIncludedInHandStringAndParameter_throws()
        {
            WeightedStartingHand hand = new WeightedStartingHand("TsTh(.5)",.5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_ShortHandString_throws()
        {
            WeightedStartingHand hand = new WeightedStartingHand("TsT(.5)", .5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_0UsedForWeight_throws()
        {
            WeightedStartingHand hand = new WeightedStartingHand("AsKh", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_NegativeUsedForWeight_throws()
        {
            WeightedStartingHand hand = new WeightedStartingHand("AsKh", -.01);
        }

        [TestMethod]
        public void Equals_SameHandsWithDifferentWeightsAreNotEqual_Passes()
        {
            WeightedStartingHand smallerWeight = new WeightedStartingHand("AsKc", .5);
            WeightedStartingHand biggerWeight = new WeightedStartingHand("AsKc", .51);

            Assert.IsFalse(smallerWeight.Equals(biggerWeight));
            Assert.IsTrue(smallerWeight.Equals(new WeightedStartingHand("AsKc", .5)));

            Assert.IsFalse(smallerWeight == biggerWeight);
            Assert.IsTrue(smallerWeight == (new WeightedStartingHand("AsKc", .5)));

            Assert.IsFalse((WeightedStartingHand)null == biggerWeight);
            Assert.IsTrue((WeightedStartingHand)null != biggerWeight);

            Assert.IsFalse(new WeightedStartingHand("7c6s", .5).Equals(new StartingHand("7c6s")),"7c6s(.5) does not equal 7c6s.");

            WeightedStartingHand hand = new WeightedStartingHand("QcQs(.25)");
            StartingHand SH = hand;
            Assert.IsTrue(new WeightedStartingHand("QcQs", .25).Equals(SH));

        }


    }
}
