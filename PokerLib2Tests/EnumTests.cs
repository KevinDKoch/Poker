using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLib2;
using System.Collections.Generic;

namespace PokerLib2Tests
{
    [TestClass]
    public class EnumTests
    {
        [TestMethod]
        public void ToLetter_Tests()
        {
            Assert.IsTrue(Suit.Diamonds.ToChar() == 'd');
            Assert.IsTrue(Suit.Diamonds.ToString() == "Diamonds");
            Assert.IsTrue(Rank.Ten.ToString() == "Ten");
            //Assert.IsTrue(Rank.Eight.ToLetter() + Suit.Spades.ToSuit( == "8c");                        
        }

        [TestMethod]
        public void StringToEnumTests()
        {
            Assert.IsTrue('s'.ToSuit() == Suit.Spades);
            Assert.IsTrue('A'.ToRank().ToChar().ToString() + 's'.ToSuit().ToChar() == "As");
        }

        [TestMethod]
        public void ToChar_ToSuit_ToRank_CreateTheSame52Cards()
        {
            List<Card> cards = new List<Card>();
            foreach (Rank r in (Rank[])Enum.GetValues(typeof(Rank)))
            {
                foreach (Suit s in (Suit[])Enum.GetValues(typeof(Suit)))
                {
                    cards.Add(new Card(r, s));
                }
            }

            int iCardCount = 0;
            foreach (Card card in cards)
            {                
                //For each card, see if the card.ToString() can be rebuilt using Rank and Suit ToChar()
                Assert.IsTrue(card.ToString() == card.Rank.ToChar().ToString() + card.Suit.ToChar().ToString());

                //See if the Char ToRank() and ToSuit() extensions can rebuild the card
                Assert.IsTrue(card.Equals(new Card(card.Rank.ToChar() + card.Suit.ToChar().ToString())));
                iCardCount++;
            }
            Assert.IsTrue(iCardCount == 52);
        }

    }
}
