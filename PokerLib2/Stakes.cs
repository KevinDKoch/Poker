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
        
        private float _SmallBlind;
        private float _BigBlind;
        private float _Ante;
        private Currency _Cur;

        public float SmallBlind { get {return _SmallBlind; } }
        public float BigBlind { get { return _BigBlind; } }
        public float Ante { get { return _Ante; } }
        public Currency Cur { get { return _Cur; } }

        public Stakes(float SmallBlind, float BigBlind, float Ante = 0, Currency Cur = Stakes.Currency.USD)
        {
            _SmallBlind = SmallBlind;
            _BigBlind = BigBlind;
            _Ante = Ante;
            _Cur = Cur;
        }
    }
}
