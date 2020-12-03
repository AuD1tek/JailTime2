using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JailTime2.Core.Commands.Jail
{
    public class UnJailPlayerCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "unjail";

        public string Help => "";

        public string Syntax => "/unjail [playername]";

        public List<string> Aliases => new List<string> { "ur" };

        public List<string> Permissions => new List<string> { "unarrest" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length == 0) // /unjail
            {
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("unjail.player.syntax")}", Color.red);
                return;
            }

            string playerName = command[0];
            UnturnedPlayer toPlayer = UnturnedPlayer.FromName(playerName);
            if (command.Length == 1) // /unjail [playername]
            {
                if (toPlayer == null)
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("unjail.player.not.found", command[0])}", Color.red);
                    return;
                }

                if (!JailTimePlugin.Instance.Prison.IsPlayerContains(toPlayer.CSteamID))
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("unjail.player.is.not.arrested", toPlayer.CharacterName)}", Color.red);
                    return;
                }

                if (toPlayer.CSteamID == player.CSteamID)
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("unjail.self", command[0])}", Color.red);
                    return;
                }

                JailTimePlugin.Instance.Prison.TakeOffHandcuffsFromPlayer(toPlayer.CSteamID);

                JailTimePlugin.Instance.Prison.UnArrestPlayer(toPlayer.CSteamID);
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("unjail.successful.unnarrested", toPlayer.CharacterName)}", Color.yellow);
                UnturnedChat.Say(toPlayer, $"{JailTimePlugin.Instance.Translate("unjail.successful.player.unnarrested", player.CharacterName)}", Color.green);

                // снять наручники с игрока
            }
        }
    }
}
