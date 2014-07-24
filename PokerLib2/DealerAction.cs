using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{    
    public abstract class DealerAction : Action
    {        
        protected DealerAction( Street street)
            : base(street)
        {
        }
    }

    public abstract class Deal : DealerAction
    {
        protected List<Card> _cards;
        public List<Card> Cards { get { return _cards; } }

        public Deal(Street street)
            : base(street)
        {

        }
    }

    class DealHoleCards : Deal
    {
        protected PlayerInfo _player;
        public PlayerInfo Player { get { return _player; } }

        public DealHoleCards(Card card1, Card card2, PlayerInfo player)
            : base(Street.PreFlop)
        {
            _cards = new List<Card> { card1, card2 };
            _player = player;
        }

    }

    class DealFlop : Deal
    {
        public DealFlop(Card card1, Card card2, Card card3)            
            : base(Street.Flop)
        {
            _cards = new List<Card> { card1, card2, card3 };
        }
    }

    class DealTurn : Deal
    {
        public DealTurn(Card card1)
            : base(Street.Turn)
        {
            _cards = new List<Card> { card1};
        }
    }

    class DealRiver : Deal
    {
        public DealRiver(Card card1)
            : base(Street.River)
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

        public AwardPot(PlayerInfo player, double pot, int sidePot = 0 )
            : base(Street.River)
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

        public StartsNextHand(long nextGameNum)
            : base(Street.None)
        {
            _nextGameNum = nextGameNum;
        }
    }

    public class TimeBankActivationWarning : DealerAction
    {
        private int _secondsUntilActivation;
        public int SecondsUntilActivation { get { return _secondsUntilActivation; } }

        public TimeBankActivationWarning( int secondsUntilActivation, Street street) 
            : base (street)
        {
            _secondsUntilActivation = secondsUntilActivation;
        }

    }

    public class TimeBankEmptyNotification : DealerAction
    {
        public TimeBankEmptyNotification(Street street)
            : base(street)
        {
        }
    }

}
