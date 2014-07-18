using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{    
    abstract class DealerAction : Action
    {        
        protected DealerAction( GameState prevGameState)
            : base(prevGameState)
        {
        }
    }

    abstract class Deal : DealerAction
    {
        protected List<Card> _cards;
        public List<Card> Cards { get { return _cards; } }

        public Deal(GameState prevGameState)
            : base(prevGameState)
        {

        }
    }

    class DealHoleCards : Deal
    {
        protected PlayerInfo _player;
        public PlayerInfo Player { get { return _player; } }

        public DealHoleCards(Card card1, Card card2, PlayerInfo player, GameState prevGameState)
            : base(prevGameState)
        {
            _cards = new List<Card> { card1, card2 };
            _player = player;
        }
    }

    class DealFlop : Deal
    {
        public DealFlop(Card card1, Card card2, Card card3, GameState prevGameState)            
            : base(prevGameState)
        {
            _cards = new List<Card> { card1, card2, card3 };
        }
    }

    class DealTurn : Deal
    {
        public DealTurn(Card card1, GameState prevGameState)
            : base(prevGameState)
        {
            _cards = new List<Card> { card1};
        }
    }

    class DealRiver : Deal
    {
        public DealRiver(Card card1, GameState prevGameState)
            : base(prevGameState)
        {
            _cards = new List<Card> { card1 };
        }
    }

    class AwardPot : DealerAction
    {
        private PlayerInfo _player;
        private double _pot;
        private int _sidePot;
        
        public PlayerInfo Player { get { return _player; } }
        public double Pot { get { return _pot; } }
        public int SidePot { get { return _sidePot; } }

        public AwardPot(PlayerInfo player, GameState prevGameState, double pot, int sidePot = 0 )
            : base(prevGameState)
        {
            _player = player;
            _pot = pot;
            _sidePot = sidePot;
        }
    }

    class StartsNextHand : DealerAction
    {
        private long _nextGameNum;
        public long NextGameNum { get { return _nextGameNum; } }

        public StartsNextHand(long nextGameNum, GameState prevGameState)
            : base(prevGameState)
        {
            _nextGameNum = nextGameNum;
        }
    }
}
