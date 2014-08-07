using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
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
}
