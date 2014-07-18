using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
    public abstract class Action
    {
        protected GameState _curGameState;
        public GameState CurrentGameState { get { return _curGameState; } }

        public Action(GameState gameState)
        {
            _curGameState = gameState;
        }
    }

}
