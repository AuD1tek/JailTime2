using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailTime2.Core
{
    public class Player : IArrested
    {
        public CSteamID SteamId { get; private set; }

        public int CellId { get; set; }

        public int Duration { get; set; }

        public Vector3SE Position { get; set; }

        public DateTime Date { get; private set; }

        public Player(CSteamID steamId, int cellId, int duration, Vector3SE position, DateTime date)
        {
            SteamId = steamId;
            CellId = cellId;
            Duration = duration;
            Position = position;
            Date = date;
        }

        public override string ToString()
        {
            return $"SteamId: {SteamId}, ArrestDuration: {Duration}с.";
        }
    }
}
