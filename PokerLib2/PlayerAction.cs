using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLib2
{
    //public enum PlayerBettingActions { Fold, Call, Raise, Check, Post_SB, Post_BB}
    abstract class PlayerAction
    {
        protected PlayerInfo _Player;

        public PlayerInfo Player { get { return _Player; } }

        protected PlayerAction(PlayerInfo Player)
        {
            _Player = Player;
        }
    }

    abstract class BettingAction : PlayerAction
    {
        //protected PlayerBettingActions _Act;
        protected double _Amount;
        protected bool _All_In;

        //public PlayerBettingActions Act { get { return _Act; } }
        public double Amount { get { return _Amount; } }
        public bool All_In { get { return _All_In; } }

        //public BettingAction(PlayerInfo Player, PlayerBettingActions Act, double Amount = 0, bool All_In = false):base(Player)
        public BettingAction(PlayerInfo Player, double Amount, bool All_In = false)
            : base(Player)
        {
            //_Act = Act;
            _Amount = Amount;
            _All_In = All_In;
        }
    }

    class Fold : PlayerAction
    {
        public Fold(PlayerInfo player) : base(player) { }
    }

    class Check : PlayerAction
    {
        public Check(PlayerInfo player) : base(player) { }
    }

    class Raise : BettingAction
    {
        public Raise(PlayerInfo player, double amount, bool all_in = false) : base(player, amount, all_in) { }
    }

    class @Call : BettingAction
    {
        public Call(PlayerInfo player, double amount, bool all_in = false) : base(player, amount, all_in) { }
    }

    class PostSmallBlind : BettingAction
    {
        public PostSmallBlind(PlayerInfo player, double amount, bool all_in = false) : base(player, amount, all_in) { }
    }

    class PostBigBlind : BettingAction
    {
        public PostBigBlind(PlayerInfo player, double amount, bool all_in = false) : base(player, amount, all_in) { }
    }

}
