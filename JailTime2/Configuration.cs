using JailTime2.Core;
using JailTime2.Core.Manager;
using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailTime2
{
    public class Configuration : IRocketPluginConfiguration
    {
        public string DatabasePath;
        
        public bool IgnoreLastPlayerPosition; // если false то игрока будет спавнить там где он пропал перед тем как попал в тюрягу

        public string BanArrestedReasonOnReconnect; // причина бана
        public bool BanArrestedOnReconnect; // банить ли игрока если он вышел с сервера
        public uint BanDurationOnReconnect; // время бана игрока если он вышел с сервера во время отсиживания своего срока

        public int WalkDistance;

        public Vector3SE SpawnPointAfterArrest; // точка спавна после ареста

        public List<Cell> Cells;

        public void LoadDefaults()
        {
            DatabasePath = @"%rocket%\Plugins\JailTime2\Database\Prisoners.xml";
            
            IgnoreLastPlayerPosition = false;

            BanArrestedReasonOnReconnect = "Вы были забанены за отключение от сервера, во время отбывания срока!";
            BanArrestedOnReconnect = true;
            BanDurationOnReconnect = 500;
            WalkDistance = 5;

            SpawnPointAfterArrest = new Vector3SE()
            {
                X = 1,
                Y = 1,
                Z = 1
            };
            Cells = new List<Cell>()
            {
                new Cell()
                {
                    Id = 1,
                    Position = new Vector3SE(1, 2, 3)
                }
            };

        }
    }
}
