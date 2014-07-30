using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
    class Range
    {
        protected List<WeightedStartingHand> _hands = new List<WeightedStartingHand>();
        public List<WeightedStartingHand> Hands { get { return _hands; } set { _hands = value; } }

        public Range(List<WeightedStartingHand> hands)
        {
            _hands = hands;
        }

        //TODO Some sort of string representation of range.
        //public Range(string hands)
        //{
            
        //}       
    }
}
