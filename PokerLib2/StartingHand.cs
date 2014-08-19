using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2.Game
{
    /// <summary>
    /// Represents a Range that has the same rank and suitedness.
    /// </summary>
    public class StartingHand : Range
    {        

        public String Name { get; private set; }

        public bool IsPocketPair { get; private set; }
        public bool IsSuited { get; private set; }

        public Rank HighRank { get; private set; }
        public Rank LowRank { get; private set; }
   
        public StartingHand(string startingHand)
            : base(startingHand)
        {
            if (_combos.Count == 0)
                throw new ArgumentException("StartingHand must be created with combos.");

            Name = _combos[0].ToString(true, true);

            IsPocketPair = _combos[0].IsPocketPair();
            IsSuited = _combos[0].IsSuited();

            HighRank = _combos[0].HighCard.Rank;
            LowRank = _combos[0].LowCard.Rank;

            foreach (WeightedStartingHandCombo c in _combos)
                if (!c.Equals(_combos[0], StartingHandCombo.MatchingMode.Suitedness, true))
                    throw new ArgumentException("All combos of a StartingHand must have that same ranks and suitedness:" + _combos[0].ToString(true, true) + "!=" + c.ToString(true, true));
            
        }


        /// <summary>
        /// Adds a combo to the range.  If a combo already exists, it is overwritten (with a potentially new weight).  If the combo is part of this starting hand, then a exception is thrown.
        /// </summary>
        /// <param name="combo">The combo to add.</param>
        public override void Add(WeightedStartingHandCombo combo)
        {
            if (combo.HighCard.Rank == this.HighRank &&
                combo.LowCard.Rank == this.LowRank &&
                combo.IsSuited() == this.IsSuited)
            {
                AddCombo(combo);
            }
            else
            {
                throw new ArgumentException("All combos of a StartingHand must have that same ranks and suitedness:" + _combos[0].ToString(true, true) + "!=" + combo.ToString(true, true));
            }            
        }

        /// <summary>
        /// Adds a range of combos which will overwrite existing combo's if they are valid combo's of this starting hand.
        /// </summary>
        /// <param name="items">The range to add.</param>
        public override void AddRange(Range combos)
        {
            foreach (WeightedStartingHandCombo hand in combos)
            {                
                this.Add(hand);
            }
        }   
    }
}
