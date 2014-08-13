using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2.Game
{
    public class Deck
    {
        protected List<Card> _deck = new List<Card>();
        public List<Card> @Cards { get { return _deck; } }

        public Deck()
        {
            foreach (Rank r in (Rank[])Enum.GetValues(typeof(Rank)))
            {
                foreach (Suit s in (Suit[])Enum.GetValues(typeof(Suit)))
                {
                    _deck.Add(new Card(r, s));
                }
            }

            if (_deck.Count != 52)
            {
                throw new ArgumentException("You are not playing with a full deck!");
            }
        }
        
    }
}
