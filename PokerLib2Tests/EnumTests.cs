using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLib2;
using System.Collections.Generic;
using PokerLib2.Game;
using PokerLib2.HandHistory;

namespace PokerLib2Tests
{
    [TestClass]
    public class EnumTests
    {
        [TestMethod]
        public void ToLetter_Tests()
        {
            Assert.IsTrue(Suit.Diamonds.ToLetter() == "d");
            Assert.IsTrue(Suit.Diamonds.ToString() == "Diamonds");
            Assert.IsTrue(Rank.Ten.ToString() == "Ten");
            //Assert.IsTrue(Rank.Eight.ToLetter() + Suit.Spades.ToSuit( == "8c");                        
        }

        [TestMethod]
        public void StringToEnumTests()
        {
            Assert.IsTrue('s'.ToSuit() == Suit.Spades);
            Assert.IsTrue('A'.ToRank().ToLetter() + 's'.ToSuit().ToLetter() == "As");
        }

        [TestMethod]
        public void ToLetter_ToSuit_ToRank_CreateTheSame52Cards()
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
                //For each card, see if the card.ToString() can be rebuilt using Rank and Suit ToLetter()
                Assert.IsTrue(card.ToString() == card.Rank.ToLetter() + card.Suit.ToLetter());

                //See if the Char ToRank() and ToSuit() extensions can rebuild the card
                Assert.IsTrue(card.Equals(new Card(card.Rank.ToLetter() + card.Suit.ToLetter())));
                iCardCount++;
            }
            Assert.IsTrue(iCardCount == 52);
        }

    }
}
