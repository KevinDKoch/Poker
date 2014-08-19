using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PokerLib2.Game
{
    public class StartingHandCombo : IEquatable<StartingHandCombo>, IComparable<StartingHandCombo>
    {
        protected readonly Card _firstCard;
        protected readonly Card _secondCard;

        public  Card FirstCard { get { return _firstCard; } }
        public  Card SecondCard { get { return _secondCard; } }

        public Card HighCard 
        { 
            get 
            {
                //If it's a PP, then compare the suits instead of ranks
                if (IsPocketPair())
                    return (_secondCard.Suit > _firstCard.Suit) ? _secondCard : _firstCard; 
                else                
                    return (_secondCard.Rank > _firstCard.Rank) ? _secondCard : _firstCard;                                  
            } 
        }

        public Card LowCard 
        { 
            get 
            {
                //If it's a PP, then compare the suits instead of ranks
                if (IsPocketPair())
                    return (_secondCard.Suit < _firstCard.Suit) ? _secondCard : _firstCard;
                else               
                    return (_secondCard.Rank <= _firstCard.Rank) ? _secondCard : _firstCard; 
            } 
        }

        public enum MatchingMode { ExactSuits, Suitedness, RankOnly }

        public StartingHandCombo(Card firstCard, Card secondCard)
        {
            ValidateCards(firstCard, secondCard);
            _firstCard = firstCard;
            _secondCard = secondCard;
        }

        public StartingHandCombo(string hand)
        {
            if (hand == null) 
                throw new ArgumentNullException(hand, "The starting hand cannot be null.");

            if (hand == string.Empty)
                throw new ArgumentException(hand, "The starting hand cannot be an empty string.");

            if (hand.Length != 4) 
                throw new ArgumentException(hand, "The starting hand must be 4 letters long:" + hand);
            
            if (Regex.IsMatch(hand, PokerRegex.hand) == false)
                throw new ArgumentException(hand, "The format of the starting hand was not recognized:" + hand);
                      
            Card firstCard = new Card(hand.Substring(0, 2));
            Card secondCard = new Card(hand.Substring(2,2));
            ValidateCards(firstCard, secondCard);

            _firstCard = firstCard;
            _secondCard = secondCard;
        }

        protected void ValidateCards(Card firstCard, Card secondCard)
        {
            if (firstCard.Equals(secondCard))
                throw new ArgumentException("A starting hand must contain two unique cards.");

        }
            
        public bool IsSuited()
        {
            return (_firstCard.Suit == _secondCard.Suit);
        }

        public int Gap()
        {
            return Math.Abs(_firstCard.Rank - _secondCard.Rank);
        }

        public bool IsPocketPair()
        {
            return (_firstCard.Rank == _secondCard.Rank);
        }

        public override bool Equals(object obj)
        {
            if ((Object)obj == null)
                return false;

            return Equals(obj as StartingHandCombo);
        }

        public virtual bool Equals(StartingHandCombo hand)
        {
            if ((Object)hand == null)
                return false;

            return Equals(hand, MatchingMode.ExactSuits, true);
        }
        
        /// <summary>
        /// Determines if this StartingHand has the same value as another StartingHand with the selected matching mode.
        /// </summary>
        /// <param name="hand">Hand to be compared to.</param>
        /// <param name="mode"><para>If ExactSuits then AsKc != AcKs.</para>
        /// <para>If Suitedness then AKs == AKs, AKo == AKo, 99 == 99, but AKs != AKo.</para>
        /// If RankOnly then AsKs == KcAh.</param>
        /// <param name="ignoreOrder">If true, AK == KA.</param>
        /// <returns>True if the hands are equal using the selected matching mode.</returns>
        public virtual bool Equals(StartingHandCombo hand, MatchingMode mode, bool ignoreOrder = true)
        {            
            if ((Object)hand == null) return false;
            
            //Matching mode not recognized
            if (Enum.IsDefined(typeof(MatchingMode), mode) == false) return false;
            
            bool exactMatch = ((_firstCard.Equals(hand.FirstCard) && _secondCard.Equals(hand.SecondCard)) ||
                (ignoreOrder && _firstCard.Equals(hand.SecondCard) && _secondCard.Equals(hand.FirstCard)));

            if (exactMatch) return true;

            bool rankMatch = (hand.FirstCard.Rank == _firstCard.Rank && hand.SecondCard.Rank == _secondCard.Rank) ||
              (ignoreOrder && hand.FirstCard.Rank == _secondCard.Rank && hand.SecondCard.Rank == _firstCard.Rank);

            //The ranks must always be equal
            if (rankMatch == false) return false;

            //AKo == AKs
            if (mode == MatchingMode.RankOnly) return rankMatch;

            //AKo != AKs
            if (mode == MatchingMode.Suitedness) return (hand.IsSuited() == this.IsSuited());

            return false;
        }

        public override int GetHashCode()
        {
            //return HighCard.GetHashCode() ^ LowCard.GetHashCode();//AsKc equals KcAs so hash equality is correct
            return SortValue();
        }

        public static bool operator ==(StartingHandCombo left, StartingHandCombo right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null))
                return false;

            return (left.HighCard == right.HighCard && left.LowCard == right.LowCard);
        }

        public static bool operator !=(StartingHandCombo left, StartingHandCombo right)
        {
            return !(left == right);
        }

        
        /// <summary>
        /// Returns a unique value for each starting hand which ignores card order, and can be used for sorting.
        /// <para>Format RRrrSs</para>
        /// <para>AcKs == 131214 because A == 13, K == 12, c == 1, s == 4</para>
        /// </summary>
        /// <returns>A value to be used for sorting.</returns>
        protected int SortValue()
        {            
            return (int)this.HighCard.Rank * 10000 +
                   (int)this.LowCard.Rank * 100 +
                   (int)this.HighCard.Suit * 10 + 
                   (int)this.LowCard.Suit;
        }

        public int CompareTo(StartingHandCombo other)
        {
            if (this.Equals(other, MatchingMode.ExactSuits, true))
                return 0;
   
            return (this.SortValue() > other.SortValue()) ? 1 : -1;                                          
        }

        public static bool operator >(StartingHandCombo left, StartingHandCombo right)
        {            
            return left.CompareTo(right) == 1;
        }
        
        public static bool operator <(StartingHandCombo left, StartingHandCombo right)
        {
            return left.CompareTo(right) == -1;
        }

        public static bool operator <=(StartingHandCombo left, StartingHandCombo right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(StartingHandCombo left, StartingHandCombo right)
        {
            return left.CompareTo(right) >= 0;
        }

        public override string ToString()
        {
            return this.ToString(false);
        }

        public virtual string ToString(bool shortFormat, bool sorted = false)
        {
            string handName;
            if (sorted)
                handName = this.HighCard.ToString() + this.LowCard.ToString();
            else
                handName = _firstCard.ToString() + _secondCard.ToString();
            
            //AcKh == "AKo", KsAs == "AKs", 9s9h == "99"
            if (shortFormat)
            {
                handName = this.HighCard.ToString().Substring(0, 1) + this.LowCard.ToString().Substring(0, 1);

                if (this.IsSuited())
                    handName += "s";
                else if (this.IsPocketPair() == false)
                    handName += "o";                
            }

            return handName;
        }
    }
    
}
