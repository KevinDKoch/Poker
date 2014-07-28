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
using System.IO;

namespace BattleStation
{
    public partial class frmMain : Form
    {

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnBatchParse_Click(object sender, EventArgs e)
        {
            int limit = Convert.ToInt32(txtParseCount.Text);

            //Connect to the DB
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string ConnString = "Server=127.0.0.1;Port=5434;User Id=postgres;Password=svcPASS83;Database=PT4_2014_05_23_120015;";            
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
                "cash_hand_histories, cash_hand_summary " +
                "WHERE cash_hand_histories.id_hand = cash_hand_summary.id_hand AND " +
                "cash_hand_summary.cnt_players <= 2 " +
                "LIMIT " + limit;




            //Load the hands
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
            da.Fill(ds);
            dt = ds.Tables[0];

            dgvSQLResults.DataSource = dt;

            Console.WriteLine(dt.Rows.Count);

            conn.Close();                            

            //Parse the hands
            List<HeadsUpHand> Hands = new List<HeadsUpHand>();

            foreach( DataRow row in dt.Rows){
                Hands.Add(new HeadsUpHand(row["history"].ToString(), new PokerSite("Party", 200), HHSource.PT4));
            }
            


        }

        private void btnGenerateHandHistories_Click(object sender, EventArgs e)
        {
            if (timerGenerateHands.Enabled)
            {
                timerGenerateHands.Stop();
                btnGenerateHandHistories.Text = "Generate";
                //TODO Clean Up hands?
            }
            else
            {
                int batchSize = Convert.ToInt32(txtHandGenerationAmount.Text);
                int seconds = Convert.ToInt32(txtGenerationPace.Text);
                timerGenerateHands.Interval = seconds * 1000;
                timerGenerateHands.Start();
                btnGenerateHandHistories.Text = "Stop";
            }
        }

        private void timerGenerateHands_Tick(object sender, EventArgs e)
        {
            //Generate a random number that is long enough
            long gameNum = 10010010010 + new Random().Next();
            
            List<string> handHistory = new List<string>();
            handHistory.Add("Game #" + gameNum.ToString() + " starts.");
            handHistory.Add("");
            handHistory.Add("***** Hand History for Game " + gameNum.ToString() + " *****");            
            handHistory.Add("$5/$10 USD NL Texas Hold'em - " + DateTime.Now.ToString("dddd, MMMM dd, hh:mm:ss EDT yyyy"));
            handHistory.Add("Table Test (Real Money)");
            handHistory.Add("Seat 2 is the button");
            handHistory.Add("Total number of players : 2/2");
            handHistory.Add("Seat 1: FakeBingo ( $1,875.25 USD )");
            handHistory.Add("Seat 2: FakePlayer2 ( $1,063.50 USD )");
            handHistory.Add("FakePlayer2 posts small blind [$5 USD].");
            handHistory.Add("FakeBingo posts big blind [$10 USD].");
            handHistory.Add("** Dealing down cards **");
            handHistory.Add("Dealt to FakeBingo [  As Kc ]");
            handHistory.Add("FakePlayer2 raises [$15 USD]");
            handHistory.Add("FakeBingo will be using his time bank for this hand.");
            handHistory.Add("FakeBingo raises [$60 USD]");
            handHistory.Add("FakePlayer2 raises [$130 USD]");
            handHistory.Add("FakeBingo will be using his time bank for this hand.");
            handHistory.Add("FakeBingo raises [$220 USD]");
            handHistory.Add("FakePlayer2 is all-In  [$913.50 USD]");
            handHistory.Add("FakeBingo calls [$773.50 USD]");
            handHistory.Add("** Dealing Flop ** [ 4h, 8c, Jd ]");
            handHistory.Add("** Dealing Turn ** [ 2d ]");
            handHistory.Add("** Dealing River ** [ 8d ]");
            handHistory.Add("FakeBingo shows [ As, Kc ]a pair of Eights.");
            handHistory.Add("FakePlayer2 shows [ Qd, Qh ]two pairs, Queens and Eights.");
            handHistory.Add("FakePlayer2 wins $2,126 USD from the main pot with two pairs, Queens and Eights.");                        

            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt"))
            File.WriteAllLines(@"E:\Documents\Poker\Temp\GeneratedHands\HandHistories.txt", handHistory.ToArray());

        }

        
    }
}
