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
        private bool _isButton;
        private bool _isSmallBlind;
        private bool _isBigBlind;

        public string Name { get { return _name; } }
        public int Seat { get { return _seat; } }
        public double Stack { get { return _stack; } }
        public bool IsButton { get { return _isButton; } set { _isButton = value; } }
        public bool IsSmallBlind { get { return _isSmallBlind; } set { _isSmallBlind = value; } }
        public bool IsBigBlind { get { return _isBigBlind; } set { _isBigBlind = value; } }

        public PlayerInfo(string name, int seat, double stack, bool isButton)//, bool isSmallBlind, bool isBigBlind)
        {
            _name = name;
            _seat = seat;
            _stack = stack;
            _isButton = isButton;
            //_isSmallBlind = isSmallBlind;
            //_isBigBlind = isBigBlind;
        }
    }
}
