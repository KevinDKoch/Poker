using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace PokerLib2
{
    public class Hand
    {        
        protected string _tableName;
        protected string _handHistory;
        protected PokerSite _site;
        protected HHSource _source = HHSource.Raw;
        protected long _gameNum;
        protected  Stakes _stakes;
        protected GameType _gameName;
        protected DateTime _datePlayed;
        protected PlayerList _players = new PlayerList();
        protected List<Action> _actions = new List<Action>();

        //Players
        //Board
        //WentToShowDown
        //Actions
        //PokerSite
        //_DatePlayed

        public Hand(string HandHistory, PokerSite Site, HHSource Source)
        {
            _handHistory = HandHistory;
            _site = Site;
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
            string[] lines = Regex.Split(_handHistory.Trim(), "\r\n");
            int i = 0;
            string res = "";                       
            _gameNum = Convert.ToInt64(Regex.Match(lines[i], @"\d{10,}").Value);
            
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

            int sb = Convert.ToInt32(Regex.Match(lines[i],@"(\d+|0?\.\d\d?)(?=\/)").Value);
            int bb = Convert.ToInt32(Regex.Match(lines[i],@"(?<=\/\D)(\d+|0?\.\d\d?)|(?<=\/)(\d+|0?\.\d\d?)").Value);
            _stakes = new Stakes(sb, bb, 0, cur);

            //Date Played
            if (Regex.IsMatch(lines[i],@"NL Texas Hold'em"))
                _gameName = GameType.NLH;
            else  
                throw new Exception("Unknown Game Type:" + lines[i]);
            res = Regex.Match(lines[i], @"(?<= - )(Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday).*").Value;
            _datePlayed = DateTime.ParseExact(res, "dddd, MMMM dd, HH:mm:ss EDT yyyy", null);

            //Table Name            
            res = Regex.Replace(lines[++i], @" \(Real Money\)","");
            _tableName = Regex.Match(res, "(?<=Table ).*").Value;

            //Button Place holder              
            int buttonSeat = Convert.ToInt32(Regex.Match(lines[++i], @"\d+").Value);            
            
            //Players
            int numPlayersToLoad = Convert.ToInt32(Regex.Match(lines[++i], @"\d+(?=\/)").Value);
            i++;
            for (; numPlayersToLoad > 0; numPlayersToLoad--,i++)
            {
                int seat = Convert.ToInt32(Regex.Match(lines[i], @"(?<=Seat )\d+").Value);
                string name = Regex.Match(lines[i], @"(?<=: )\S*").Value;
                res = Regex.Match(lines[i], @"(?<=\( )\S*(?= )").Value;
                res = Regex.Replace(res, @"^\D|,", "");
                double stack = Convert.ToDouble(res);
                _players.Add(new PlayerInfo(name, seat, stack));
            }

            //Actions
            
            //Post Small Blind
            //Fake_Bingo posts small blind [$5 USD].
            for (; i <= lines.Length-1; i++)
            {
                if (ProcessPlayerAction(lines[i]))
                {

                }
                else
                {

                }
            
            }//End processing actions

        }//PartyParse

        //Returns false if it was not a player action
        private bool ProcessPlayerAction(string line)
        {
            bool isPlayerAction = false;
            PlayerInfo actingPlayer = null;
            foreach (PlayerInfo p in _players)
            {
                if (line.StartsWith(p.Name + " "))
                {
                    actingPlayer = p;
                    isPlayerAction = true;
                    line = line.Substring(p.Name.Length + 1);
                    break;
                }
            }

            if (isPlayerAction)
            {
                switch (Regex.Match(line, @"\S+").Value)
                {
                    case "folds":
                        break;
                    case "calls":
                        break;
                    case "raises":
                        break;
                    case "checks":
                        break;
                    case "all-In":
                        break;
                    case "posts":
                        if (line.StartsWith("posts small blind"))
                        {
                            string sbAmount = Regex.Match(line, @"(?:\d+(?=\s))|(?:\d*\.\d\d?)").Value;                            
                            //sbAmount = Regex.Match(sbAmount, @"[\d\.]\d*").Value;

                            PostSmallBlind sb = new PostSmallBlind(actingPlayer, Convert.ToDouble(sbAmount));
                            //_actions.Add()
                        }
                        break;
                    default:
                        break;
                }
            }
            return isPlayerAction;
        }


    }

    public class PlayerList : KeyedCollection<string, PlayerInfo>
    {
        public PlayerList() : base() { }
        protected override string GetKeyForItem(PlayerInfo item)
        {
            return item.Name;            
        }
    }
}
