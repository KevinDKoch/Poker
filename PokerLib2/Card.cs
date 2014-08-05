using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
    public struct Card
    {
        private readonly Rank rank;
        private readonly Suit suit;

        public Rank Rank { get { return this.rank; } }
        public Suit Suit { get { return this.suit; } }

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

            this.rank = rank;
            this.suit = suit;
        }

        /// <summary>
        /// Represents a playing card.
        /// </summary>
        /// <param name="card">A two letter representation of a card.  The capitolized first letter represents the rank, followed by a lowercase letter representing the suit.</param>
        public Card(string card)
        {
            if(card.Length != 2) throw new ArgumentException("Too many characters:" + card);
            this.rank = card[0].ToRank();
            this.suit = card[1].ToSuit();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Card))            
                return false;

            return Equals((Card)obj);
        }

        public bool Equals(Card card)
        {
            return (card.Rank == this.rank && card.Suit == this.suit);
        }

        public override int GetHashCode()
        {           
            return this.rank.GetHashCode() * 17 ^ this.suit.GetHashCode() * 23;
        }

        public override string ToString()
        {
            string name = this.rank.ToChar().ToString();
            name += this.suit.ToChar().ToString();
            return name;
        }
    }
}
