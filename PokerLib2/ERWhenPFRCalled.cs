using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using PokerLib2.Game;
using Npgsql;
using MathNet.Numerics.Statistics;


namespace PokerLib2.Reports
{
    public class ERWhenPFRCalledData:ICSVReport
    {
        public List<double> EquityRealized { get; set; }

        public int TotalHands { get { return EquityRealized.Count; } }

        public ERWhenPFRCalledData()
        {
            EquityRealized = new List<double>();
        }

        public void Add(double netWon, double PFRsize, double rake)
        {            
            if (netWon > 0)
            {
                //Replace the rake when we win, because we want a true ER across stakes and different rake structures
                netWon += rake;
            }
            double ER = (netWon + PFRsize) / (PFRsize * 2);

            EquityRealized.Add(ER);
        }

        public double ER_CI()
        {
            //TODO Figure out why these CI as so narrow, perhaps I'm doing the bootstrapping wrong...
            int totalResamples = 1000;
            double [][]resamples = new double[totalResamples][];

            double[] resampleMeans = new double[totalResamples];
            //Create a double array of size resamples
            Random rand = new Random();
            for (int iResample = 0; iResample < totalResamples; iResample++)
            {
                resamples[iResample] = new double[EquityRealized.Count];
                for (int iSample = 0; iSample < EquityRealized.Count; iSample++)
                {
                    int randSample = rand.Next(0,EquityRealized.Count-1);
                    resamples[iResample][iSample] = EquityRealized[randSample];                    
                }
                resampleMeans[iResample] = resamples[iResample].Mean();
            }

            double critValue = 1.96;
            double stdErr = resampleMeans.StandardDeviation() / Math.Sqrt(totalResamples);
            double CI = critValue * stdErr;

            return CI;
        }

        public string CSV()
        {
            return EquityRealized.Mean().ToString();
        }

        public string CSVHeaders()
        {
            return "ER";
        }
    }

    public class ERWhenPFRCalled : StartingHandReport<ERWhenPFRCalledData>
    {

        public ERWhenPFRCalled()
            : base()
        {
            BuildDataTable();
        }

        public ERWhenPFRCalled(string connString)
            : base(connString)
        {
            BuildDataTable();
        }

        public ERWhenPFRCalled(DataTable data)
            : base(data)
        {
            BuildReport(data);
        }

        private void BuildDataTable()
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connString);
            conn.Open();

            if (conn.State == System.Data.ConnectionState.Open)
                Console.WriteLine("Connected!");
            else
            {
                Console.WriteLine("NOT CONNECTED!");
                //TODO Throw exception for failed connection?
                return;
            }
            
            DataTable data = new DataTable();
            //For each common PFR size
            double[] PFRSize = { 2, 2.5, 3 };
            for (int i = 0; i < PFRSize.Length; i++)
            {
                Console.WriteLine("PFR Size:" + PFRSize[i]);

                //( (CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_summary.amt_rake)/( cash_limit.amt_bb)) ELSE 0 END) ) as \"amt_rake\"
                string query = "SELECT (cash_hand_summary.id_hand) as \"id_hand\", (cash_hand_summary.id_site) as \"id_site\", (cash_hand_summary.hand_no) as \"hand_no\", (cash_hand_summary.id_gametype) as \"id_gametype_summary\", (cash_hand_player_statistics.holecard_1) as \"id_holecard1\", (cash_hand_player_statistics.holecard_2) as \"id_holecard2\", (cash_hand_player_statistics.holecard_3) as \"id_holecard3\", (cash_hand_player_statistics.holecard_4) as \"id_holecard4\", ( (CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_player_statistics.amt_won * 1.0 )/( cash_limit.amt_bb)) ELSE 0 END) ) as \"amt_bb_won\", (player_winner.player_name) as \"str_winner\",( (CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_summary.amt_rake)/( cash_limit.amt_bb)) ELSE 0 END) ) as \"amt_rake\", (cash_limit.limit_currency) as \"limit_currency\" FROM      lookup_actions lookup_actions_p, cash_hand_player_statistics , cash_hand_summary, player player_winner, cash_limit WHERE  (cash_hand_summary.id_hand = cash_hand_player_statistics.id_hand  AND cash_hand_summary.id_limit = cash_hand_player_statistics.id_limit)  AND (cash_limit.id_limit = cash_hand_player_statistics.id_limit)  AND (player_winner.id_player = cash_hand_summary.id_winner)  AND (cash_limit.id_limit = cash_hand_summary.id_limit)   AND (cash_hand_player_statistics.id_player = (SELECT id_player FROM player WHERE player_name_search='donktacular'  AND id_site='100'))   AND lookup_actions_p.id_action = cash_hand_player_statistics.id_action_p      AND ((cash_hand_player_statistics.id_gametype = 1)AND ((((((cash_hand_player_statistics.flg_blind_s)))))AND (((((cash_hand_summary.id_gametype = 1))AND ((cash_limit.flg_nl)))))AND (((((cash_hand_summary.cnt_players BETWEEN 2 and 2)))))AND ((NOT ((((((case when(char_length(lookup_actions_p.action) < 1) then '' else (substring(lookup_actions_p.action from 1 for 1)) end) = 'R'))AND ((lookup_actions_p.action LIKE '__%')))))))AND (((((cash_hand_player_statistics.flg_f_saw)))))AND (((((cash_hand_player_statistics.flg_p_first_raise AND ( (CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_player_statistics.amt_p_raise_made )/( cash_limit.amt_bb)) ELSE 0 END) ) BETWEEN 2.00 and 2.00)))))))  ORDER BY (timezone('UTC',  cash_hand_player_statistics.date_played  + INTERVAL '0 HOURS')) desc";
                //Use the correct PFR size                
                query = Regex.Replace(query, @"(?<=\) BETWEEN )\d\.\d\d", PFRSize[i].ToString("N2"));
                query = Regex.Replace(query, @"(?<=\) BETWEEN \d\.\d\d and )\d\.\d\d", PFRSize[i].ToString("N2"));

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                //Load the hands                                
                da.Fill(ds);
                dt = ds.Tables[0];

                //Add column and assign it the value of the current PFR size                
                dt.Columns.Add("PFRSize", typeof(System.Double));
                foreach (DataRow row in dt.Rows)                
                    row["PFRSize"] = PFRSize[i];
                
                data.Merge(dt);
                Console.WriteLine(" Hands: " + dt.Rows.Count);
            }
            conn.Close();

            BuildReport(data);
        }

        private void BuildReport(DataTable data)
        {
            Console.Write("Starting to build report data...");
            //For each Row                
            foreach (DataRow row in data.Rows)
            {                
                //What hand is this row for?
                int firstCardID = Convert.ToInt32(row["id_holecard1"]);
                int secondCardID = Convert.ToInt32(row["id_holecard2"]);

                //Make sure holecards are known
                if (firstCardID > 0 && secondCardID > 0)
                {
                    //Store the data
                    StartingHand curHand = PT4.GetStartingHand(Convert.ToInt32(row["id_holecard1"]), Convert.ToInt32(row["id_holecard2"]));
                    _result[curHand.Name].Data.Add(Convert.ToDouble(row["amt_bb_won"]), (double)row["PFRSize"], Convert.ToDouble(row["amt_rake"]));
                }
            }
            Console.WriteLine("Finished!");
        }
    }
}
