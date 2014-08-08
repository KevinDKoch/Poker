using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLib2;
using System.Collections.Generic;

namespace PokerLib2Tests
{
    [TestClass]
    public class CardUnitTests
    {
        [TestMethod]
        public void CreateCardWithString()
        {            
            Card testStrCreation = new Card("Ah");
            Assert.AreEqual(testStrCreation.Rank, Rank.Ace);
            Assert.AreEqual(testStrCreation.Suit, Suit.Hearts);
        }

        [TestMethod]
        public void ToStringAndConstructor_ToStringUsedToConstructEqualCards_NoThrows()
        {
            int valueEqualityCount = 0;
            int hashEqualityCount = 0;
            Deck deck = new Deck();
            List<Card> cards = new List<Card>();

            foreach( Card c in deck.Cards)
            {                
                Card cardFromStr = new Card(c.ToString());  
                Assert.IsTrue(c.Equals(cardFromStr));
                Assert.IsTrue(c == cardFromStr);
                Assert.IsTrue(c.Equals((Object)cardFromStr));
                valueEqualityCount++;

                foreach (Card c2 in deck.Cards)
                {
                    if (c.GetHashCode() == c2.GetHashCode())
                    {
                        hashEqualityCount++;
                    }
                    //hashEqualityCount += (c.GetHashCode() == c2.GetHashCode()) ? 1 : 0;
                }                                
            }

            Assert.AreEqual(52, valueEqualityCount);
            Assert.AreEqual(52, hashEqualityCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_InvalidSuit_Throws()
        {            
            Card c2 = new Card(Rank.Two,(Suit)99);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_InvalidRank_Throws()
        {
            Card c2 = new Card((Rank)0, Suit.Diamonds);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Null_Throws()
        {
            Card c2 = new Card(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EmptyString_Throws()
        {
            Card c2 = new Card(String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_StringTooLong_Throws()
        {
            Card c2 = new Card("Aso");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidCardRankString_Throws()
        {
            Card c2 = new Card("Bs");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidCardSuitString_Throws()
        {
            Card c2 = new Card("AS");
        }

        [TestMethod]
        public void Equality_ValueEquality_PassAssertions()
        {
            Assert.IsTrue(new Card("Tc") == new Card(Rank.Ten, Suit.Clubs));
            Assert.IsTrue(new Card("Tc") != new Card(Rank.Ten, Suit.Spades));

            Assert.IsTrue(null != new Card(Rank.Ten, Suit.Spades));
            Assert.IsFalse(null == new Card(Rank.Ten, Suit.Spades));
            Assert.IsTrue(new Card(Rank.Ten, Suit.Spades) != null);
            Assert.IsFalse(new Card(Rank.Ten, Suit.Spades) == null);
        }

        [TestMethod]
        public void CompareToANDComparisionOperators_ComparingCards_PassAssertions()
        {
            Assert.IsTrue(new Card("Ac") > new Card("Kd"));
            Assert.IsTrue(new Card("Ac").CompareTo(new Card("Kd")) == 1);
            
            Assert.IsTrue(new Card("7c") < new Card("Jd"));
            Assert.IsTrue(new Card("7c").CompareTo(new Card("Jd")) == -1);

            Assert.IsTrue(new Card("Ac") >= new Card("Kd"));
            Assert.IsTrue(new Card("Ac").CompareTo(new Card("Kd")) == 1);

            Assert.IsTrue(new Card("7c") <= new Card("8d"));
            Assert.IsTrue(new Card("7c").CompareTo(new Card("8d")) == -1);

            Assert.IsTrue(new Card("Ad") >= new Card("Ac"));
            Assert.IsTrue(new Card("Ad").CompareTo(new Card("Ac")) == 1);

            Assert.IsTrue(new Card("Ad") >= new Card("Ad"));
            Assert.IsTrue(new Card("Ad").CompareTo(new Card("Ad")) == 0);

            Assert.IsTrue(new Card("7c") <= new Card("7d"));
            Assert.IsTrue(new Card("7c").CompareTo(new Card("7d")) == -1);

            Assert.IsTrue(new Card("7c") <= new Card("7c"));
            Assert.IsTrue(new Card("7c").CompareTo(new Card("7c")) == 0);

            Assert.IsTrue(new Card("Ac") == new Card("Ac"));
            Assert.IsTrue(new Card("Ac").CompareTo(new Card("Ac")) == 0 );

            Assert.IsTrue(new Card("7c") != new Card("7d"));
            Assert.IsTrue(new Card("7c").CompareTo(new Card("7d")) == -1);
           
        }

               
    }
}
