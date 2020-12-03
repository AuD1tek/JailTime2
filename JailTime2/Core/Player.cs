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

        public bool IsArrested { get; set; }

        public int ArrestDuration { get; set; }

        public Vector3SE LastPositionBeforeArrest { get; set; }

        public DateTime JailTime { get; private set; }

        public Player(CSteamID steamId, int cellId, bool isArrested, int arrestDuration, Vector3SE lastPositionBeforeArrest, DateTime jailTime)
        {
            SteamId = steamId;
            CellId = cellId;
            IsArrested = isArrested;
            ArrestDuration = arrestDuration;
            LastPositionBeforeArrest = lastPositionBeforeArrest;
            JailTime = jailTime;
        }

        public override string ToString()
        {
            return $"SteamId: {SteamId}, Arrested: {(IsArrested ? "да" : "нет")}, ArrestDuration: {ArrestDuration}с.";
        }
    }
}
