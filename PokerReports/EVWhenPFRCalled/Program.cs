using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data;
using PokerLib2.HandHistory;
using PokerLib2.Game;
using System.Text.RegularExpressions;
using MathNet;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;


namespace EVWhenPFRCalled
{
    public class StartingHandGroup
    {
        public int Id_holecard { get; set; }
        public string Name { get; set; }
        private List<StartingHandGroupData> _hands = new List<StartingHandGroupData>();
        public List<StartingHandGroupData> Hands { get { return _hands; }  }

        public double TotalBBWon()
        {
            double totalBlinds = 0;
            foreach (StartingHandGroupData hand in _hands)
            {
                totalBlinds += hand.AmtBBWon;
            }

            return totalBlinds;
        }

        public double EquityRealized()
        {
            return (TotalBBWon() / _hands.Count) / (_hands[0].PotSizeOnFlop / 2);
        }

        public double EVWhenCalled()
        {
            return TotalBBWon() / _hands.Count + (_hands[0].PotSizeOnFlop / 2);
        }

        public double EVWhenCalledVariance()
        {
            double uEV = EVWhenCalled();            
            double totalSqrDevFromMean = 0;
            foreach (StartingHandGroupData hand in _hands)
            {                
                totalSqrDevFromMean += Math.Pow(hand.AmtBBWon + (_hands[0].PotSizeOnFlop / 2) - uEV, 2);
            }

            return totalSqrDevFromMean / (_hands.Count -1);
        }

        public double EVWhenCalledStdDev()
        {
            return Math.Sqrt(EVWhenCalledVariance());
        }

        public double EVWhenCalledCI()
        {
            double critValue = 1.96;
            double stdErr = EVWhenCalledStdDev() / Math.Sqrt(_hands.Count);
            return critValue * stdErr;
        }
    }

    public class StartingHandGroupData
    {
        public double AmtBBWon { get; private set; }
        public double PotSizeOnFlop { get; private set; }
        public double EquityRealized { get; private set; }


