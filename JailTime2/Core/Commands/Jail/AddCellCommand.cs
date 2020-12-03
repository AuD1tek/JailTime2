using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JailTime2.Core.Commands.Jail
{
    public class AddCellCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "addcell";

        public string Help => "";

        public string Syntax => "/addcell [id] [X] [Y] [Z]";

        public List<string> Aliases => new List<string> { "add" };

        public List<string> Permissions => new List<string> { "addcell" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;


            if (command.Length == 0) // /addcell
            {
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("addcell.created.auto", player.Position)}", Color.yellow);
                JailTimePlugin.Instance.Prison.CreateCell(JailTimePlugin.Instance.Configuration.Instance.Cells.Count + 1, new Vector3SE(player.Position.x, player.Position.y, player.Position.z));

                JailTimePlugin.Instance.Configuration.Save();
            }

            float.TryParse(command[1], out float xResult);
            float.TryParse(command[2], out float yResult);
            float.TryParse(command[3], out float zResult);

            if (command.Length == 3) // /addcell [X] [Y] [Z]
            {
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("addcell.created", xResult, yResult, zResult)}", Color.yellow);
                JailTimePlugin.Instance.Prison.CreateCell(JailTimePlugin.Instance.Configuration.Instance.Cells.Count + 1, new Vector3SE(player.Position.x, player.Position.y, player.Position.z));

                JailTimePlugin.Instance.Configuration.Save();
            }
        }
    }
}
