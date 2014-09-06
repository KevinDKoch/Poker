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
    public class ERWhenCalling3BetData:ICSVReport
    {
        public List<double> EquityRealized { get; set; }

        public int TotalHands { get { return EquityRealized.Count; } }

        public ERWhenCalling3BetData()
        {
            EquityRealized = new List<double>();
        }

        public void Add(double netWon, double potSizeFlop, double rake)
        {
            if (netWon > 0)
            {
                //Replace the rake when we win, because we want a true ER across stakes and different rake structures
                netWon += rake;
            }
            
            double ER = (netWon + (potSizeFlop/2)) / (potSizeFlop);

            EquityRealized.Add(ER);
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

    public class ERWhenCalling3Bet : StartingHandReport<ERWhenCalling3BetData>
    {

        public ERWhenCalling3Bet()
            : base()
        {
            BuildDataTable();
        }

        public ERWhenCalling3Bet(string connString)
            : base(connString)
        {
            BuildDataTable();
        }

        public ERWhenCalling3Bet(DataTable data)
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
            
                       
            string query = "SELECT (cash_hand_summary.id_hand) as \"id_hand\", (cash_hand_summary.id_site) as \"id_site\", (cash_hand_summary.hand_no) as \"hand_no\", (cash_hand_summary.id_gametype) as \"id_gametype_summary\", (cash_hand_player_statistics.id_limit) as \"id_limit\", ( (CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_summary.amt_pot_f)/( cash_limit.amt_bb)) ELSE 0 END) ) as \"pot_size_flop\",( (CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_player_statistics.amt_won * 1.0 )/( cash_limit.amt_bb)) ELSE 0 END) ) as \"amt_bb_won\",(cash_hand_summary.amt_pot) as \"amt_pot\", (cash_limit.limit_currency) as \"limit_currency\", (cash_hand_player_statistics.id_final_hand) as \"id_final_hand\", (cash_hand_player_statistics.flg_showed) as \"flg_showed\", (cash_hand_player_statistics.enum_folded) as \"enum_folded\", (cash_hand_player_statistics.holecard_1) as \"id_holecard1\", (cash_hand_player_statistics.holecard_2) as \"id_holecard2\", (cash_hand_player_statistics.holecard_3) as \"id_holecard3\", (cash_hand_player_statistics.holecard_4) as \"id_holecard4\", (lookup_actions_p.action) as \"str_actions_p\", (cash_hand_summary.card_1) as \"id_flop1\", (cash_hand_summary.card_2) as \"id_flop2\", (cash_hand_summary.card_3) as \"id_flop3\", (lookup_actions_f.action) as \"str_actions_f\", (cash_hand_summary.card_4) as \"id_turn\", (lookup_actions_t.action) as \"str_actions_t\", (cash_hand_summary.card_5) as \"id_river\", (lookup_actions_r.action) as \"str_actions_r\", (player_winner.player_name) as \"str_winner\", (cash_hand_summary.id_win_hand) as \"id_win_hand\", ( (CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_summary.amt_rake)/( cash_limit.amt_bb)) ELSE 0 END) ) as \"amt_rake\"FROM   cash_hand_player_statistics , cash_hand_summary, lookup_actions lookup_actions_p, lookup_actions lookup_actions_f, lookup_actions lookup_actions_t, lookup_actions lookup_actions_r, player player_winner, cash_limit WHERE  (cash_hand_summary.id_hand = cash_hand_player_statistics.id_hand  AND cash_hand_summary.id_limit = cash_hand_player_statistics.id_limit)  AND (lookup_actions_p.id_action=cash_hand_player_statistics.id_action_p)  AND (lookup_actions_f.id_action=cash_hand_player_statistics.id_action_f)  AND (lookup_actions_t.id_action=cash_hand_player_statistics.id_action_t)  AND (lookup_actions_r.id_action=cash_hand_player_statistics.id_action_r)  AND (cash_limit.id_limit = cash_hand_player_statistics.id_limit)  AND (player_winner.id_player = cash_hand_summary.id_winner)  AND (cash_limit.id_limit = cash_hand_summary.id_limit)   AND (cash_hand_player_statistics.id_player = (SELECT id_player FROM player WHERE player_name_search='donktacular'  AND id_site='100'))   AND ((cash_hand_player_statistics.id_gametype = 1)AND ((((((cash_hand_player_statistics.flg_blind_s)))))AND (((((cash_hand_summary.id_gametype = 1))AND ((cash_limit.flg_nl)))))AND (((((cash_hand_summary.cnt_players BETWEEN 2 and 2)))))AND ((((((case when(char_length(lookup_actions_p.action) < 1) then '' else (substring(lookup_actions_p.action from 1 for 1)) end) = 'R'))AND (((case when(char_length(lookup_actions_p.action) < 2) then '' else (substring(lookup_actions_p.action from 2 for 1)) end) = 'C')))))AND (((((( (CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_player_statistics.amt_p_effective_stack )/( cash_limit.amt_bb)) ELSE 0 END) ) BETWEEN 90.00 and 9999.00)))))AND (((((( (CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_player_statistics.amt_p_3bet_facing )/( cash_limit.amt_bb)) ELSE 0 END) ) BETWEEN 5.00 and 8.00)))))))  ORDER BY (timezone('UTC',  cash_hand_player_statistics.date_played  + INTERVAL '0 HOURS')) desc";
            //string query = "SELECT (cash_hand_summary.id_hand) as \"id_hand\", (cash_hand_summary.id_site) as \"id_site\", (cash_hand_summary.hand_no) as \"hand_no\", (cash_hand_summary.id_gametype) as \"id_gametype_summary\", (cash_hand_player_statistics.id_limit) as \"id_limit\", cash_hand_summary.amt_pot_f as \"pot_size_flop\",( (CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_player_statistics.amt_won * 1.0 )/( cash_limit.amt_bb)) ELSE 0 END) ) as \"amt_bb_won\",(cash_hand_summary.amt_pot) as \"amt_pot\", (cash_limit.limit_currency) as \"limit_currency\", (cash_hand_player_statistics.id_final_hand) as \"id_final_hand\", (cash_hand_player_statistics.flg_showed) as \"flg_showed\", (cash_hand_player_statistics.enum_folded) as \"enum_folded\", (cash_hand_player_statistics.holecard_1) as \"id_holecard1\", (cash_hand_player_statistics.holecard_2) as \"id_holecard2\", (cash_hand_player_statistics.holecard_3) as \"id_holecard3\", (cash_hand_player_statistics.holecard_4) as \"id_holecard4\", (lookup_actions_p.action) as \"str_actions_p\", (cash_hand_summary.card_1) as \"id_flop1\", (cash_hand_summary.card_2) as \"id_flop2\", (cash_hand_summary.card_3) as \"id_flop3\", (lookup_actions_f.action) as \"str_actions_f\", (cash_hand_summary.card_4) as \"id_turn\", (lookup_actions_t.action) as \"str_actions_t\", (cash_hand_summary.card_5) as \"id_river\", (lookup_actions_r.action) as \"str_actions_r\", (player_winner.player_name) as \"str_winner\", (cash_hand_summary.id_win_hand) as \"id_win_hand\", (cash_hand_summary.amt_rake) as \"amt_rake\" FROM   cash_hand_player_statistics , cash_hand_summary, lookup_actions lookup_actions_p, lookup_actions lookup_actions_f, lookup_actions lookup_actions_t, lookup_actions lookup_actions_r, player player_winner, cash_limit WHERE  (cash_hand_summary.id_hand = cash_hand_player_statistics.id_hand  AND cash_hand_summary.id_limit = cash_hand_player_statistics.id_limit)  AND (lookup_actions_p.id_action=cash_hand_player_statistics.id_action_p)  AND (lookup_actions_f.id_action=cash_hand_player_statistics.id_action_f)  AND (lookup_actions_t.id_action=cash_hand_player_statistics.id_action_t)  AND (lookup_actions_r.id_action=cash_hand_player_statistics.id_action_r)  AND (cash_limit.id_limit = cash_hand_player_statistics.id_limit)  AND (player_winner.id_player = cash_hand_summary.id_winner)  AND (cash_limit.id_limit = cash_hand_summary.id_limit)   AND (cash_hand_player_statistics.id_player = (SELECT id_player FROM player WHERE player_name_search='donktacular'  AND id_site='100'))   AND ((cash_hand_player_statistics.id_gametype = 1)AND ((((((cash_hand_player_statistics.flg_blind_s)))))AND (((((cash_hand_summary.id_gametype = 1))AND ((cash_limit.flg_nl)))))AND (((((cash_hand_summary.cnt_players BETWEEN 2 and 2)))))AND ((((((case when(char_length(lookup_actions_p.action) < 1) then '' else (substring(lookup_actions_p.action from 1 for 1)) end) = 'R'))AND (((case when(char_length(lookup_actions_p.action) < 2) then '' else (substring(lookup_actions_p.action from 2 for 1)) end) = 'C')))))AND (((((( (CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_player_statistics.amt_p_effective_stack )/( cash_limit.amt_bb)) ELSE 0 END) ) BETWEEN 90.00 and 9999.00)))))AND (((((( (CASE WHEN ( cash_limit.amt_bb) <> 0 THEN ((cash_hand_player_statistics.amt_p_3bet_facing )/( cash_limit.amt_bb)) ELSE 0 END) ) BETWEEN 5.00 and 8.00)))))))  ORDER BY (timezone('UTC',  cash_hand_player_statistics.date_played  + INTERVAL '0 HOURS')) desc";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            //Load the hands                                
            da.Fill(ds);
            dt = ds.Tables[0];
                
            Console.WriteLine("Hands: " + dt.Rows.Count);
            
            conn.Close();

            BuildReport(dt);
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
                    _result[curHand.Name].Data.Add(Convert.ToDouble(row["amt_bb_won"]), Convert.ToDouble(row["pot_size_flop"]), Convert.ToDouble(row["amt_rake"]));
                }
            }
            Console.WriteLine("Finished!");
        }
    }
    
}
