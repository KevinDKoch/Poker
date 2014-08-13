using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerLib2.Game;

namespace PokerLib2.HandHistory
{
    public class BoardCards
    {
        protected Card[] _flop = new Card[3];
        protected Card _turn;
        protected Card _river;

        public Card[] Flop { get { return _flop; } }
        public Card Turn { get { return _turn; } }
        public Card River { get { return _river; } }
        
        public BoardCards()
        {
        }

        public BoardCards(Card[] flop, Card Turn , Card river )
        {
            if (flop == null)
            {
                throw new ArgumentException("The flop can not be null.");
            }
            else if (flop.Length != 3)
            {
                throw new ArgumentException("The flop must be 3 cards.");
            }

            //if (_turn == null & _river != null)
            //{
            //    throw new ArgumentException("If there is a river, there must be a turn.");
            //}

            _flop = flop;
            _turn = Turn;
            _river = river;
        }

        public BoardCards DeepClone()
        {
            Card[] flop = {new Card(_flop[0].ToString()), new Card(_flop[1].ToString()), new Card(_flop[2].ToString())};
            //Card turn = (_turn == null)? null : new Card(_turn.ToString());
            Card turn = new Card(_turn.ToString());
            //Card river = (_river == null)? null : new Card(_river.ToString());
            Card river = new Card(_river.ToString());

            return new BoardCards(flop, turn, river);
        }


    }

}
