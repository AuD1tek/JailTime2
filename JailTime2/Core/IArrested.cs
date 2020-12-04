using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JailTime2.Core
{
    public interface IArrested
    {
        int CellId { get; } // номер клетки в которой находится игрок
        int Duration { get; } // время ареста
        Vector3SE Position { get; } // Последняя позиция игрока передм тем как он попал в тюрьму
        DateTime Date { get; } // Время ареста, когда игрока арестовали
    }
}
