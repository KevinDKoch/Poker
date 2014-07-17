using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
    //public enum DealerActionTypes { HoleCards, Flop, Turn, River }
    abstract class DealerAction
    {
        //protected DealerActionTypes _act;
        protected List<Card> _cards;

        public List<Card> Cards { get { return _cards; } }

        protected DealerAction( ){}
    }

    class DealHoleCards : DealerAction
    {
        public DealHoleCards(Card card1, Card card2)
        {
            _cards = new List<Card> { card1, card2 };
        }
    }

    class DealFlop : DealerAction
    {
        public DealFlop(Card card1, Card card2, Card card3)            
        {
            _cards = new List<Card> { card1, card2, card3 };
        }
    }

    class DealTurn : DealerAction
    {
        public DealTurn(Card card1)
        {
            _cards = new List<Card> { card1};
        }
    }

    class DealRiver : DealerAction
    {
        public DealRiver(Card card1)
        {
            _cards = new List<Card> { card1 };
        }
    }
}
