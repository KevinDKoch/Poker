using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PokerLib2.Game
{
    public class WeightedStartingHandCombo : StartingHandCombo, IEquatable<WeightedStartingHandCombo>
    {
        protected readonly double _weight;
        public double Weight { get { return _weight; } }

        public WeightedStartingHandCombo(Card firstCard, Card secondCard, double weight)
            : base(firstCard, secondCard)
        {
            ValidateWeight(weight);
            _weight = weight;
        }

        public WeightedStartingHandCombo(string hand, double weight)
            : base(hand)
        {
            ValidateWeight(weight);
            _weight = weight;
        }

        private static string TrimWeight(string weightedHand)
        {
            try
            {
                return Regex.Replace(weightedHand, PokerRegex.weight + "$", String.Empty);
            }
            catch (RegexMatchTimeoutException e)
            {
                throw new ArgumentException("Regex timed out while triming the weight.", e);
            }
        }

        public WeightedStartingHandCombo(string hand)
            : base(TrimWeight(hand))
        {
            _weight = 1;
            if (hand.Length > 4)
            {
                string weight = hand.Substring(4);
                if (Regex.IsMatch(weight, PokerRegex.weight) == false)
                    throw new ArgumentException("Hand weight is not recognizable:" + weight);

                _weight = Convert.ToDouble(weight.Trim(new char[] { '(', ')' }));
                ValidateWeight(_weight);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is WeightedStartingHandCombo))
                return false;

            return base.Equals((WeightedStartingHandCombo)obj);
        }

        public bool Equals(WeightedStartingHandCombo other)
        {
            if ((Object)other == null)
                return false;

            if (base.Equals(other, MatchingMode.ExactSuits) == false)
                return false;

            return this.Weight == other.Weight;
        }

        /// <summary>
        /// Compares the value equality of this WeightedStartingHand to a StartingHand with the assumed weight of 1.
        /// </summary>
        /// <param name="other">The StartingHand being equated with.</param>
        /// <returns>True if equal, False otherwise.</returns>
        public override bool Equals(StartingHandCombo other)
        {
            if ((base.Equals(other, MatchingMode.ExactSuits) == false))
                return false;

            if (other is WeightedStartingHandCombo)
                return _weight == ((WeightedStartingHandCombo)other).Weight;

            return _weight == 1;
        }

        public override bool Equals(StartingHandCombo other, MatchingMode mode, bool ignoreOrder = true)
        {
            if (base.Equals(other, mode, ignoreOrder) == false)
                return false;

            if (other is WeightedStartingHandCombo)
                return _weight == ((WeightedStartingHandCombo)other).Weight;
            
            return _weight == 1;
        }

        public static bool operator ==(WeightedStartingHandCombo left, WeightedStartingHandCombo right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(WeightedStartingHandCombo left, WeightedStartingHandCombo right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected void ValidateWeight(double weight)
        {
            if (weight > 1 || weight < 0)
            {
                throw new ArgumentOutOfRangeException("The weight of a starting hand must be >= 0 and <= 1.");
            }
        }
        public override string ToString()
        {
            string weightStr = (this._weight != 1) ? ("(" + this._weight.ToString() + ")") : String.Empty;
            return base.ToString(false, false) + weightStr;
        }

        public override string ToString(bool shortFormat, bool sorted = false)
        {
            string weightStr = (this._weight != 1) ? ("(" + this._weight.ToString() + ")") : String.Empty;
            return base.ToString(shortFormat, sorted) + weightStr;
        }
    }   
}
