using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2.HandHistory
{
    public class GameState
    {       
        protected Street _curStreet;
        protected double _potSize;
        protected int _activePlayers;
        protected BoardCards _board;

        public Street CurStreet { get { return _curStreet; } set { _curStreet = value; } }
        public double PotSize { get { return _potSize; } set { _potSize = value; } }
        public int ActivePlayers { get { return _activePlayers; } set { _activePlayers = value; } }
        public BoardCards Board { get { return _board; } }
                
        public GameState(Street curStreet, double potSize, int activePlayers, BoardCards board = null)
        {
            _curStreet = curStreet;
            _potSize = potSize;
            _activePlayers = activePlayers;
            _board = board;
        }

        public GameState DeepClone()
        {
            BoardCards board = (_board == null)? null: _board.DeepClone();
            return new GameState(_curStreet, _potSize, _activePlayers, board);
        }
    }
}
