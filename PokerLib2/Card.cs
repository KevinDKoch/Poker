using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
    public enum Rank { Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Jack = 11, Queen = 12, King = 13, Ace = 14 }
    public enum Suit { Clubs = 1, Diamonds = 2, Hearts = 3, Spades = 4 }

    public class Card
    {
        protected readonly Rank _rank;
        protected readonly Suit _suit;

        public Rank Rank { get { return _rank; } }
        public Suit Suit { get { return _suit; } }

        /// <summary>
        /// Represents a playing card.
        /// </summary>
        /// <param name="rank">The rank of the card.</param>
        /// <param name="suit">The suit of the card.</param>
        public Card(Rank rank, Suit suit)
        {
            if (Enum.IsDefined(typeof(Rank), rank) == false) 
            { 
                throw new ArgumentOutOfRangeException("Unknown Rank:" + rank); 
            }

            if (Enum.IsDefined(typeof(Suit), suit) == false)
            {
                throw new ArgumentOutOfRangeException("Unknown Suit:" + suit);
            }

            _rank = rank;
            _suit = suit;
        }

        /// <summary>
        /// Represents a playing card.
        /// </summary>
        /// <param name="card">A two letter representation of a card.  The capitolized first letter represents the rank, followed by a lowercase letter representing the suit.</param>
        public Card(string card)
        {

            if(card.Length != 2) throw new ArgumentException("Unknown Card:" + card);

            switch (card[0])
            {
                case '2':
                    _rank = Rank.Two;
                    break;
                case '3':
                    _rank = Rank.Three;
                    break;
                case '4':
                    _rank = Rank.Four;
                    break;
                case '5':
                    _rank = Rank.Five;
                    break;
                case '6':
                    _rank = Rank.Six;
                    break;
                case '7':
                    _rank = Rank.Seven;
                    break;
                case '8':
                    _rank = Rank.Eight;
                    break;
                case '9':
                    _rank = Rank.Nine;
                    break;
                case 'T':
                    _rank = Rank.Ten;
                    break;
                case 'J':
                    _rank = Rank.Jack;
                    break;
                case 'Q':
                    _rank = Rank.Queen;
                    break;
                case 'K':
                    _rank = Rank.King;
                    break;
                case 'A':
                    _rank = Rank.Ace;
                    break;
                default:
                    throw new ArgumentException("Unknown Rank:" + card);
            }

            switch (card[1])
            {
                case 'c':
                    _suit = Suit.Clubs;
                    break;
                case 'd':
                    _suit = Suit.Diamonds;
                    break;
                case 'h':
                    _suit = Suit.Hearts;
                    break;
                case 's':
                    _suit = Suit.Spades;
                    break;
                default:
                    throw new ArgumentException("Unknown Suit:" + card);
            }

        }

        public override bool Equals(object obj)
        {
            Card card = obj as Card;
            if (obj == null)
            {
                return false;
            }            
            return Equals(card);            
        }

        public bool Equals(Card card)
        {
            if (card == null)
            {
                return false;
            }
            return (card.Rank == _rank && card.Suit == _suit);
        }

        public override int GetHashCode()
        {           
            return _rank.GetHashCode() * 17 ^ _suit.GetHashCode() * 23;
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
            }//switch(_rank)

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
            }//switch(_suit)

            return name;
        }
    }
}
