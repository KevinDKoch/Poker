using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2.Game
{
    public enum Rank { Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Jack = 11, Queen = 12, King = 13, Ace = 14 }

    public static class RankExtension
    {
        public static string ToLetter(this Rank rank)
        {
            if (rank >= Rank.Two & rank <= Rank.Nine)
                return ((int)rank).ToString()[0].ToString();
            else
                return rank.ToString()[0].ToString();
        }
    }

    public enum Suit { Clubs = 1, Diamonds = 2, Hearts = 3, Spades = 4 }

    public static class SuitExtension
    {
        /// <summary>
        /// Returns a single letter which represents a card suit.  ex: Clubs == "c"
        /// </summary>
        /// <param name="suit">The suit to get the letter for.</param>
        /// <returns>The letter which representing the suit.</returns>
        public static string ToLetter(this Suit suit)
        {
            return suit.ToString().ToLower()[0].ToString();
        }
    }

    public static class CharExtension
    {
        /// <summary>
        /// Returns a letter representing a card rank. ex: King == "K"
        /// </summary>
        /// <param name="rank">The rank to convert to a letter.</param>
        /// <returns>A single letter representing a rank.</returns>
        public static Rank ToRank(this char rank)
        {
            switch (rank.ToString())
            {
                case "2":
                    return Rank.Two;
                case "3":
                    return Rank.Three;
                case "4":
                    return Rank.Four;
                case "5":
                    return Rank.Five;
                case "6":
                    return Rank.Six;
                case "7":
                    return Rank.Seven;
                case "8":
                    return Rank.Eight;
                case "9":
                    return Rank.Nine;
                case "T":
                    return Rank.Ten;
                case "J":
                    return Rank.Jack;
                case "Q":
                    return Rank.Queen;
                case "K":
                    return Rank.King;
                case "A":
                    return Rank.Ace;
                default:
                    throw new ArgumentException("Unknown Rank:" + rank);
            }
        }

        public static Suit ToSuit(this char suit)
        {
            switch (suit.ToString())
            {
                case "c":
                    return Suit.Clubs;
                case "d":
                    return Suit.Diamonds;
                case "h":
                    return Suit.Hearts;
                case "s":
                    return Suit.Spades;
                default:
                    throw new ArgumentException("Unknown Suit:" + suit);
            }
        }

    }
}
