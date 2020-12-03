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
    class RemoveCellCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "removecell";

        public string Help => "";

        public string Syntax => "/removecell [id]";

        public List<string> Aliases => new List<string> { "/rc" };

        public List<string> Permissions => new List<string> { "removecell" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length == 0)
            {
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("removecell.syntax")}", Color.red);
                return;
            }

            int.TryParse(command[0], out int id);
            if (command.Length == 1)
            {
                if (!JailTimePlugin.Instance.Prison.IsCellContains(id))
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("removecell.contains", id)}", Color.red);
                    return;
                }

                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("removecell.removed", id)}", Color.yellow);
                JailTimePlugin.Instance.Prison.RemoveCell(id);

                JailTimePlugin.Instance.Configuration.Save();
            }
        }
    }
}
