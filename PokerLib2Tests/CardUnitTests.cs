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
        public void ToString_Creates_Valid_Cards_With_Value_Equality()
        {
            int valueEqualityCount = 0;
            int hashEqualityCount = 0;
            Deck deck = new Deck();
            List<Card> cards = new List<Card>();

            foreach( Card c in deck.Cards)
            {                
                Card cardFromStr = new Card(c.ToString());  
                Assert.IsTrue(c.Equals(cardFromStr));
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
        public void TestInvalidCards()
        {
            //Card c1 = new Card("");
            Card c2 = new Card(Rank.Two,(Suit)99);
            Console.WriteLine(c2.ToString());
        }
               
    }
}
