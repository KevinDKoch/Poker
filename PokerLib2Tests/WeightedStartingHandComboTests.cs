using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLib2;
using PokerLib2.Game;
using PokerLib2.HandHistory;

namespace PokerLib2Tests
{
    [TestClass]
    public class WeightedStartingHandComboTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Null_throws()
        {
            WeightedStartingHandCombo hand = new WeightedStartingHandCombo(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullWithWeight_throws()
        {
            WeightedStartingHandCombo hand = new WeightedStartingHandCombo(null, .5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EmptyString_throws()
        {
            WeightedStartingHandCombo hand = new WeightedStartingHandCombo(String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_WeightIncludedInHandStringAndParameter_throws()
        {
            WeightedStartingHandCombo hand = new WeightedStartingHandCombo("TsTh(.5)",.5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_ShortHandString_throws()
        {
            WeightedStartingHandCombo hand = new WeightedStartingHandCombo("TsT(.5)", .5);
        }

        [TestMethod]        
        public void Constructor_0UsedForWeight_Passes()
        {
            WeightedStartingHandCombo hand = new WeightedStartingHandCombo("AsKh", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_NegativeUsedForWeight_throws()
        {
            WeightedStartingHandCombo hand = new WeightedStartingHandCombo("AsKh", -.01);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NegativeUsedForWeightInString_throws()
        {
            WeightedStartingHandCombo hand = new WeightedStartingHandCombo("AsKh(-.01)");
        }

        [TestMethod]
        public void Equals_SameHandsWithDifferentWeightsAreNotEqual_Passes()
        {
            WeightedStartingHandCombo smallerWeight = new WeightedStartingHandCombo("AsKc", .5);
            WeightedStartingHandCombo biggerWeight = new WeightedStartingHandCombo("AsKc", .51);

            Assert.IsFalse(smallerWeight.Equals(biggerWeight));
            Assert.IsTrue(smallerWeight.Equals(new WeightedStartingHandCombo("AsKc", .5)));

            Assert.IsFalse(smallerWeight == biggerWeight);
            Assert.IsTrue(smallerWeight == (new WeightedStartingHandCombo("AsKc", .5)));

            Assert.IsFalse((WeightedStartingHandCombo)null == biggerWeight);
            Assert.IsTrue((WeightedStartingHandCombo)null != biggerWeight);

            Assert.IsFalse(new WeightedStartingHandCombo("7c6s", .5).Equals(new StartingHandCombo("7c6s")),"7c6s(.5) does not equal 7c6s.");

            WeightedStartingHandCombo hand = new WeightedStartingHandCombo("QcQs(.25)");
            StartingHandCombo SH = hand;
            Assert.IsTrue(new WeightedStartingHandCombo("QcQs", .25).Equals(SH));

            SH = new StartingHandCombo("QcQs");
            Assert.IsFalse(new WeightedStartingHandCombo("QcQs", .25).Equals(SH));

        }

        [TestMethod]
        public void EqualityOperator_NullComparisons_Passes()
        {
            Assert.IsTrue((WeightedStartingHandCombo)null == (WeightedStartingHandCombo)null);
            Assert.IsFalse((WeightedStartingHandCombo)null == new WeightedStartingHandCombo("AsKs", .5));
            Assert.IsFalse(new WeightedStartingHandCombo("AsKs", .5)==(WeightedStartingHandCombo)null);
        }

        [TestMethod]
        public void InequalityOperator_NullComparisons_Passes()
        {
            Assert.IsFalse((WeightedStartingHandCombo)null != (WeightedStartingHandCombo)null);
            Assert.IsTrue((WeightedStartingHandCombo)null != new WeightedStartingHandCombo("AsKs", .5));
            Assert.IsTrue(new WeightedStartingHandCombo("AsKs", .5) != (WeightedStartingHandCombo)null);
        }       

        [TestMethod]
        public void ToString_IncludesWeight_Passes()
        {
            Assert.AreEqual("AcKh(0.333)", new WeightedStartingHandCombo("AcKh", .333).ToString());
            Assert.AreEqual("AcKh", new WeightedStartingHandCombo("AcKh", 1).ToString());

            WeightedStartingHandCombo testDecimals = new WeightedStartingHandCombo("3c2h", .333);
            Assert.AreEqual(testDecimals.ToString(), new WeightedStartingHandCombo(testDecimals.ToString()).ToString());

            string hand = "AcKh(0.333)";
            Assert.AreEqual(hand, new WeightedStartingHandCombo(new WeightedStartingHandCombo("AcKh", .333).ToString()).ToString());

        }
        [TestMethod]
        public void ToStringOverloads_FormatingWorksAsExpected_Passes()
        {

            WeightedStartingHandCombo hand = new WeightedStartingHandCombo("KhQd", .5);
            Assert.IsTrue(hand.ToString(true) == "KQo(0.5)");

            Assert.IsTrue(new WeightedStartingHandCombo("QhKh", .5).ToString(true, true) == "KQs(0.5)");

        }

    }
}
