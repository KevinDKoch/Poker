using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
    public struct Card : IEquatable<Card>,IComparable<Card>
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
        /// <param name="card">A two letter representation of a card.  The capitolized first letter represents the rank, followed by a lowercase letter representing the suit.
        /// <para>Ex: Ac</para></param>
        public Card(string card)
        {
            if ((Object)card == null) 
                throw new ArgumentNullException("card");
            if(card.Length != 2) 
                throw new ArgumentException("Card must be 2 characters:" + card);

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

        public static bool operator ==(Card left, Card right)
        {
            if (object.ReferenceEquals(left, right)) 
                return true;

            // If one is null, but not both, return false.
            if (((object)left == null) || ((object)right == null))            
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Card left, Card right)
        {
            return !(left == right);
        }


        /// <summary>
        /// Compares two cards for sorting based off their rank and suit.
        /// </summary>
        /// <param name="other">The card to be compared with.</param>
        /// <returns>1 if greater than, 0 if equal, -1 if less than. </returns>
        public int CompareTo(Card other)
        {
            if (this == other)
                return 0;

            if (this.Rank == other.Rank )
            {
                if(this.Suit == other.Suit)
                    return 0;
                else
                    return(this.Suit > other.Suit)? 1 : -1;
            }
            
            return (this.Rank > other.Rank) ? 1 : -1;
        }

        public static bool operator >(Card left, Card right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <(Card left, Card right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >=(Card left, Card right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <=(Card left, Card right)
        {
            return left.CompareTo(right) <= 0;
        }

        public override string ToString()
        {
            string name = this.rank.ToLetter();
            name += this.suit.ToLetter();
            return name;
        }
    }
}
