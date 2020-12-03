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
        bool IsArrested { get; } // арестован ли игрок
        int ArrestDuration { get; } // время ареста
        Vector3SE LastPositionBeforeArrest { get; } // Последняя позиция игрока передм тем как он попал в тюрьму
        DateTime JailTime { get; } // Время ареста, когда игрока арестовали
    }
}
