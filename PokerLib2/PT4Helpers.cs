using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerLib2.Game;

namespace PokerLib2.Reports
{
    public static class PT4
    {
        public const String ConnString = "Server=127.0.0.1;Port=5434;User Id=postgres;Password=svcPASS83;Database=Main_PT4_DB;";

        public static Card GetCard(int id_holecard)
        {
            Rank rank = (Rank)(id_holecard % 13 + 1);
            if (rank < Rank.Two)
                rank = Rank.Ace;

            Suit suit = (Suit)(id_holecard / 13 + ((id_holecard % 13 > 0) ? 1 : 0));

            return new Card(rank, suit);
        }
        public static StartingHandCombo GetCombo(int id_holecard1, int id_holecard2)
        {
            Card firstCard = GetCard(id_holecard1);
            Card secondCard = GetCard(id_holecard2);

            return new StartingHandCombo(firstCard, secondCard);
        }

        public static StartingHand GetStartingHand(int id_holecard1, int id_holecard2)
        {
            StartingHandCombo hand = GetCombo(id_holecard1, id_holecard2);
            return new StartingHand(hand.ToString(true, true));
        }
    }
}
