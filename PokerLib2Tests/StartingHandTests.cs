using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PokerLib2;
using System.Windows.Forms;
using System.Linq;

namespace PokerLib2Tests
{


    [TestClass]
    public class StartingHandTest
    {
        private static Dictionary<string, StartingHand> _startingHandCombos;
        private static Dictionary<string, StartingHand> _startingHandPermutations;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            _startingHandCombos = new Dictionary<string, StartingHand>();
            _startingHandPermutations = new Dictionary<string, StartingHand>();

            //All starting hands match a regular expression after ToString is called
            List<Card> cards = new List<Card>();
            foreach (Rank r in (Rank[])Enum.GetValues(typeof(Rank)))
            {
                foreach (Suit s in (Suit[])Enum.GetValues(typeof(Suit)))
                {
                    cards.Add(new Card(r, s));
                }
            }

            for (int iFirstCard = 0; iFirstCard < 52; iFirstCard++)
            {
                for (int iSecondCard = iFirstCard + 1; iSecondCard < 52; iSecondCard++)
                {
                    StartingHand hand = new StartingHand(cards[iFirstCard], cards[iSecondCard]);
                    _startingHandCombos.Add(hand.ToString(false), hand);
                    _startingHandPermutations.Add(hand.ToString(), hand);

                    StartingHand handOppOrder = new StartingHand(cards[iSecondCard], cards[iFirstCard]);
                    _startingHandPermutations.Add(handOppOrder.ToString(), handOppOrder);
                }
            }

