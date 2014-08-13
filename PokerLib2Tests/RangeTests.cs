using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLib2;
using PokerLib2.Game;
using PokerLib2.HandHistory;

namespace PokerLib2Tests
{
    [TestClass]
    public class RangeTests
    {
        [TestMethod]
        public void CreateRangesFromStrings()
        {
            Range range = new Range("{AcKs}");
            Assert.IsTrue(range.Combos() == 1);

            range = new Range("{AcKs,AsKc}");
            Assert.IsTrue(range.Combos() == 2);

            range = new Range("{AcKs,AsKc,AcAh}");
            Assert.IsTrue(range.Combos() == 3);

            range = new Range("{AK}");
            Assert.IsTrue(range.Combos() == 16);

            range = new Range("{99}");
            Assert.IsTrue(range.Combos() == 6);

            range = new Range("{AKo}");
            Assert.IsTrue(range.Combos() == 12);

            range = new Range("{AKs(.5)}");
            Assert.IsTrue(range.Combos() == 2);

            range = new Range("{A*s}");
            Assert.IsTrue(range.Combos() == 12*4);

            range = new Range("{*As}");
            Assert.IsTrue(range.Combos() == 12 * 4);

            range = new Range("{**s}");
            Assert.IsTrue(range.Combos() == 312);

            range = new Range("{**o}");
            Assert.IsTrue(range.Combos() == 936);

            range = new Range("{**}");
            Assert.IsTrue(range.Combos() == 1326);

            range = new Range("{**(.5)}");
            Assert.IsTrue(range.Combos() == 1326 * .5);

            range = new Range("{A5s-A2s}");
            Assert.IsTrue(range.Combos() == 4 * 4);

            range = new Range("{A5s-2As}");
            Assert.IsTrue(range.Combos() == 4 * 4);

            range = new Range("{22-AA}");
            Assert.IsTrue(range.Combos() == 6 * 13);

            range = new Range("{77-33}");
            Assert.IsTrue(range.Combos() == 6 * 5);

            range = new Range("{22-33}");
            Assert.IsTrue(range.Combos() == 6 * 2);

            range = new Range("{TT-AA(.5)}");
            Assert.IsTrue(range.Combos() == 5 * 6 * .5);

            range = new Range("{AKo-A3o(.75)}");
            Assert.IsTrue(range.Combos() == 12 * 11 * .75 );

            range = new Range("{JT-76(.75)}");
            Assert.IsTrue(range.Combos() == 16 * 5 * .75);

            range = new Range("{TT+}");
            Assert.IsTrue(range.Combos() == 6 * 5);

            range = new Range("{TT-}");
            Assert.IsTrue(range.Combos() == 6 * 9);

            range = new Range("{76s+}");
            Assert.IsTrue(range.Combos() == 4 * 8);

            range = new Range("{76o+}");
            Assert.IsTrue(range.Combos() == 12 * 8);

            range = new Range("{76+}");
            Assert.IsTrue(range.Combos() == 16 * 8);

            this.TestRange("{76s,76o}", 16);


        }

        public void TestRange(string range, double expectedCombos)
        {
            Range testRange = new Range(range);
            Assert.AreEqual(expectedCombos, testRange.Combos(), range);
        }
        [TestMethod]
        public void InValidRanges()
        {

            Assert.IsTrue(Range.IsValidRange("{AKs-AQs}"));
            Assert.IsTrue(Range.IsValidRange("{JTo-76o}"));
            Assert.IsTrue(Range.IsValidRange("{76o-JTo}"));
            Assert.IsTrue(Range.IsValidRange("{76-JT}"));
            Assert.IsTrue(Range.IsValidRange("{A5s-A2s}"));
            Assert.IsTrue(Range.IsValidRange("{22-99}"));
            Assert.IsTrue(Range.IsValidRange("{99-22}"));

            Assert.IsFalse(Range.IsValidRange("{A5s-A2}"));
            Assert.IsFalse(Range.IsValidRange("{A5-A2s}"));
            Assert.IsFalse(Range.IsValidRange("{76o-J9o}"));
            Assert.IsFalse(Range.IsValidRange("{AKs-72s}"));
            Assert.IsFalse(Range.IsValidRange("{99-22s}"));
            Assert.IsFalse(Range.IsValidRange("{AK-22s}"));
            Assert.IsFalse(Range.IsValidRange("{AK-22s}"));
            
        }

        [TestMethod]
        public void Add_AddingDuplicateWeightedStartingHandOfDifferentWeight_Passes()
        {
            Range r = new Range("AKs(.25)");
            r.ToString();

            Assert.AreEqual(4, r.Count);
            r.Add(new WeightedStartingHand("AcKc(.255)"));
            Assert.AreEqual(4, r.Count);
            Assert.IsFalse(r.Contains(new WeightedStartingHand("AcKc(.25)")));
            Assert.IsTrue(r.Contains(new WeightedStartingHand("AcKc(.255)")));

        }

        [TestMethod]
        public void Equals_RangeOrderDoesNotMatter_Passes()
        {
            Range r1 = new Range("99(.5),AKs");
            Range r2 = new Range("AKs,99(.5)");

            Assert.IsTrue(r1.Equals(r2), "Equality Failed when order was changed.");
        }

        [TestMethod]
        public void Equals_SameRangeCreatedWithDifferentStrings_Passes()
        {
            Range r1 = new Range("77+,66+");
            Range r2 = new Range("66+");

            Assert.IsTrue(r1.Equals(r2), "Equality Failed with equal range strings.");

            r1.AddRange(new Range("A5s-A2s,A*s").Hands);
            r2.AddRange(new Range("AKs-A2s").Hands);

            Assert.IsTrue(r1.Equals(r2), "Equality Failed with equal range strings.");

            r1.AddRange(new Range("A5s-A2s(.1),A*s(.5)").Hands);
            r2.AddRange(new Range("AKs-A2s(.5)").Hands);

            Assert.IsTrue(r1.Equals(r2), "Equality Failed with equal range strings.");

        }

        [TestMethod]
        public void Equals_OrderMatterWithStringCreationAndWeighting_Passes()
        {
            Range r1 = new Range("22+,TT+(.5)");
            Range r2 = new Range("TT+(.5),22+");

            Assert.IsFalse(r1.Equals(r2), "Equality was unexpectedly equal.");
            
            Assert.IsTrue(r1.Equals(new Range("TT+(.5),99-22")), "Equality was expected to be equal.");
            Assert.IsTrue(r2.Equals(new Range("22+")), "Equality was expected to be equal.");            
        }

        [TestMethod]
        public void ToString_highCardFirstSortsStartingHandOrder_Passes()
        {
            Range r1 = new Range("AcKs,QcQs");
            Range r2 = new Range("KsAc,QsQc");

            Assert.IsTrue(r1.ToString() != r2.ToString());
            Assert.IsTrue(r1.ToString(true) == r2.ToString(true));
            Assert.IsTrue(r1.ToString(true) != r2.ToString(false));
        }
    }
}
