using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
    public enum Rank { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }
    public enum Suit { Clubs, Diamonds, Hearts, Spades }

    public class Card
    {
        protected Rank _rank;
        protected Suit _suit;

        public Rank Rank { get { return _rank; } }
        public Suit Suit { get { return _suit; } }

        public Card(Rank rank, Suit suit)
        {
            _rank = rank;
            _suit = suit;
        }

        public Card(string card)
        {
            Deck deck = new Deck();
            foreach (Card c in deck.Cards)
            {
                if (card == c.ToString())
                {
                    _rank = c.Rank;
                    _suit = c.Suit;
                    break;
                }
            }
        }

        public override string ToString()
        {
            string name = "Unknown Rank";
            switch (_rank)
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

            switch (_suit)
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