        public StartingHandGroupData(double amtBBWon, double potSizeOnFlop)
        {
            AmtBBWon = amtBBWon;
            PotSizeOnFlop = potSizeOnFlop;
            EquityRealized = amtBBWon / potSizeOnFlop;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {                                  
            //Connect to the DB
            string ConnString = "Server=127.0.0.1;Port=5434;User Id=postgres;Password=svcPASS83;Database=Main_PT4_DB;";
            NpgsqlConnection conn = new NpgsqlConnection(ConnString);
            conn.Open();

            if (conn.State == System.Data.ConnectionState.Open)            
                Console.WriteLine("Connected!");
            else
            {
                Console.WriteLine("OH NO, NOT CONNECTED!");
                return;
            }            
                
            //Load the hands
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = "SELECT (cash_hand_summary.id_hand) as \"id_hand\", (cash_hand_summary.id_site) as \"id_site\", (cash_hand_summary.hand_no) as \"hand_no\", (cash_hand_summary.id_gametype) as \"id_gametype_summary\", (cash_hand_player_statistics.holecard_1) as \"id_holecard1\", (cash_hand_player_statistics.holecard_2) as \"id_holecard2\", (cash_hand_player_statistics.holecard_3) as \"id_holecard3\", (cash_hand_player_statistics.holecard_4) as \"id_holecard4\", ( (CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_player_statistics.amt_won * 1.0 )/( cash_limit.amt_bb)) ELSE 0 END) ) as \"amt_bb_won\", (player_winner.player_name) as \"str_winner\", (cash_hand_summary.amt_rake) as \"amt_rake\", (cash_limit.limit_currency) as \"limit_currency\" FROM      lookup_actions lookup_actions_p, cash_hand_player_statistics , cash_hand_summary, player player_winner, cash_limit WHERE  (cash_hand_summary.id_hand = cash_hand_player_statistics.id_hand  AND cash_hand_summary.id_limit = cash_hand_player_statistics.id_limit)  AND (cash_limit.id_limit = cash_hand_player_statistics.id_limit)  AND (player_winner.id_player = cash_hand_summary.id_winner)  AND (cash_limit.id_limit = cash_hand_summary.id_limit)   AND (cash_hand_player_statistics.id_player = (SELECT id_player FROM player WHERE player_name_search='donktacular'  AND id_site='100'))   AND lookup_actions_p.id_action = cash_hand_player_statistics.id_action_p      AND ((cash_hand_player_statistics.id_gametype = 1)AND ((((((cash_hand_player_statistics.flg_blind_s)))))AND (((((cash_hand_summary.id_gametype = 1))AND ((cash_limit.flg_nl)))))AND (((((cash_hand_summary.cnt_players BETWEEN 2 and 2)))))AND ((NOT ((((((case when(char_length(lookup_actions_p.action) < 1) then '' else (substring(lookup_actions_p.action from 1 for 1)) end) = 'R'))AND ((lookup_actions_p.action LIKE '__%')))))))AND (((((cash_hand_player_statistics.flg_f_saw)))))AND (((((cash_hand_player_statistics.flg_p_first_raise AND ( (CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_player_statistics.amt_p_raise_made )/( cash_limit.amt_bb)) ELSE 0 END) ) BETWEEN 2.00 and 2.00)))))AND (((((cash_hand_player_statistics.id_gametype=1 AND cash_hand_player_statistics.id_holecard in (1))))))))  ORDER BY (timezone('UTC',  cash_hand_player_statistics.date_played  + INTERVAL '0 HOURS')) desc";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
            
            da.Fill(ds);
            dt = ds.Tables[0];

            //dgvSQLResults.DataSource = dt;
            Console.WriteLine(dt.Rows.Count);
                        
            List<StartingHandGroup> startingHands = new List<StartingHandGroup>();
            int handID = 1;
            for (Rank iRank1 = Rank.Ace; iRank1 >= Rank.Two; iRank1--)
            {
                for (Rank iRank2 = iRank1; iRank2 >= Rank.Two; iRank2--)
                {                    
                    if (iRank1 == iRank2)
                    {
                        startingHands.Add(new StartingHandGroup { Id_holecard = handID, Name = iRank1.ToLetter() + iRank2.ToLetter() });                        
                    }
                    else
                    {
                        startingHands.Add(new StartingHandGroup { Id_holecard = handID, Name = iRank1.ToLetter() + iRank2.ToLetter() + "s" });
                        handID++;
                        startingHands.Add(new StartingHandGroup { Id_holecard = handID, Name = iRank1.ToLetter() + iRank2.ToLetter() + "o" });
                    }
                    handID++;
                }
            }

            double PFRSize = 2;
            //Replace the PFR size in the SQL string
            //SQL portion to look for:
            //((CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_player_statistics.amt_p_raise_made )/( cash_limit.amt_bb)) ELSE 0 END) ) BETWEEN 2.50 and 2.50)
            sql = Regex.Replace(sql, @"(?<=\) BETWEEN )\d\.\d\d", PFRSize.ToString("N2"));
            sql = Regex.Replace(sql, @"(?<=\) BETWEEN \d\.\d\d and )\d\.\d\d", PFRSize.ToString("N2"));
            foreach (StartingHandGroup hg in startingHands)
            {
                //Regex looks for:.id_holecard in (1)
                sql = Regex.Replace(sql, @"(?<=\.id_holecard in \()\d+", hg.Id_holecard.ToString());

                //Console.WriteLine(hg.name + "=" + hg.id_holecard);
                //Query DB
                ds = new DataSet();
                dt = new DataTable();
                //da = new NpgsqlDataAdapter(sql, conn);                
                da.SelectCommand = new NpgsqlCommand(sql, conn);
                da.Fill(ds);
                dt = ds.Tables[0];
                
                foreach (DataRow row in dt.Rows)
                {
                    //Console.WriteLine(row["id_hand"].ToString());
                    //Store the amount won in bb's for each hand
                    StartingHandGroupData newHand = new StartingHandGroupData(Convert.ToDouble((row["amt_bb_won"])), PFRSize * 2);
                    hg.Hands.Add(newHand);
                }
                
                Console.WriteLine(hg.Name + " Realized: " + (hg.EquityRealized() * 100).ToString("N2") + " EV(When Called) = " + hg.EVWhenCalled().ToString("N2") + " +/- " + hg.EVWhenCalledCI().ToString("N2"));
            }

       
            //TODO Filter by excluding fish, which will probably be an entirely different project, or simply building a list of players to exclude hands from
            
            //Output EV and standard deviation stuff in a csv file
            //Combine results to some sort of meaningful average
            //Output summary CSV

            conn.Close();
            Console.ReadLine();
        }
    }
}
