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
    }
}
