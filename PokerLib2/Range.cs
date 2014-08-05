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
                string hand = tok;

                //Get and trim the weight
                float weight = 1;
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
                        firstCardRanks.Add(hand[0].ToRank());

                    if (hand[1] =='*')
                        secondCardRanks = Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList();
                    else
                        secondCardRanks.Add(hand[1].ToRank());

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
                    
                    //Start with first hand group, then determine direction and iterate to the end hand group

                    //StartingHand startHand = new StartingHand(Regex.Match(hand, @".*(?=-)").Value);
                    //StartingHand endHand = new StartingHand(Regex.Match(hand, @"(?=>-).*").Value);
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
                    
                    //A5-A2
                    //99-22
                    //JT-76
                    //A2-72
                    string suitedness = (Regex.IsMatch(hand, PokerRegex.suitedness + "$")) ? hand[hand.Length - 1].ToString() : string.Empty;
                    StartingHand curHand = startHand;
                    StartingHand prevHand = curHand;
                    int horizontalStep = (startHand.LowCard.Rank == endHand.LowCard.Rank) ? 0 : -1;
                    int verticalStep = (startHand.HighCard.Rank == endHand.HighCard.Rank) ? 0 : -1;
                    do
                    {
                        Range newHands = new Range(curHand.HighCard.Rank.ToChar().ToString() +
                                                   curHand.LowCard.Rank.ToChar().ToString() +
                                                   suitedness + "(" + weight.ToString() + ")");
                        _hands.AddRange(newHands.Hands);

                        Rank nextHiRank = curHand.HighCard.Rank + verticalStep;
                        Rank nextLoRank = curHand.LowCard.Rank + horizontalStep;
                        prevHand = curHand;
                        if (Enum.IsDefined(typeof(Rank) , nextHiRank)==false || Enum.IsDefined(typeof(Rank), nextLoRank) == false) 
                            break; //The next hand in this line does not exist
                        curHand = new StartingHand(nextHiRank.ToChar() + "c" + nextLoRank.ToChar() + "s");
                    } while (prevHand.Equals(endHand) == false);

                }
            }
        }

        //
        protected List<WeightedStartingHand> BuildCombos(string handGroup)
        {
            List<WeightedStartingHand> combos = new List<WeightedStartingHand>();
            

            return combos;
        }
        
        public static bool IsValidRange(string range, StringBuilder errorMsg = null)
        {            
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

                string errDuplicate = "Hand grouping matches multiple definitions.";
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

                if (Regex.IsMatch(hand, PokerRegex.RangeGroups.closedLinear))
                {
                    
                    //StartingHand startHand = new StartingHand(Regex.Match(hand, @".*(?=-)").Value);
                    StartingHand startHand = new StartingHand(hand[0] + "c" + hand[1] + "s");
                    //StartingHand endHand = new StartingHand(Regex.Match(hand, @"(?=>-).*").Value);
                    string endHandStr = Regex.Match(hand, @"(?<=-).*").Value;
                    StartingHand endHand = new StartingHand(endHandStr[0] + "c" + endHandStr[1] + "s");

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
