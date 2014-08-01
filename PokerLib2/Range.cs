using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PokerLib2
{
    public class Range
    {
        protected List<WeightedStartingHand> _hands = new List<WeightedStartingHand>();
        public List<WeightedStartingHand> Hands { get { return _hands; } set { _hands = value; } }

        //protected List<string> _validRangeGroupings = new List<string>();

        public Range(List<WeightedStartingHand> hands)
        {
            _hands = hands;
        }

        public Range(string range)
        {
            if (string.IsNullOrEmpty(range))            
                throw new ArgumentNullException("The range string cannot be null or empty.");

            if (IsValidRange(range) == false)            
                throw new ArgumentException("Invalid Range:" + range);

            range = range.TrimStart('{').TrimEnd('}');
            range = Regex.Replace(range, @"\s", String.Empty);
            string[] tokens = range.Split(',');
            foreach (string tok in tokens)
            {
                float weight = 1;
                string hand = tok;
                if (Regex.IsMatch(tok, PokerRegex.weight + "$")) 
                { 
                    weight = Convert.ToSingle(Regex.Match(hand, PokerRegex.weight).Value.TrimStart('(').TrimEnd(')'));
                    hand = Regex.Replace(tok, PokerRegex.weight + "$", string.Empty);
                }
                                
                if (Regex.IsMatch(hand, PokerRegex.RangeGroups.singleHand ))
                {
                    _hands.Add(new WeightedStartingHand(hand));
                }
                else if ((Regex.IsMatch(hand,  PokerRegex.RangeGroups.handGroup )) ||
                         (Regex.IsMatch(hand,  PokerRegex.RangeGroups.wildGroup )))
                {   //AK,AKs,99                                        
                    //A*, *A, **, A*s, *Ao, **s
                    List<Rank> firstCardRanks = new List<Rank>();
                    List<Rank> secondCardRanks = new List<Rank>();;
                    if (hand.StartsWith("*"))
                        firstCardRanks = Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList();
                    else
                        firstCardRanks.Add(new Card(hand[0] + "s").Rank);

                    if (hand[1] =='*')
                        secondCardRanks = Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList();
                    else
                        secondCardRanks.Add(new Card(hand[1] + "s").Rank);

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
                                        if (_hands.Contains(newHand) == false)
                                        {
                                            if ((hand.EndsWith("s") && newHand.IsSuited()) ||
                                               (hand.EndsWith("o") && !newHand.IsSuited() && newHand.FirstCard.Rank != newHand.SecondCard.Rank) ||
                                               (!Regex.IsMatch(hand, PokerRegex.suitedness)))
                                            {                                                
                                                _hands.Add(newHand);
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
                    //TODO: Build ClosedLinear group
                }
            }
        }

        public static bool IsValidRange(string range)
        {
            bool foundMatch = false;
            range = range.TrimStart('{').TrimEnd('}');            
            range = Regex.Replace(range, @"\s", String.Empty);
            
            foreach (string token in range.Split(','))
            {
                foundMatch = false; //Reset for this next token
                //Trim the weight from the token.
                //If the weight is invalid, then it will remain and the hand will fail to match anything
                string hand = Regex.Replace(token, PokerRegex.weight + "$", string.Empty);
                
                if (Regex.IsMatch(hand, PokerRegex.RangeGroups.singleHand))
                {
                    if (foundMatch)
                        return false; //Only one group can match
                    else
                        foundMatch = true;                
                }

                if (Regex.IsMatch(hand, PokerRegex.RangeGroups.handGroup))
                {
                    if (foundMatch)
                        return false; //Only one group can match
                    else
                        foundMatch = true;
                }

                if (Regex.IsMatch(hand, PokerRegex.RangeGroups.wildGroup))
                {
                    if (foundMatch)
                        return false; //Only one group can match
                    else
                        foundMatch = true;
                }

                if (Regex.IsMatch(hand, PokerRegex.RangeGroups.openLinear))
                {
                    if (foundMatch)
                        return false; //Only one group can match
                    else
                        foundMatch = true;
                }

                if (Regex.IsMatch(hand, PokerRegex.RangeGroups.closedLinear))
                {
                    //Verify groups forms a line(s)
                    Rank hiRankStart = new Card(hand[0] + "s").Rank;
                    Rank loRankStart = new Card(hand[1] + "s").Rank;
                    if (loRankStart > hiRankStart)
                    {
                        Rank temp = hiRankStart;
                        hiRankStart = loRankStart;
                        loRankStart = temp;
                    }

                    int iDash = hand.IndexOf('-');
                    Rank hiRankEnd = new Card(hand[iDash+1] + "s").Rank;
                    Rank loRankEnd = new Card(hand[iDash+2] + "s").Rank;
                    if (loRankEnd > hiRankEnd)
                    {
                        Rank temp = hiRankEnd;
                        hiRankEnd = loRankEnd;
                        loRankEnd = temp;
                    }

                    if ((hiRankStart == hiRankEnd || loRankStart == loRankEnd) || //Horizontal and Vertical
                        hiRankStart - hiRankEnd == loRankStart - loRankEnd)//Diagonal
                    {
                        if (foundMatch)
                            return false; //Only one group can match
                        else
                            foundMatch = true;
                    }
                }                                

                

            }

            return foundMatch;           
        }

        public float Combos()
        {
            float totalCombos = 0;

            foreach (WeightedStartingHand hand in _hands)
            {
                totalCombos += hand.Weight;
            }

            return totalCombos;
        }

        public override string ToString()
        {
            string rangeStr = "{";
            foreach (WeightedStartingHand hand in _hands)
            {
                rangeStr += hand.ToString(false, true);
                if( hand.Weight < 1)
                    rangeStr += "(" + hand.Weight + ")";
                rangeStr += ",";
            }
            rangeStr = rangeStr.TrimEnd(',') + "}";

            return rangeStr;
        }
    }
}
