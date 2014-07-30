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
        
        private readonly float _smallBlind;
        private readonly float _bigBlind;
        private readonly float _ante;
        private readonly Currency _cur;

        public float SmallBlind { get {return _smallBlind; } }
        public float BigBlind { get { return _bigBlind; } }
        public float Ante { get { return _ante; } }
        public Currency Cur { get { return _cur; } }

        public Stakes(float smallBlind, float bigBlind, float ante = 0, Currency cur = Stakes.Currency.USD)
        {
            _smallBlind = smallBlind;
            _bigBlind = bigBlind;
            _ante = ante;
            _cur = cur;
        }
    }
}
