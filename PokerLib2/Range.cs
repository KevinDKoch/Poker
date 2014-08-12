using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PokerLib2
{
    public class Range:IList<WeightedStartingHand>, IEquatable<Range>
    {
        protected List<WeightedStartingHand> hands = new List<WeightedStartingHand>();
        public List<WeightedStartingHand> Hands { get { return this.hands; } set { this.hands = value; } }

        public string RangeString { get; private set; }

        public Range(List<WeightedStartingHand> hands)
        {            
            this.hands = hands;
        }

        public Range(string range)
        {

            if(ReferenceEquals(range, null))
                throw new ArgumentNullException(range, "range cannot be null.");

            StringBuilder errMsg = new StringBuilder();
            if (IsValidRange(range) == false)            
                throw new ArgumentException("Invalid Range: " + errMsg + ":" + range);

            this.RangeString = range;
            range = range.TrimStart('{').TrimEnd('}');
            range = Regex.Replace(range, @"\s", String.Empty);
            string[] tokens = range.Split(',');
            foreach (string tok in tokens)
            {
                string hand = tok;

                string suitedness = Regex.Match(hand, @"(?<=" + PokerRegex.rank + PokerRegex.rank + ")" + PokerRegex.suitedness).Value;
                //Get and trim the weight                
                double weight = 1;
                if (Regex.IsMatch(tok, PokerRegex.weight + "$")) 
                { 
                    weight = Convert.ToDouble(Regex.Match(hand, PokerRegex.weight).Value.TrimStart('(').TrimEnd(')'));
                    hand = Regex.Replace(tok, PokerRegex.weight + "$", string.Empty);
                }
                                
                if (Regex.IsMatch(hand, PokerRegex.RangeGroups.singleHand ))
                {
                    this.Add(new WeightedStartingHand(hand));
                }
                else if ((Regex.IsMatch(hand, PokerRegex.RangeGroups.handGroup)) || //Ex: AK,AKs,99  
                         (Regex.IsMatch(hand, PokerRegex.RangeGroups.wildGroup)))   //Ex: A*, *A, **, A*s, *Ao, **s
                {                                                             
                    List<Rank> firstCardRanks = new List<Rank>();
                    List<Rank> secondCardRanks = new List<Rank>();
                    if (hand.StartsWith("*"))
                        firstCardRanks = Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList();
                    else
                        firstCardRanks.Add(hand[0].ToRank());

                    
                    if (hand[1] =='*')
                        secondCardRanks = Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList();
                    else
                        secondCardRanks.Add(hand[1].ToRank());

                    //Create all suit combinations for each allowed rank combination and filter for suitedness
                    foreach (Rank firstRank in firstCardRanks)
                    {
                        foreach (Rank secondRank in secondCardRanks)
                        {
                            foreach (Suit s1 in (Suit[])Enum.GetValues(typeof(Suit)))
                            {
                                Card first = new Card(firstRank, s1);
                                foreach (Suit s2 in (Suit[])Enum.GetValues(typeof(Suit)))
                                {
                                    Card second = new Card(secondRank, s2);
                                    if (first.Equals(second) == false)
                                    {
                                        WeightedStartingHand newHand = new WeightedStartingHand(first, second, weight);
                                        if (this.hands.Contains(newHand) == false)
                                        {                   
                                            //Filter for suitedness
                                            if ((hand.EndsWith("s") && newHand.IsSuited()) ||
                                               (hand.EndsWith("o") && !newHand.IsSuited() && newHand.FirstCard.Rank != newHand.SecondCard.Rank) || //PP's are offsuit, but can't be written as 99o
                                               (!Regex.IsMatch(hand, PokerRegex.suitedness))) //Groups that don't filter for suitedness
                                            {                                                
                                                this.Add(newHand);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Regex.IsMatch(hand, PokerRegex.RangeGroups.closedLinear))
                {                    
                    //Start with first hand group, then determine direction and iterate to the end hand group
                    StartingHand startHand = new StartingHand(hand[0] + "c" + hand[1] + "s");
                    string endHandStr = Regex.Match(hand, @"(?<=-).*").Value;
                    StartingHand endHand = new StartingHand(endHandStr[0] + "c" + endHandStr[1] + "s");                    
                    StartingHand temp = endHand;

                    //The start point should be the higher ranking group
                    if (startHand.HighCard.Rank < endHand.HighCard.Rank)
                    {
                        endHand = startHand;
                        startHand = temp;
                    }
                    else if (startHand.HighCard.Rank == endHand.HighCard.Rank &&
                             startHand.LowCard.Rank < endHand.LowCard.Rank)
                    {
                        endHand = startHand;
                        startHand = temp;
                    }
                    
                    //Starting with the larger ranking hand, traverse to the ending hand.
                    //Movement will be down, left-to-right, or diagonally down and right.
                    StartingHand curHand = startHand;
                    StartingHand prevHand = curHand;
                    int verticalStep = (startHand.HighCard.Rank == endHand.HighCard.Rank) ? 0 : -1;
                    int horizontalStep = (startHand.LowCard.Rank == endHand.LowCard.Rank) ? 0 : -1;
                    do
                    {
                        Range newHands = new Range(curHand.HighCard.Rank.ToLetter() +
                                                   curHand.LowCard.Rank.ToLetter() +
                                                   suitedness + "(" + weight.ToString() + ")");
                        this.AddRange(newHands.Hands);

                        //Get the next hand group
                        Rank nextHiRank = curHand.HighCard.Rank + verticalStep;
                        Rank nextLoRank = curHand.LowCard.Rank + horizontalStep;
                        prevHand = curHand;
                        //Make sure the next hand is possible
                        if (Enum.IsDefined(typeof(Rank) , nextHiRank)==false || Enum.IsDefined(typeof(Rank), nextLoRank) == false) 
                            break;
                        curHand = new StartingHand(nextHiRank.ToLetter() + "c" + nextLoRank.ToLetter() + "s");
                    } while (prevHand.Equals(endHand) == false);

                }//22+, 76+, JTs+, TT-, 76-, AKo-
                else if (Regex.IsMatch(hand, PokerRegex.RangeGroups.openLinear))
                {
                    //Determine the end hand group and recursively call the range constuctor using the closedLinear format
                    //Ex: TT+ == AA-TT
                    StartingHand startHand = new StartingHand(hand[0] + "c" + hand[1] + "d");
                    Range newHands = null;
                    if (hand.EndsWith("+"))
                    {
                        //Ex: 77+
                        if (startHand.IsPocketPair())
                            newHands = new Range(hand[0].ToString() + hand[1].ToString() + "-" + "AA(" + weight + ")");
                        else//Ex: 76s+
                            newHands = new Range(hand[0].ToString() + hand[1].ToString() + suitedness + "-A" + ('A'.ToRank() - startHand.Gap()).ToLetter() + suitedness + "(" + weight + ")");

                        this.AddRange(newHands.Hands);
                    }
                    else
                    {
                        //Ex: 77-
                        if (startHand.IsPocketPair())
                            newHands = new Range(hand[0].ToString() + hand[1].ToString() + "-" + "22(" + weight + ")");
                        else//76s-
                            newHands = new Range(hand[0] + hand[1] + suitedness + "-2" + ('2'.ToRank() + startHand.Gap()).ToLetter() + suitedness + "(" + weight + ")");

                        this.AddRange(newHands.Hands);
                    }                                       
                }
            }
        }

        /// <summary>
        /// Returns true if the range is valid.
        /// </summary>
        /// <param name="range">The string representation of a range.</param>
        /// <param name="errorMsg">Contains any error message pertaining to invalid ranges.</param>
        /// <returns>True if the range is valid.</returns>
        public static bool IsValidRange(string range, StringBuilder errorMsg = null)
        {
            if (string.IsNullOrEmpty(range))
            {
                errorMsg.Append("Range cannot be null or empty.");
                return false;   
            }
                
            if(errorMsg == null) errorMsg = new StringBuilder();

            bool foundMatch = false;
            if (Regex.IsMatch(range, @"^{.*}$|^[^{].*[^}]$") == false)
            {
                errorMsg.Append("Ranges must start and end with brackets, or contain none at all");
                return false;
            }
                
            range = range.TrimStart('{').TrimEnd('}');            
            range = Regex.Replace(range, @"\s", String.Empty);
            
            foreach (string token in range.Split(','))
            {
                foundMatch = false; //Reset for this next token
                //Trim the weight from the token.
                //If the weight is invalid, then it will remain and the hand will fail to match anything
                string hand = Regex.Replace(token, PokerRegex.weight + "$", string.Empty);

                string errDuplicate = "Hand grouping matches multiple definitions:" + hand;
                
                //Ex: AcKs
                if (Regex.IsMatch(hand, PokerRegex.RangeGroups.singleHand))
                {
                    if (foundMatch)
                    {
                        errorMsg.Append(errDuplicate);
                        return false; 
                    }                        
                    else
                        foundMatch = true;                
                }

                //Ex: AK, AKs, AKo
                if (Regex.IsMatch(hand, PokerRegex.RangeGroups.handGroup))
                {
                    if (foundMatch)
                    {
                        errorMsg.Append(errDuplicate);
                        return false; 
                    }                        
                    else
                        foundMatch = true;
                }

                //Ex: *As, A*, **, **s
                if (Regex.IsMatch(hand, PokerRegex.RangeGroups.wildGroup))
                {
                    if (foundMatch)
                    {
                        errorMsg.Append(errDuplicate);
                        return false;
                    }                        
                    else
                        foundMatch = true;
                }

                //Ex: 77+, 76s+, 54+, AKo-, TT-
                if (Regex.IsMatch(hand, PokerRegex.RangeGroups.openLinear))
                {
                    if (foundMatch)
                    {
                        errorMsg.Append(errDuplicate);
                        return false;
                    }
                    else                    
                        foundMatch = true;
                }

                //Ex: AA-TT, JTs-76s, JT-AK
                if (Regex.IsMatch(hand, PokerRegex.RangeGroups.closedLinear))
                {                    
                    StartingHand startHand = new StartingHand(hand[0] + "c" + hand[1] + "s");
                    string endHandStr = Regex.Match(hand, @"(?<=-).*").Value;
                    StartingHand endHand = new StartingHand(endHandStr[0] + "c" + endHandStr[1] + "s");

                    //Do the hand groups make a line?
                    if ((startHand.HighCard.Rank == endHand.HighCard.Rank || startHand.LowCard.Rank == endHand.LowCard.Rank) || //Horizontal and Vertical
                        startHand.HighCard.Rank - endHand.HighCard.Rank == startHand.LowCard.Rank - endHand.LowCard.Rank)//Diagonal
                    {
                        if (foundMatch)
                        {
                            errorMsg.Append(errDuplicate);
                            return false;
                        }
                        else
                            foundMatch = true;
                    }
                    else
                    {
                        errorMsg.Append("Hand grouping does not form a line:" + hand);
                        return false;
                    }
                }
                
                if (foundMatch == false)
                {
                    errorMsg.Append("Unknown hand group:" + hand);
                    return false;
                }
            }
            
            return foundMatch;           
        }

        /// <summary>
        /// Returns the combos after factoring the weight.
        /// <para>AKs(.5) = 2 combos</para>
        /// </summary>
        /// <returns>The number of combinations after adjusting for weight.</returns>
        public double Combos()
        {
            double totalCombos = 0;

            foreach (WeightedStartingHand hand in this.hands)
            {
                totalCombos += hand.Weight;
            }

            return totalCombos;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool highCardsFirst)
        {
            string rangeStr = "{";
            foreach (WeightedStartingHand hand in this.hands)
            {
                rangeStr += hand.ToString(false, highCardsFirst);
                rangeStr += ",";
            }
            rangeStr = rangeStr.TrimEnd(',') + "}";

            return rangeStr;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Range == false)
                return false;

            return Equals((Range)obj);
        }

        public bool Equals(Range other)
        {
            if ((Object)other == null)
                return false;

            if(this.hands.Count != other.Count)
                return false;

            foreach (WeightedStartingHand hand in other.Hands)
            {
                if (this.Hands.Contains(hand) == false)
                    return false;
            }

            return true;
        }        

#region IList Implementation

        public int IndexOf(WeightedStartingHand item)
        {
            return this.hands.IndexOf(item);
        }

        public void Insert(int index, WeightedStartingHand item)
        {
            this.hands.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.hands.RemoveAt(index);
        }

        public WeightedStartingHand this[int index]
        {
            get
            {
                return this.hands[index];
            }
            set
            {
                this.hands[index] = value;
            }
        }

        public void Clear()
        {
            this.hands.Clear();
        }

        public bool Contains(WeightedStartingHand item)
        {
            return this.hands.Contains(item);            
        }

        public void CopyTo(WeightedStartingHand[] array, int arrayIndex)
        {
            this.hands.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get {return this.hands.Count;}
        }

        public bool IsReadOnly
        {
            get { return ((IList<WeightedStartingHand>)this.hands).IsReadOnly; }
        }

        public bool Remove(WeightedStartingHand item)
        {
            return this.hands.Remove(item);
        }

        public IEnumerator<WeightedStartingHand> GetEnumerator()
        {
            return this.hands.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds a WeightedStartingHand to the range.  If a hand already exists, it is overwritten (with a potentially new weight).
        /// </summary>
        /// <param name="item">The hand to add.</param>
        public void Add(WeightedStartingHand item)
        {
            foreach (WeightedStartingHand h in this.hands)
            {                                
                if (((StartingHand)h) == ((StartingHand)item))
                {                    
                    this.hands.Remove(h);
                    break;
                }
            }
            
            this.hands.Add(item);
        }

        /// <summary>
        /// Adds a list of WeightStartingHands.  If a hand already exists, it is overwritten (with a potentially new weight).
        /// </summary>
        /// <param name="items">The range to add.</param>
        public void AddRange(List<WeightedStartingHand> items)
        {
            foreach (WeightedStartingHand hand in items)
            {
                this.Add(hand);
            }
        }
    }

    #endregion    
}
