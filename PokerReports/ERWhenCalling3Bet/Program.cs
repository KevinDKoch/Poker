using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Npgsql;
using System.Data;
using PokerLib2.Game;
using PokerLib2.Reports;
using MathNet.Numerics.Statistics;

namespace EquityRealizedWhenCalling3Bet
{
    class Program
    {
        static void Main(string[] args)
        {
            ERWhenCalling3Bet report = new ERWhenCalling3Bet();

            Range rangeFilter = new Range("AK,AQ,AJ,AT,A9,KQ,KJ,KT,K9,QJ,QT,Q9,JT");
            foreach (StartingHandData<ERWhenCalling3BetData> hand in report.Result)
            {
                if (rangeFilter.Contains(hand.Combos[0]))
                {
                    double mean = hand.Data.EquityRealized.ToArray().Mean();
                    double variance = hand.Data.EquityRealized.ToArray().Variance();
                    double stdDev = hand.Data.EquityRealized.ToArray().StandardDeviation();

                    double critValue = 1.96;
                    double stdErr = stdDev / Math.Sqrt(hand.Data.EquityRealized.Count);
                    double nonNormalCI = critValue * stdErr;

                    Console.Write(hand.Name + " u = " + mean.ToString("N2") + " Non-Normal: +/- " + nonNormalCI.ToString("N2"));                    
                    Console.Write(" var = " + variance.ToString("N2"));
                    Console.Write(" stdDev = " + stdDev.ToString("N2"));
                    Console.WriteLine();
                }
            }

            report.Result.SaveCSV(@"E:\Documents\Poker\Reports\ERWhenCalling3Bet" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss") + ".csv");

            //TODO Smooth the equities by using some sort of best fit curve

            Console.ReadLine();            


        }
    }
}
