using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
    //public enum PlayerBettingActions { Fold, Call, Raise, Check, Post_SB, Post_BB}
    abstract class PlayerAction : Action
    {
        protected PlayerInfo _player;
        public PlayerInfo Player { get { return _player; } }
        
        protected PlayerAction(PlayerInfo player, GameState gameState)
            : base(gameState)
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
        public BettingAction(PlayerInfo player, GameState prevGameState, double amount, bool All_In = false)
            : base(player, prevGameState)
        {
            //_Act = Act;
            _amount = amount;
            _all_In = All_In;
            _curGameState.PotSize += amount;
        }
    }

    class Fold : BettingAction
    {
        public Fold(PlayerInfo player, GameState prevGameState)
            : base(player, prevGameState, 0)
        {            
            _curGameState.ActivePlayers -= 1;
        }
    }

    class Check : BettingAction
    {
        public Check(PlayerInfo player, GameState prevGameState) 
            : base(player, prevGameState, 0) 
        {            
        }
    }

    class Raise : BettingAction
    {
        public Raise(PlayerInfo player, GameState prevGameState, double amount, bool all_in = false)
            : base(player, prevGameState, amount, all_in) 
        {
            
        }
    }

    class Bet : BettingAction
    {
        public Bet(PlayerInfo player, GameState prevGameState, double amount, bool all_in = false)
            : base(player, prevGameState, amount, all_in) 
        {
            
        }
    }

    class @Call : BettingAction
    {
        public Call(PlayerInfo player, GameState prevGameState, double amount, bool all_in = false) 
            : base(player, prevGameState, amount, all_in) 
        {
                  
        }
    }

    class PostSmallBlind : BettingAction
    {
        public PostSmallBlind(PlayerInfo player, GameState prevGameState, double amount, bool all_in = false) 
            : base(player, prevGameState, amount, all_in) 
        {
            player.IsSmallBlind = true;            
        }
    }

    class PostBigBlind : BettingAction
    {
        public PostBigBlind(PlayerInfo player, GameState prevGameState, double amount, bool all_in = false) 
            : base(player, prevGameState, amount, all_in) 
        {
            player.IsBigBlind = true;            
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

        public ShowsDownHand(PlayerInfo player, GameState prevGameState, Card card1, Card card2, bool faceUp, bool voluntary = false) 
            : base(player, prevGameState)
        {
            _card1 = card1;
            _card2 = card2;
            _voluntary = voluntary;
            _faceUp = faceUp;            
        }
    }

    class MuckHand : PlayerAction
    {
        public MuckHand(PlayerInfo player, GameState prevGameState)
            : base(player, prevGameState)
        {            
        }
    }

    class UseTimeBank : PlayerAction
    {
        public UseTimeBank(PlayerInfo player, GameState prevGameState)
            : base(player, prevGameState)
        {
        }
    }

    class TimeOut : PlayerAction
    {
        public TimeOut(PlayerInfo player, GameState prevGameState)
            : base(player, prevGameState)
        {
        }
    }
}
