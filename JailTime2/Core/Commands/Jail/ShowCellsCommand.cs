using JailTime2.Core.Manager;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailTime2.Core.Commands.Jail
{
    public class ShowCellsCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "cells";

        public string Help => "";

        public string Syntax => "/cells";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "show.cells" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length == 0)
            {
                foreach (Cell cell in JailTimePlugin.Instance.Configuration.Instance.Cells.OrderBy(c => c.Id))
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("cells", cell.Id, cell.Position.X, cell.Position.Y, cell.Position.Z)}");
                }
            }
        }
    }
}
