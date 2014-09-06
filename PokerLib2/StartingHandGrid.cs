using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerLib2;
using PokerLib2.Game;
using System.IO;

namespace PokerLib2.Reports
{
    public class StartingHandGrid<T> //: IEnumerable<StartingHandData<T>>
        where T : ICSVReport, new()//IGridReport, 
    {
        private Dictionary<string, StartingHandData<T>> _hands = new Dictionary<string,StartingHandData<T>>();
        public T Data { get; set; }
        
        public StartingHandGrid()
        {                        
            int handID = 1;
            for (Rank iRank1 = Rank.Ace; iRank1 >= Rank.Two; iRank1--)
            {
                for (Rank iRank2 = iRank1; iRank2 >= Rank.Two; iRank2--)
                {
                    if (iRank1 == iRank2)
                    {
                        //Add PP group
                        StartingHandData<T> newHand = new StartingHandData<T>(iRank1.ToLetter() + iRank2.ToLetter(), handID);
                        _hands.Add(newHand.Name, newHand);
                        handID++;
                    }
                    else
                    {
                        //Add suited group
                        StartingHandData<T> newHand = new StartingHandData<T>(iRank1.ToLetter() + iRank2.ToLetter() + "s", handID);
                        _hands.Add(newHand.Name, newHand);
                        handID++;

                        //Add off-suit group
                        newHand = new StartingHandData<T>(iRank1.ToLetter() + iRank2.ToLetter() + "o", handID);
                        _hands.Add(newHand.Name, newHand);
                        handID++;
                    }
                }
            }
        }

        public StartingHandData<T> this[string name]
        {
            get { return _hands[name]; }
            set { _hands[name] = value; }
        }

        public IEnumerator<StartingHandData<T>> GetEnumerator()
        {
            return _hands.Values.GetEnumerator();
        }

        /// <summary>
        /// Writes all of the starting hands and values in properties of T to a CSV value.
        /// </summary>
        /// <param name="fileName">The file to create.</param>
        public void SaveCSV(string fileName)
        {
            FileInfo CSVFile = new FileInfo(fileName);
            if (CSVFile.Exists)
                throw new ArgumentException("File already exists:" + fileName);

            
            //FileStream fs = CSVFile.Create();
            
            
            
            bool firstRow = true;
            foreach (StartingHandData<T> hand in _hands.Values)
            {
                if (firstRow)
                {
                    StreamWriter writer = new StreamWriter(CSVFile.OpenWrite());
                    writer.WriteLine("Hand," + hand.Data.CSVHeaders());
                    writer.Close();
                    firstRow = false;
                }

                hand.SaveCSV(fileName);
            }
        }
    }       
}
