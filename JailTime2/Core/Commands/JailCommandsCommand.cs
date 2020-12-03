using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JailTime2.Core.Commands
{
    public class JailCommandsCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "jailc";

        public string Help => "";

        public string Syntax => "/jailc";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "jailcommands" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length == 0)
            {
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("addcell.info")}", Color.yellow);
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("removecell.info")}", Color.yellow);
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jail.info")}", Color.yellow);
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("unjail.info")}", Color.yellow);
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("currentposition.info")}", Color.yellow);
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("cells.info")}", Color.yellow);
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("handcuffs.info")}", Color.yellow);
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("unhandcuffs.info")}", Color.yellow);
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jailtime.info")}", Color.yellow);
            }
        }
    }
}
