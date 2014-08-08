using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
    public class Stakes
    {
        public enum Currency {Play, USD, CAD, EUR, GBP};
        
        private readonly double _smallBlind;
        private readonly double _bigBlind;
        private readonly double _ante;
        private readonly Currency _cur;

        public double SmallBlind { get {return _smallBlind; } }
        public double BigBlind { get { return _bigBlind; } }
        public double Ante { get { return _ante; } }
        public Currency Cur { get { return _cur; } }

        public Stakes(double smallBlind, double bigBlind, double ante = 0, Currency cur = Stakes.Currency.USD)
        {
            _smallBlind = smallBlind;
            _bigBlind = bigBlind;
            _ante = ante;
            _cur = cur;
        }
    }
}
