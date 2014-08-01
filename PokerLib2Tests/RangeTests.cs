using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLib2;

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

    }
}
