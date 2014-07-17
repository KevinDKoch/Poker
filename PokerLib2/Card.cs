using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
    public enum Rank { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }
    public enum Suit { Clubs, Diamonds, Hearts, Spades }

    class Card
    {
        protected Rank _Rank;
        protected Suit _Suit;

        public Rank Rank { get { return _Rank; } }
        public Suit Suit { get { return _Suit; } }

        public Card(Rank rank, Suit suit)
        {
            _Rank = rank;
            _Suit = suit;
        }

        public override string ToString()
        {
            string name = "Unknown Rank";
            switch (_Rank)
            {
                case Rank.Two:
                    name = "2";
                    break;
                case Rank.Three:
                    name = "3";
                    break;
                case Rank.Four:
                    name = "4";
                    break;
                case Rank.Five:
                    name = "5";
                    break;
                case Rank.Six:
                    name = "6";
                    break;
                case Rank.Seven:
                    name = "7";
                    break;
                case Rank.Eight:
                    name = "8";
                    break;
                case Rank.Nine:
                    name = "9";
                    break;
                case Rank.Ten:
                    name = "T";
                    break;
                case Rank.Jack:
                    name = "J";
                    break;
                case Rank.Queen:
                    name = "Q";
                    break;
                case Rank.King:
                    name = "K";
                    break;
                case Rank.Ace:
                    name = "A";
                    break;
                default:
                    throw new Exception("Unknown Card Rank.");
            }

            switch (_Suit)
            {
                case Suit.Clubs:
                    name += "c";
                    break;
                case Suit.Diamonds:
                    name += "d";
                    break;
                case Suit.Hearts:
                    name += "h";
                    break;
                case Suit.Spades:
                    name += "s";
                    break;
                default:
                    throw new Exception("Unknown Card Suit.");

            }

            return name;
        }
    }
}
