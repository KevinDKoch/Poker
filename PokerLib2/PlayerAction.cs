using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerLib2.Game;

namespace PokerLib2.HandHistory
{
    //public enum PlayerBettingActions { Fold, Call, Raise, Check, Post_SB, Post_BB}
    abstract class PlayerAction : Action
    {
        protected PlayerInfo _player;
        public PlayerInfo Player { get { return _player; } }
        
        protected PlayerAction(PlayerInfo player, Street street)
            : base(street)
        {
            _player = player;            
        }
    }

    abstract class BettingAction : PlayerAction
    {
        //protected PlayerBettingActions _Act;
        protected double _amount;
        protected bool _all_In;

        //public PlayerBettingActions Act { get { return _Act; } }
        public double Amount { get { return _amount; } }
        public bool All_In { get { return _all_In; } }

        //public BettingAction(PlayerInfo Player, PlayerBettingActions Act, double Amount = 0, bool All_In = false):base(Player)
        public BettingAction(PlayerInfo player, Street street, double amount, bool All_In = false)
            : base(player, street)
        {
            //_Act = Act;
            _amount = amount;
            _all_In = All_In;
            //_curGameState.PotSize += amount;
        }
    }

    class Fold : BettingAction
    {
        public Fold(PlayerInfo player, Street street)
            : base(player, street, 0)
        {            
            //_curGameState.ActivePlayers -= 1;
        }
    }

    class Check : BettingAction
    {
        public Check(PlayerInfo player, Street street) 
            : base(player, street, 0) 
        {            
        }
    }

    class Raise : BettingAction
    {
        public Raise(PlayerInfo player, Street street, double amount, bool all_in = false)
            : base(player, street, amount, all_in) 
        {
            
        }
    }

    class Bet : BettingAction
    {
        public Bet(PlayerInfo player, Street street, double amount, bool all_in = false)
            : base(player, street, amount, all_in) 
        {
            
        }
    }

    class @Call : BettingAction
    {
        public Call(PlayerInfo player, Street street, double amount, bool all_in = false) 
            : base(player, street, amount, all_in) 
        {
                  
        }
    }

    class PostSmallBlind : BettingAction
    {
        public PostSmallBlind(PlayerInfo player, Street street, double amount, bool all_in = false) 
            : base(player, street, amount, all_in) 
        {
            player.IsSmallBlind = true;
            //_curGameState.ActivePlayers += 1;
        }
    }

    class PostBigBlind : BettingAction
    {
        public PostBigBlind(PlayerInfo player, Street street, double amount, bool all_in = false) 
            : base(player, street, amount, all_in) 
        {
            player.IsBigBlind = true;
            //_curGameState.ActivePlayers += 1;
        }
    }

    class ShowsDownHand : PlayerAction
    {
        private Card _card1;
        private Card _card2;
        private bool _voluntary;
        private bool _faceUp;

        public Card Card1 { get { return _card1; } }
        public Card Card2 { get { return _card2; } }
        public bool Voluntary { get { return _voluntary; } }
        public bool FaceUp { get { return _faceUp; } }

        public ShowsDownHand(PlayerInfo player, Street street, Card card1, Card card2, bool faceUp, bool voluntary = false) 
            : base(player, street)
        {
            _card1 = card1;
            _card2 = card2;
            _voluntary = voluntary;
            _faceUp = faceUp;            
        }
    }

    class MuckHand : PlayerAction
    {
        public MuckHand(PlayerInfo player, Street street)
            : base(player, street)
        {            
        }
    }

    class UseTimeBank : PlayerAction
    {
        public UseTimeBank(PlayerInfo player, Street street)
            : base(player, street)
        {
        }
    }

    class TimeOut : PlayerAction
    {
        public TimeOut(PlayerInfo player, Street street)
            : base(player, street)
        {
        }
    }

    class DisconnectionTimeOut : TimeOut
    {
        public DisconnectionTimeOut( PlayerInfo player, Street street)
            :base(player, street)
        {

        }
    }
    
    class Chat : PlayerAction
    {
        private string _message;
        public string Message { get { return _message; } }

        public Chat( PlayerInfo player, Street street, string message )
            : base(player, street)
        {
            _message = message;
        }
    }

    class Disconnected : PlayerAction
    {
        private int _secondsRemaining;
        public int SecondsRemaining { get { return _secondsRemaining; } }

        public Disconnected(PlayerInfo player, Street street, int secondsRemaining)
            : base(player, street)
        {
            _secondsRemaining = secondsRemaining;
        }

    }

    class Reconnected : PlayerAction
    {
        private int _secondsToAct;
        public int SecondsToAct { get { return _secondsToAct; } }

        public Reconnected(PlayerInfo player, Street street, int secondsToAct)
            : base(player, street)
        {
            _secondsToAct = secondsToAct;
        }

    }

    class LeaveTable : PlayerAction
    {
        public LeaveTable( PlayerInfo player, Street street)
            : base(player, street)
        {

        }
    }



}
