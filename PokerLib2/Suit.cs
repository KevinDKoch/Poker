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
        public static char ToChar(this Suit suit)
        {
            return suit.ToString().ToLower()[0];
        }
    }
}
