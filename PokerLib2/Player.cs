using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
    public class PlayerInfo
    {
        private string _name;
        private int _seat;
        private double _stack;

        public string Name { get { return _name; } }
        public int Seat { get { return _seat; } }
        public double Stack { get { return _stack; } }

        public PlayerInfo(string name, int seat, double stack)
        {
            _name = name;
            _seat = seat;
            _stack = stack;
        }
    }
}
