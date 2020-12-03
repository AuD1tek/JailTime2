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
    public class RemainingTimeJailCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "jailtime";

        public string Help => "";

        public string Syntax => "/jailtime | /jailtime [playername]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length == 0) // /jailtime
            {
                if (!JailTimePlugin.Instance.Prison.IsPlayerContains(player.CSteamID)) // арестован ли этот игрок
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jailtime.not.jailed")}", Color.red);
                    return;
                }

                Player prisoner = JailTimePlugin.Instance.Prison.GetPlayerBySteamId(player.CSteamID); // получаем игрока

                double result = prisoner.ArrestDuration - (DateTime.Now - prisoner.JailTime).TotalSeconds; // получение времени которое осталось сидеть игроку

                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jailtime.successful", result.ToString("0"))}", Color.yellow);

                return;
            }

            UnturnedPlayer toPlayer = UnturnedPlayer.FromName(command[0]);
            if (command.Length == 1) // /jailtime [playername]
            {
                if (toPlayer == null)
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jailtime.player.not.found", command[0])}", Color.red);
                    return;
                }
                if (!JailTimePlugin.Instance.Prison.IsPlayerContains(toPlayer.CSteamID)) // арестован ли этот игрок
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jailtime.not.jailed.to.player", toPlayer.CharacterName)}", Color.red);
                    return;
                }

                Player prisoner = JailTimePlugin.Instance.Prison.GetPlayerBySteamId(toPlayer.CSteamID); // получаем игрока

                double result = prisoner.ArrestDuration - (DateTime.Now - prisoner.JailTime).TotalSeconds; // получение времени которое осталось сидеть игроку

                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("jailtime.successful.to.player", toPlayer.CharacterName, result.ToString("0"))}", Color.yellow);
                return;
            }
        }
    }
}
