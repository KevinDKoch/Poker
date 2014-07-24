using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
    public abstract class Action
    {
        //protected GameState _curGameState;
        //public GameState CurrentGameState { get { return _curGameState; } }
        
        protected Street _street;
        public Street Street { get { return _street; } }

        //public Action(GameState gameState)
        public Action(Street street)
        {
            _street = street;
            //_curGameState = gameState.DeepClone();
        }

    }

}
