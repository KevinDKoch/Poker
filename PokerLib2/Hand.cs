using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace PokerLib2
{
    public class HeadsUpHand
    {        
        protected string _tableName;
        protected string _handHistory;
        protected PokerSite _site;
        protected HHSource _source = HHSource.Raw;
        protected long _gameNum;
        protected Stakes _stakes;
        protected GameType _gameName;
        protected DateTime _datePlayed;
        protected PlayerList _players = new PlayerList();
        protected List<Action> _actions = new List<Action>();
        protected int _maxPlayers;

        //public enum Street {PreFlop, Flop, Turn, River}

        //Players
        //Board
        //WentToShowDown
        //Actions
        //PokerSite
        //_DatePlayed

        public HeadsUpHand(string HandHistory, PokerSite Site, HHSource Source)
        {
            _handHistory = HandHistory;
            _site = Site;
            _maxPlayers = 2;
            BuildHand();
        }

        private void BuildHand()
        {            
            //Select which parser to use
            switch ( _site.Name)
            {
                case "Party": 
                    PartyParse();
                    break;
                default:
                    throw new Exception("Unknown Site:" + _site.Name);                    
            }
        }
        
        public void PartyParse()
        {            
            string[] lines = Regex.Split(_handHistory.Trim(), "\n");
            int i = 0;
            string res = "";
                        
            _gameNum = Convert.ToInt64(Regex.Match(lines[i], @"\d{10,}").Value);
            
            //if (_gameNum == 13879181026) 
            //{ 
            //    Console.WriteLine("Break on problem hands"); 
            //}
            
            //Stakes
            i++;            
            Stakes.Currency cur;
            switch (Regex.Match(lines[i],"^.").Value)
            {
                case "$": 
                    cur = Stakes.Currency.USD;                    
                    break;
                                 
                default:
                    cur = Stakes.Currency.Play;
                    break;
            }

            Single sb = Convert.ToSingle(Regex.Match(lines[i],@"(\d+|0?\.\d\d?)(?=\/)").Value);
            Single bb = Convert.ToSingle(Regex.Match(lines[i], @"(?<=\/\D)(\d+|0?\.\d\d?)|(?<=\/)(\d+|0?\.\d\d?)").Value);
            _stakes = new Stakes(sb, bb, 0, cur);

            //Date Played
            if (Regex.IsMatch(lines[i],@"NL Texas Hold'em"))
            {
                _gameName = GameType.NLH;
            }
            else
            {
                throw new Exception("Unknown Game Type:" + lines[i]);
            }
                
            res = Regex.Match(lines[i], @"(?<= - )(Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday).*").Value;
            _datePlayed = DateTime.ParseExact(res, "dddd, MMMM dd, HH:mm:ss EDT yyyy", null);

            //Table Name            
            res = Regex.Replace(lines[++i], @" \(Real Money\)","");
            _tableName = Regex.Match(res, "(?<=Table ).*").Value;

            //Button Place holder              
            int buttonSeat = Convert.ToInt32(Regex.Match(lines[++i], @"\d+").Value);            
            
            //Players
            int numPlayersToLoad = Convert.ToInt32(Regex.Match(lines[++i], @"\d+(?=\/)").Value);
            if (numPlayersToLoad > _maxPlayers)
            {
                throw new ArgumentException("More players then expected.");
            }
            i++;
            for (; numPlayersToLoad > 0; numPlayersToLoad--,i++)
            {
                int seat = Convert.ToInt32(Regex.Match(lines[i], @"(?<=Seat )\d+").Value);
                string name = Regex.Match(lines[i], @"(?<=: ).*(?= \()").Value;
                res = Regex.Match(lines[i], @"(?<=\( )\S*(?= )").Value;
                res = Regex.Replace(res, @"^\D|,", "");
                double stack = Convert.ToDouble(res);
                _players.Add(new PlayerInfo(name, seat, stack, (seat == buttonSeat)));
            }

            //Actions
            
            //Post Small Blind
            //Fake_Bingo posts small blind [$5 USD].
            for (; i <= lines.Length-1; i++)
            {
                bool processed = false;
                processed = ProcessPlayerAction(lines[i]);
                processed = (processed)? true : ProcessDealerAction(lines[i]);

                if (!processed)
                {
                    throw new ArgumentException("Unknown line:" + lines[i]);
                }
            
            }//End processing actions

        }//PartyParse

        //Returns false if it was not a player action
        private bool ProcessPlayerAction(string line)
        {
            //Is this a player action?
            int actionCount = _actions.Count;
            PlayerInfo actingPlayer = null;
            foreach (PlayerInfo p in _players)
            {
                //if (line.StartsWith(p.Name + " "))
                if(Regex.IsMatch(line, "^" + p.Name))// + @" (:?folds|calls|raises|checks|all-In|posts|shows)"))
                {
                    actingPlayer = p;
                    //isPlayerAction = true;
                    line = line.Substring(p.Name.Length).TrimStart(' '); //Trim player name to make future processing easier
                    
                    break;
                }
            }

            //Process the player action
            if (actingPlayer != null)
            {
                //Grab the amount, if there is one
                string betAmt = Regex.Match(line, @"(?:\.\d\d?)|(?:[\d,]+(\.\d\d?)?)").Value;
                Single amt = (betAmt.Length > 0 && Char.IsDigit(betAmt[betAmt.Length-1]))? Convert.ToSingle(Regex.Replace(betAmt, ",", "")):0;

                if ("folds" == Regex.Match(line, @"^\S+").Value)
                {
                    _actions.Add(new Fold(actingPlayer, _actions.Last().Street));
                }
                else if ("calls" == Regex.Match(line, @"^\S+").Value)
                {
                    _actions.Add(new Call(actingPlayer, _actions.Last().Street, amt));
                }
                else if ("raises" == Regex.Match(line, @"^\S+").Value)
                {
                    _actions.Add(new Raise(actingPlayer, _actions.Last().Street, amt));
                }
                else if ("bets" == Regex.Match(line, @"^\S+").Value)
                {
                    _actions.Add(new Bet(actingPlayer, _actions.Last().Street, amt));
                }
                else if ("checks" == Regex.Match(line, @"^\S+").Value)
                {
                    _actions.Add(new Check(actingPlayer, _actions.Last().Street));
                }
                else if (Regex.IsMatch(line, @"^posts small blind"))
                {
                    if (_actions.Count == 0)
                    {
                        _actions.Add(new PostSmallBlind(actingPlayer, Street.PreFlop, Convert.ToDouble(amt)));
                    }
                    else
                    {
                        _actions.Add(new PostSmallBlind(actingPlayer, _actions.Last().Street, Convert.ToDouble(amt)));                        
                    }                    
                }
                else if (Regex.IsMatch(line, @"^posts big blind"))
                {
                    _actions.Add(new PostBigBlind(actingPlayer, Street.PreFlop, Convert.ToDouble(amt)));
                }
                else if (Regex.IsMatch(line, @"all-In"))
                {
                    //Determine if this was a raise or a call
                    //PlayerInfo nonActingPlayer = (actingPlayer, curStreet.Name == _players[0].Name) ? _players[1] : _players[0];
                    //Regress back to the last player betting action or deal action to determine the context of the All-In
                    Action prevAct = null;
                    for (int iAct = _actions.Count - 1; iAct >= 0; iAct--)
                    {
                        if (_actions[iAct] is Deal || _actions[iAct] is BettingAction)
                        {
                            prevAct = _actions[iAct];
                            break;
                        }
                    }
                                            
                    if (prevAct is Deal || prevAct is Check)
                    {//This must have been a bet
                        _actions.Add(new Bet(actingPlayer, _actions.Last().Street, amt, true));
                    }
                    else if (prevAct is Bet || prevAct is Raise || prevAct is Call)
                    {//This must have been a raise or a call
                        BettingAction prevBet = (BettingAction)prevAct;

                        double amtToCall = TotalBet(prevBet.Player, prevBet.Street, prevBet) - TotalBet(actingPlayer, _actions.Last().Street, _actions.Last());
                        if (amtToCall < amt)
                        {//We must have raised all-in
                            _actions.Add(new Raise(actingPlayer, _actions.Last().Street, amt, true));
                        }
                        else if (amtToCall >= amt)
                        {//We must have gone all-in while calling
                            _actions.Add(new Call(actingPlayer, _actions.Last().Street, amt, true));
                        }
                        else
                        {
                            throw new ArgumentException("Unable to determine context of the all-in.");
                        }
                    }
                    else if (prevAct is PostSmallBlind)
                    {//We must have posted all-In
                        _actions.Add(new PostBigBlind(actingPlayer, _actions.Last().Street, amt, true));
                    }else if(prevAct == null){
                        _actions.Add(new PostSmallBlind(actingPlayer, _actions.Last().Street, amt, true));
                    }                                   
                }//All-In
                else if (Regex.IsMatch(line, @"shows"))
                {
                    Card card1 = new Card(Regex.Match(line, @"(?<=\[\s+)\S+").Value);
                    Card card2 = new Card(Regex.Match(line, @"(?<=\[\s+\S\S )\S+").Value);
                    _actions.Add(new ShowsDownHand(actingPlayer, _actions.Last().Street, card1, card2,true ));
                }
                else if (Regex.IsMatch(line, @"does not show"))
                {
                    _actions.Add(new MuckHand(actingPlayer, _actions.Last().Street));
                }
                else if (Regex.IsMatch(line, @"doesn't show \["))
                {
                    Card card1 = new Card(Regex.Match(line, @"(?<=\[\s+)\S+").Value);
                    Card card2 = new Card(Regex.Match(line, @"(?<=\[\s+\S\S )\S+").Value);
                    _actions.Add(new ShowsDownHand(actingPlayer, _actions.Last().Street, card1, card2, false));
                }
                else if (Regex.IsMatch(line, "^will be using his time bank"))
                {
                    _actions.Add(new UseTimeBank(actingPlayer, _actions.Last().Street));
                }
                else if (Regex.IsMatch(line, "^did not respond in time"))
                {
                    _actions.Add(new TimeOut(actingPlayer, _actions.Last().Street));
                }//grayson772: g2g soon
                else if (Regex.IsMatch(line, @"^: .*"))
                {
                    string message = Regex.Match(line, @"(?<=^: ).*").Value;
                    Street street = (_actions.Count > 0) ? _actions.Last().Street : Street.None;
                    _actions.Add(new Chat(actingPlayer, street, message));
                }
                else if (Regex.IsMatch(line, @"is disconnected\. We will wait"))
                {
                    int seconds = Convert.ToInt32(Regex.Match(line, @"(?<=maximum of )\d+ (?=seconds)").Value);
                    _actions.Add(new Disconnected(actingPlayer, _actions.Last().Street, seconds));
                }
                else if (Regex.IsMatch(line, @"could not respond in time\.\(disconnected\)"))
                {
                    _actions.Add(new DisconnectionTimeOut(actingPlayer, _actions.Last().Street));
                }
                else if (Regex.IsMatch(line, @"has been reconnected and has"))
                {
                    int seconds = Convert.ToInt32(Regex.Match(line, @"(?<= and has )\d+(?= seconds)").Value);
                    _actions.Add(new Reconnected(actingPlayer, _actions.Last().Street, seconds));
                }//Player1 has left the table.
                else if (Regex.IsMatch(line, @"^has left the table"))
                {
                    Street street = (_actions.Count > 0) ? _actions.Last().Street : Street.None;
                    _actions.Add(new LeaveTable(actingPlayer, street));
                }




            }//if (isPlayerAction)

            //Returns true if 1 dealer action was added
            int numActionsAdded = _actions.Count - actionCount;
            if (numActionsAdded == 1)
            {
                return true;
            }
            else if (numActionsAdded == 0)
            {
                return false;
            }
            else
            {
                throw new ArgumentException("Only 1 or 0 actions can be added per line:" + line);
            }            
        }//private bool ProcessPlayerAction(string line)

        private bool ProcessDealerAction(string line)
        {
            int actionCount = _actions.Count;
            bool ignoreAction = false;

            //Regex of all player names
            string playerRegex = "^(?:";
            foreach (PlayerInfo p in _players)
            {
                playerRegex += p.Name + "|";
            }
            playerRegex = playerRegex.TrimEnd('|');
            playerRegex += ") ";

            //Awarding Pots
            if (Regex.IsMatch(line, playerRegex + @"(:?wins )"))
            {
                PlayerInfo winningPlayer = _players[Regex.Match(line, @"^.*(?= wins )").Value];
                string pot = Regex.Match(line, @"(?:\.\d\d?)|(?:[\d,]+(\.\d\d?)?)").Value;
                Double potSize = (pot.Length > 0 && Char.IsDigit(pot[pot.Length - 1])) ? Convert.ToSingle(Regex.Replace(pot, ",", "")) : 0;

                //Side Pot
                int sidePot = 0;
                if(Regex.IsMatch(line, "from the side pot"))
                {
                    sidePot = Convert.ToInt32(Regex.Match(line,@"(?<=side pot )\d(?= )").Value);
                }

                _actions.Add(new AwardPot(winningPlayer, potSize, sidePot));
            }//Dealing Board Cards
            else if (Regex.IsMatch(line, @"^\*\* Dealing down cards"))
            {                
                ignoreAction = true;
            }
            else if (Regex.IsMatch(line, @"^\*\* Dealing Flop"))
            {                
                Card card1 = new Card(Regex.Match(line, @"(?<=\[\s+)\S+").Value);
                Card card2 = new Card(Regex.Match(line, @"(?<=\[\s+\S\S )\S+").Value);
                Card card3 = new Card(Regex.Match(line, @"(?<=\[\s+\S\S \S\S )\S+").Value);
                _actions.Add(new DealFlop(card1, card2, card3));
            }
            else if (Regex.IsMatch(line, @"^\*\* Dealing Turn"))
            {
                _actions.Add(new DealTurn(new Card(Regex.Match(line, @"(?<=\[\s+)\S+").Value)));
            }
            else if (Regex.IsMatch(line, @"^\*\* Dealing River"))
            {
                _actions.Add(new DealRiver(new Card(Regex.Match(line, @"(?<=\[\s+)\S+").Value)));
            }            
            else if (Regex.IsMatch(line, @"^Dealt to"))
            {
                Card card1 = new Card(Regex.Match(line, @"(?<=\[\s+)\S+").Value);
                Card card2 = new Card(Regex.Match(line, @"(?<=\[\s+\S\S )\S+").Value);
                _actions.Add(new DealHoleCards(card1, card2, _players[Regex.Match(line, @"(?<=\S+ \S+ )(\S+)").Value]));
            }
            else if (Regex.IsMatch(line, @"^Your time bank will be activated in \d+ secs"))
            {
                int secs = Convert.ToInt32(Regex.Match(line, @"\d+").Value);
                _actions.Add(new TimeBankActivationWarning(secs, _actions.Last().Street));
            }
            else if (Regex.IsMatch(line, @"Currently you do not have any time left in your Time Bank and your hand has been folded\."))
            {
                _actions.Add(new TimeBankEmptyNotification(_actions.Last().Street));
            }
            else if (Regex.IsMatch(line, @"^Game #\d+ starts"))
            {
                _actions.Add(new StartsNextHand(Convert.ToInt64(Regex.Match(line, @"\d+").Value)));
            }

            //Returns true if 1 dealer action was added
            int numActionsAdded = _actions.Count - actionCount;
            if (numActionsAdded == 1 & ignoreAction == false)
            {
                return true;
            }
            else if (numActionsAdded == 0 & ignoreAction == false)
            {
                return false;
            }
            else if (numActionsAdded == 0 & ignoreAction == true)
            {
                return true;
            }
            else
            {
                throw new ArgumentException("An error occured while processing line:" + line);
            }
        }

        public double TotalBet(PlayerInfo player, Street startingStreet, Action endingAction)
        {
            double result = 0;
            bool startAdding = false;
            int endingIndex = GetActionIndex(endingAction);
            for (int i = 0; i <= endingIndex; i++)
            {
                if (i > _actions.Count - 1)
                {
                    throw new ArgumentException("Invalid hand history: Unable to find ending action.");
                }
                if (startAdding == false)
                {
                    startAdding = (startingStreet == _actions[i].Street);
                }

                if (startAdding)
                {                                                           
                    if (_actions[i] is BettingAction )
                    {
                        BettingAction bet = (BettingAction)_actions[i];
                        if (bet.Player.Name == player.Name)
                        {
                            result += bet.Amount;
                        }
                    }
                }

            }

            return result;
        }

        public int GetActionIndex(Action action)
        {
            int retActionIndex = 0;
            foreach( Action act in _actions){
                if (act == action)
                {
                    return retActionIndex;
                }
                retActionIndex++;
            }

            return -1;
        }
    }//class Hand

    public class PlayerList : KeyedCollection<string, PlayerInfo>
    {
        public PlayerList() : base() { }
        protected override string GetKeyForItem(PlayerInfo item)
        {
            return item.Name;            
        }
    }
}
