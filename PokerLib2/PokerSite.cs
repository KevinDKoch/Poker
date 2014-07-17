using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
    public class PokerSite
    {
        private string _Name;
        private int _ID;

        public  string Name { get { return _Name; } }
        public  int ID { get{ return _ID;} }

        public PokerSite(string Name, int ID)
        {
            _Name = Name;
            _ID = ID;
        }
    }
}
