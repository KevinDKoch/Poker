using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PokerLib2.Game
{
    public static class PokerRegex
    {
        public const string rank = @"[2-9TJQKA]";
        public const string suit = @"[cdhs]";
        public const string card = rank + suit;
        public const string hand = "(" + card + @")(?!\1)" + card; //([2-9TJQKA][cdhs])(?!\1)[2-9TJQKA][cdhs]
        public const string suitedness = @"[os]";

        //^([2-9TJQKA])(?!\1)[2-9TJQKA][os]$|^([2-9TJQKA])\2$
        public const string shortHandFormat = @"^(" + rank + @")(?!\2)" + rank + suitedness + "$|^(" + rank + @")\2$";

        public const string weight = @"\(1\)|\((0?\.\d+)\)";

        public static class RangeGroups
        {
            public const string groupOperators = @"[\+\-]";
            public const string singleHand = "^" + hand + "$"; //AsKc, 9h9s
            public const string handGroup = "^" + @"((" + rank + @")(?!\2)" + rank + suitedness + @")$|^(" + rank + rank + @")$"; //AK, AKo, AKs, 99
                        
            //A*, *A, A*s, *Ao, **, **s
            public const string wildGroup = "^" + rank + @"\*" + suitedness + @"?$|^\*" + rank + suitedness + @"?$|^\*\*" + suitedness + "?$";
            
            public const string openLinear = @"^[2-9TJQKA][2-9TJQKA][\+\-]$|^([2-9TJQKA])(?!\1)[2-9TJQKA][os][\+\-]$";
            
            public const string closedLinear = @"^[2-9TJQKA][2-9TJQKA]\-[2-9TJQKA][2-9TJQKA]$|^([2-9TJQKA])(?!\1)[2-9TJQKA]([os])\-[2-9TJQKA][2-9TJQKA](?=\2)[os]$";            
            
        }


    }
}
