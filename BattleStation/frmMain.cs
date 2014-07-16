using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using PokerLib2;

namespace BattleStation
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {            
            //Connect to the DB
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string ConnString = "Server=127.0.0.1;Port=5434;User Id=postgres;Password=svcPASS83;Database=PT4_2014_05_23_120015;";
            //string ConnString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=dbpass;Database='New PT4 DB';";
            NpgsqlConnection conn = new NpgsqlConnection(ConnString);
            conn.Open();

            if (conn.State == System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Connected!");
            }
            else
            {
                Console.WriteLine("OH NO, NOT CONNECTED!");
            }
                
            //Query for some hands
            string sql = 
                "SELECT " +
                "history " +
                "FROM " +
                "cash_hand_histories, cash_hand_player_statistics " +
                "WHERE " +
                "cash_hand_histories.id_hand = cash_hand_player_statistics.id_hand AND " + 
                "cash_hand_player_statistics.id_player = 2 AND " + 
                "cash_hand_player_statistics.date_played >= '2014/07/05 04:00:00'";

            //Load the hands
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
            da.Fill(ds);
            dt = ds.Tables[0];

            dgvSQLResults.DataSource = dt;

            //Add hands to players
            //Update stats
            //Update listeners
 
            conn.Close();                            
        }

        private void btnTestParse_Click(object sender, EventArgs e)
        {
            string HH = "***** Hand History for Game 33333333333 *****\r\n" +
                        "$5/$10 USD NL Texas Hold'em - Monday, July 07, 15:16:05 EDT 2014\r\n" +
                        "Table Epping Forest (Real Money)\r\n" +
                        "Seat 1 is the button\r\n" +
                        "Total number of players : 2/2\r\n" +
                        "Seat 1: Fake_Bingo ( $1,170.25 USD )\r\n" +
                        "Seat 2: Fake_Buddy ( $2,921.38 USD )\r\n" +
                        "Fake_Bingo posts small blind [$5 USD].\r\n" +
                        "Fake_Buddy posts big blind [$10 USD].\r\n" +
                        "** Dealing down cards **\r\n" +
                        "Dealt to Fake_Bingo [  Ac Ad ]\r\n" +
                        "Fake_Bingo raises [$20 USD]\r\n" +
                        "Fake_Buddy raises [$75 USD]\r\n" +
                        "Fake_Bingo will be using his time bank for this hand.\r\n" +
                        "Fake_Bingo raises [$175 USD]\r\n" +
                        "Fake_Buddy calls [$115 USD]\r\n" +
                        "** Dealing Flop ** [ 9s, 6h, Js ]\r\n" +
                        "Fake_Buddy checks\r\n" +
                        "Fake_Bingo will be using his time bank for this hand.\r\n" +
                        "Fake_Bingo bets [$199.50 USD]\r\n" +
                        "Fake_Buddy raises  [$1,500.01 USD]\r\n" +
                        "Fake_Bingo is all-In  [$770.75 USD]\r\n" +
                        "** Dealing Turn ** [ 3s ]\r\n" +
                        "** Dealing River ** [ 6s ]\r\n" +
                        "Fake_Bingo shows [ Ac, Ad ]two pairs, Aces and Sixes.\r\n" +
                        "Fake_Buddy shows [ Jc, Qc ]two pairs, Jacks and Sixes.\r\n" +
                        "Fake_Buddy wins $1,751.13 USD from the side pot 1 with two pairs, Jacks and Sixes.\r\n" +
                        "Fake_Bingo wins $2,339.50 USD from the main pot with two pairs, Aces and Sixes.";

            Hand H = new Hand(HH, new PokerSite("Party", 200), HHSource.Raw);

        }
    }
}
