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

namespace EquityRealizedWhenPFRCalled
{
    class Program
    {
        //private class ERWhenCalled
        //{
        //    public List<double> EquityRealized { get; set; }
        //    //public double[] EV { get; set; }

        //    public ERWhenCalled()
        //    {
        //        EquityRealized = new List<double>();
        //    }

        //    public void Add(double netWon, double PFRsize, double rake)
        //    {
        //        if (netWon > 0)
        //        {
        //            //Replace the rake when we win, because we want a true ER across stakes
        //            netWon += rake;
        //        }
        //        double ER = (netWon + PFRsize) / (PFRsize * 2);

        //        EquityRealized.Add(ER);
        //    }
        //}

        static void Main(string[] args)
        {
            
            ERWhenPFRCalled report = new ERWhenPFRCalled();

            Range rangeFilter = new Range("**o");
            foreach (StartingHandData<ERWhenPFRCalledData> hand in report.Result)
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
                    //Console.Write(" BCI +/- " + hand.Data.ER_CI().ToString("N2"));
                    Console.Write(" var = " + variance.ToString("N2"));
                    Console.Write(" stdDev = " + stdDev.ToString("N2"));
                    Console.WriteLine();
                }                    
            }
            
            report.Result.SaveCSV(@"E:\Documents\Poker\Reports\ERWhenPFRCalled" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss") + ".csv");
            
            //TODO Smooth the equities by using some sort of best fit curve

            Console.ReadLine();            
        }
    }
}