            Assert.IsTrue(_startingHandCombos.Count == (52 * 51) / 2, "Incorrect number of combo's.");
            Assert.IsTrue(_startingHandPermutations.Count == 52 * 51, "Incorrect number of permutations.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_DuplicateCards_Throws()
        {
            StartingHand hand = new StartingHand(new Card(Rank.Ace, Suit.Diamonds), new Card("AsKd"));
        }

        [TestMethod]        
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EmptyString_Throws()
        {
            StartingHand hand = new StartingHand("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullInsteadofString_Throws()
        {
            StartingHand hand = new StartingHand(null);
        }

        [TestMethod]        
        public void Constructor_MalformedHandString_CatchesSoItPasses()
        {
            //Invalid Capitolization
            InvalidConstructorString("asKd");
            InvalidConstructorString("ASKd");
            InvalidConstructorString("Askd");
            InvalidConstructorString("AsKD");

            //Invalid Whitespace
            InvalidConstructorString("As Kd");
            InvalidConstructorString("AsK ");
            InvalidConstructorString("As d");
            InvalidConstructorString("A Kd");            
            InvalidConstructorString(" sKd");

            //Invalid Length
            InvalidConstructorString("AsKdd");
            InvalidConstructorString("AsKd ");
            InvalidConstructorString(" AsKd");
            InvalidConstructorString("AssKd");

            //Invalid capitolization
            InvalidConstructorString("askd");
            InvalidConstructorString("ASKD");

            //Duplicate Cards
            InvalidConstructorString("9s9s");
            InvalidConstructorString("KdKd");
           
            //Short Hand Format is used
            InvalidConstructorString("99s");
            InvalidConstructorString("99o");
            InvalidConstructorString("99");
            InvalidConstructorString("AK");
            InvalidConstructorString("AKo");
            InvalidConstructorString("AKs");

        }

        public void InvalidConstructorString(string test)
        {
            try
            {
                StartingHand hand = new StartingHand(test);
                Assert.Fail("This hand should have thrown an exception:" + test);
            }
            catch (ArgumentException e)
            {
                
            }
        }

        [TestMethod]
        public void Gap_TestGapsForVariousHands_Passes()
        {
            Assert.AreEqual(0, new StartingHand("9s9c").Gap());
            Assert.AreEqual(1, new StartingHand("7s6c").Gap());
            Assert.AreEqual(1, new StartingHand("6s7c").Gap());

            Assert.AreEqual(5, new StartingHand("7s2c").Gap());
            Assert.AreEqual(2, new StartingHand("AsQc").Gap());
            Assert.AreEqual(5, new StartingHand("2s7c").Gap());
            Assert.AreEqual(2, new StartingHand("QsAc").Gap());

        }


        [TestMethod]
        public void ToString_Matches_Expected_Regex()
        {
            Regex startingHandLongName = new Regex(@"^[2-9,A,K,Q,J,T][c,d,h,s][2-9,A,K,Q,J,T][c,d,h,s]$");
            Regex startingHandShortName = new Regex(@"^([2-9,A,K,Q,J,T][2-9,A,K,Q,J,T][o,s]$)|([22,33,44,55,66,77,88,99,TT,JJ,QQ,KK,AA]$)");

            foreach (StartingHand hand in _startingHandCombos.Values)
            {
                Assert.IsTrue(startingHandLongName.IsMatch(hand.ToString()), "Incorrect Hand Long Format found in Combos:"+hand.ToString());
                Assert.IsTrue(startingHandLongName.IsMatch(hand.ToString(false)), "Incorrect Hand Long Format found in Combos:" + hand.ToString(false));
                Assert.IsTrue(startingHandShortName.IsMatch(hand.ToString(true)), "Incorrect Hand Short Format found in Combos:" + hand.ToString(true));

                //Verify order in short format
                if(hand.SecondCard.Rank > hand.FirstCard.Rank)
                {
                    StartingHand orderedHand = new StartingHand(hand.SecondCard, hand.FirstCard);

                    //First card should be bigger then the second
                    Assert.IsTrue(hand.ToString(true).Substring(0, 1) == orderedHand.FirstCard.ToString().Substring(0, 1));
                    //Either order should output the same when using the short format
                    Assert.IsTrue(hand.ToString(true) == orderedHand.ToString(true));
                    //Order should be different with the long format
                    Assert.IsTrue(hand.ToString(false) != orderedHand.ToString());

                    Assert.IsTrue(hand.ToString(false, true) == orderedHand.ToString(),"The sorted long format did not match:" + hand.ToString(false, true) + " != " + hand.ToString());                    
                }                
            }

            foreach (StartingHand hand in _startingHandPermutations.Values)
            {
                Assert.IsTrue(startingHandLongName.IsMatch(hand.ToString()), "Incorrect Hand Long Format found in Permutations:" + hand.ToString());
                Assert.IsTrue(startingHandLongName.IsMatch(hand.ToString(false)), "Incorrect Hand Long Format found in Permutations:" + hand.ToString(false));
                Assert.IsTrue(startingHandShortName.IsMatch(hand.ToString(true)), "Incorrect Hand Short Format found in Permutations:" + hand.ToString(true));

                //Verify order in simple format
                if (hand.SecondCard.Rank >= hand.FirstCard.Rank)
                {
                    StartingHand orderedHand = new StartingHand(hand.SecondCard, hand.FirstCard);

                    //First card should be bigger then the second
                    Assert.IsTrue(hand.ToString(true).Substring(0, 1) == orderedHand.FirstCard.ToString().Substring(0, 1));
                    //Either order should output the same when using the simple format
                    Assert.IsTrue(hand.ToString(true) == orderedHand.ToString(true));
                    //Order should be different with the long format
                    Assert.IsTrue(hand.ToString(false) != orderedHand.ToString());
                }                

            }

        }

        [TestMethod]
        public void Equals_EveryStartingHandHasCorrectMatchesForAllMatchingModesAndOrders_Passes()
        {
            foreach (StartingHand hand1 in _startingHandPermutations.Values)
            {
                int exactMatches = 0;
                int suitednessMatches = 0;
                int rankMatches = 0;

                int orderedExactMatches = 0;
                int orderedSuitednessMatches = 0;
                int orderedRankMatches = 0;

                foreach (StartingHand hand2 in _startingHandPermutations.Values)
                {
                    exactMatches += (hand1.Equals(hand2, StartingHand.MatchingMode.ExactSuits)) ? 1 : 0;
                    suitednessMatches += (hand1.Equals(hand2, StartingHand.MatchingMode.Suitedness)) ? 1 : 0;
                    rankMatches += (hand1.Equals(hand2, StartingHand.MatchingMode.RankOnly)) ? 1 : 0;

                    orderedExactMatches += (hand1.Equals(hand2, StartingHand.MatchingMode.ExactSuits, false)) ? 1 : 0;                    
                    if(hand1.Equals(hand2, StartingHand.MatchingMode.Suitedness, false))
                    {
                        orderedSuitednessMatches += 1;
                    }
                    orderedRankMatches += (hand1.Equals(hand2, StartingHand.MatchingMode.RankOnly, false)) ? 1 : 0;

                }

                Assert.AreEqual(2, exactMatches, hand1.ToString());
                Assert.AreEqual(1, orderedExactMatches, hand1.ToString());
                if (hand1.IsPocketPair())
                {
                    Assert.AreEqual(12, suitednessMatches, hand1.ToString());
                    Assert.AreEqual(12, rankMatches, hand1.ToString());

                    Assert.AreEqual(12, orderedSuitednessMatches, "Ordered Suitedness Match Count for " + hand1.ToString());
                    Assert.AreEqual(12, orderedRankMatches, hand1.ToString());
                }
                else if(hand1.IsSuited())
                {
                    Assert.AreEqual(8, suitednessMatches, hand1.ToString());
                    Assert.AreEqual(32, rankMatches, hand1.ToString());

                    Assert.AreEqual(4, orderedSuitednessMatches, hand1.ToString());
                    Assert.AreEqual(16, orderedRankMatches, hand1.ToString());
                }
                else
                {
                    Assert.AreEqual(24, suitednessMatches, hand1.ToString());
                    Assert.AreEqual(32, rankMatches, hand1.ToString());

                    Assert.AreEqual(12, orderedSuitednessMatches, hand1.ToString());
                    Assert.AreEqual(16, orderedRankMatches, hand1.ToString());
                }
            }
        }

        [TestMethod]
        public void Equals_Null_Test()
        {
            Assert.IsFalse(new StartingHand(new Card("As"), new Card("Ks")).Equals(null));
            Assert.IsFalse(new StartingHand(new Card("As"), new Card("Ks")).Equals(new Card("As")));
            Assert.IsFalse(new StartingHand(new Card("As"), new Card("Ks")).Equals(new StartingHand(new Card("As"), new Card("Ks")),(StartingHand.MatchingMode)9));
        }

        [TestMethod]
        public void ToString_CreatesExactOrderedOrginalHands_Passes()
        {
            foreach (StartingHand hand in _startingHandPermutations.Values)
            {
                StartingHand handFromString = new StartingHand(hand.ToString());
                Assert.IsTrue(hand.Equals(handFromString, StartingHand.MatchingMode.ExactSuits, true), "The hand created from ToString was not an exact match");
            }
        }

        [TestMethod]
        public void ComparisonOperators_CommonUsageWorksAsExpected_Passes()
        {
            Assert.IsTrue(new StartingHand("7s3c") > new StartingHand("2s7c"));
            Assert.IsTrue(new StartingHand("7s2c") < new StartingHand("3s7c"));

            Assert.IsFalse(new StartingHand("7s2c") < new StartingHand("7c2s"));
            Assert.IsFalse(new StartingHand("AsKc") < new StartingHand("7c2s"));

            Assert.IsTrue(new StartingHand("9s9h") > new StartingHand("9s9c"));
            Assert.IsTrue(new StartingHand("7s2c") < new StartingHand("3s7c"));
        }

        [TestMethod]
        public void CompareTo_ValidateSortingForAllPerms_Passes()
        {
            List<StartingHand> copy = new List<StartingHand>();
            List<StartingHand> sorted = new List<StartingHand>();
            List<StartingHand> shuffled = new List<StartingHand>();

            foreach (StartingHand h in _startingHandPermutations.Values)
            {
                copy.Add(h);
                sorted.Add(h);       
            }

            Random iRand = new Random();
            do{ 
                int i = iRand.Next(copy.Count - 1);
                shuffled.Add(copy[i]);
                copy.RemoveAt(i);
            }while(copy.Count > 0);

            Assert.IsFalse(EqualLists(sorted, shuffled));

            shuffled.Sort();
            sorted.Sort();

            Assert.IsTrue(EqualLists(sorted, shuffled));
        }

        public bool EqualLists(List<StartingHand> listA, List<StartingHand> listB)
        {
            if (listA.Count != listB.Count)
                return false;

            for(int i = 0; i < listA.Count; i++)
            {
                if (listA[i].Equals(listB[i], StartingHand.MatchingMode.ExactSuits) == false)
                    return false;
            }

            return true;
        }
        
        [TestMethod]
        public void HighCardAndLowCard_ConsistentEvenWithPPs_Pass()
        {
            StartingHand hand = new StartingHand("9s9c");
            StartingHand revHand = new StartingHand("9c9s");

            Assert.IsTrue(hand.Equals(revHand, StartingHand.MatchingMode.ExactSuits));
            Assert.IsFalse(hand.Equals(revHand, StartingHand.MatchingMode.ExactSuits, false));

            Assert.IsTrue(hand.HighCard == revHand.HighCard);
            Assert.IsTrue(hand.LowCard == revHand.LowCard);

            hand = new StartingHand("Th9d");
            revHand = new StartingHand("9dTh");

            Assert.IsTrue(hand.Equals(revHand, StartingHand.MatchingMode.ExactSuits));
            Assert.IsFalse(hand.Equals(revHand, StartingHand.MatchingMode.ExactSuits, false));

            Assert.IsTrue(hand.HighCard == revHand.HighCard);
            Assert.IsTrue(hand.LowCard == revHand.LowCard);
        }

    }
}
