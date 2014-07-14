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
                
            conn.Close();
                


            //NpgsqlDataAdapter da = new NpgsqlDataAdapter(SqlDbType, conn);
            



            //Query for new hands
            //Add active players
            //Get data for new players

        }
    }
}
