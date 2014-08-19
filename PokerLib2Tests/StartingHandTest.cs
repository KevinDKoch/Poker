using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLib2.Game;

namespace PokerLib2Tests
{
    [TestClass]
    public class StartingHandTest
    {
        [TestMethod]
        public void CommonUsage()
        {
            StartingHand SH = new StartingHand("AKs");
            Assert.IsTrue(SH.WeightedCount() == 4);

            SH = new StartingHand("AKo");
            Assert.IsTrue(SH.WeightedCount() == 12);

            SH = new StartingHand("AA");
            Assert.IsTrue(SH.WeightedCount() == 6);

            SH = new StartingHand("AcKs");
            Assert.IsTrue(SH.WeightedCount() == 1);

            SH.Add(new WeightedStartingHandCombo("AsKc"));
            Assert.IsTrue(SH.WeightedCount() == 2);

            SH.AddRange(new Range("AKo(.5)"));
            Assert.IsTrue(SH.WeightedCount() == 6);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EmptyString_Throws()
        {
            StartingHand SH = new StartingHand(String.Empty);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_AddInvalidStartingHand_Throws()
        {
            StartingHand SH = new StartingHand("AKs");
            SH.Add(new WeightedStartingHandCombo("QcQh"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_AddWrongSuitedness_Throws()
        {
            StartingHand SH = new StartingHand("AKs, AKo");            
        }
        
    }
}
