using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PokerLib2
{
    public static class PokerRegex
    {
        public const string rank = @"[2-9,T,J,Q,K,A]";
        public const string suit = @"[c,d,h,s]";
        public const string card = rank + suit;
        public const string hand = "(" + card + @")(?!\1)" + card; //([2-9,T,J,Q,K,A][c,d,h,s])(?!\1)[2-9,T,J,Q,K,A][c,d,h,s]
        public const string suitedness = @"[o,s]";
        public const string shortHandFormat = @"((" + rank + @")(?!\2)" + rank + suitedness + ")|(" + rank + rank + @")";

        public const string weight = @"\(1\)|\((0?\.\d+)\)";

        //TODO: 76s+, 76o-        
        //TODO: JTs-76s
        public static class RangeGroups
        {
            public const string groupOperators = @"[\+,\-]";
            public const string singleHand = "^" + hand + "$"; //AsKc, 9h9s
            public const string handGroup = "^" + @"((" + rank + @")(?!\2)" + rank + suitedness + @")$|^(" + rank + rank + @")$"; //AK, AKo, AKs, 99
            
            //[2-9,T,J,Q,K,A]\*[o,s]?|\*[2-9,T,J,Q,K,A][o,s]?|\*\*[o,s]?
            //A*, *A, A*s, *Ao, **, **s
            public const string wildGroup = "^" + rank + @"\*" + suitedness + @"?$|^\*" + rank + suitedness + @"?$|^\*\*" + suitedness + "?$";
            public const string openLinear = @"^[2-9,T,J,Q,K,A][2-9,T,J,Q,K,A][\+\-]$|^([2-9,T,J,Q,K,A])(?!\1)[2-9,T,J,Q,K,A][o,s][\+,\-]$";
            public const string closedLinear = @"^[2-9TJQKA][2-9TJQKA]\-[2-9TJQKA][2-9TJQKA]$|^([2-9TJQKA])(?!\1)[2-9TJQKA]([os])\-[2-9TJQKA][2-9TJQKA](?=\2)[os]$";            
            
        }


    }
}
