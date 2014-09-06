using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace PokerLib2.Reports
{
    public abstract class StartingHandReport<T>
        where T:ICSVReport,new()
    {
        protected string _connString;

        protected DataTable _data;
        public DataTable Data { get; private set; }

        protected StartingHandGrid<T> _result = new StartingHandGrid<T>();
        public StartingHandGrid<T> Result { get { return _result; } }

        public StartingHandReport()
        {
            _connString = PT4.ConnString;            
        }

        public StartingHandReport(string connString)
        {
            _connString = connString;            
        }

        public StartingHandReport(DataTable data)
        {
            _data = data;
        }

        

    }
}
