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
    public class JailPlayerCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "jail";

        public string Help => "";

        public string Syntax => "/jail [playername] [duration]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "arrest.immune" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length == 0) // /jail
            {
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jail.syntax")}", Color.red);
                return;
            }

            UnturnedPlayer toPlayer = UnturnedPlayer.FromName(command[0]);
            if (command.Length == 1) // /jail [playername]
            {
                if (toPlayer == null)
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jail.player.not.found", command[0])}", Color.red);
                    return;
                }
                if (toPlayer.CSteamID == player.CSteamID)
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jail.player.self")}", Color.red);
                    return;
                }
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jail.syntax")}", Color.red);
                return;
            }

            int.TryParse(command[1], out int resultDuration);
            if (command.Length == 2) // /jail [playername] [duration]
            {
                if (toPlayer == null)
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jail.player.not.found", command[0])}", Color.red);
                    return;
                }

                if (toPlayer.HasPermission(Permissions[0])) // есть ли у другого игрока пермишен иммунитета
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jail.player.immune", toPlayer.CharacterName)}", Color.red);
                    return;
                }

                if (JailTimePlugin.Instance.Prison.IsPlayerContains(toPlayer.CSteamID)) // находится ли игрок в тюрьме
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jail.player.arrested", toPlayer.CharacterName, resultDuration)}", Color.red);
                    return;
                }

                if (toPlayer.CSteamID == player.CSteamID) // проверка на само jail
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jail.player.self")}", Color.red);
                    return;
                }

                // одеть наручники на игрока
                JailTimePlugin.Instance.Prison.HandcuffToPlayer(toPlayer.CSteamID);

                JailTimePlugin.Instance.Prison.ArrestPlayer(toPlayer.CSteamID, resultDuration);
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jail.successful.arrested", toPlayer.CharacterName, resultDuration)}", Color.yellow);
                UnturnedChat.Say(toPlayer, $"{JailTimePlugin.Instance.Translate("jail.player.arrested.message", player.CharacterName, resultDuration)}", Color.red);
            }
        }
    }
}
