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

namespace JailTime2.Core.Commands
{
    public class UnHandcuffCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "unhandcuff";

        public string Help => "";

        public string Syntax => "/unhandcuff (на кого смотри игрок) | /unhandcuff [имя игрока]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length == 0)
            {
                RaycastInfo rayinfo = DamageTool.raycast(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), 3f, RayMasks.PLAYER_INTERACT, player.Player);

                if (rayinfo.player == null)
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("unhandcuff.ray.player.not.found")}", Color.red);
                    return;
                }
                UnturnedPlayer toPlayer = UnturnedPlayer.FromPlayer(rayinfo.player);


                // снять наручники с игрока
                JailTimePlugin.Instance.Prison.TakeOffHandcuffsFromPlayer(toPlayer.CSteamID);

                UnturnedChat.Say(toPlayer, $"{JailTimePlugin.Instance.Translate("unhandcuff.successful.to.player", player.CharacterName)}");
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("unhandcuff.successful", toPlayer.CharacterName)}");
            }

            UnturnedPlayer playerFromName = UnturnedPlayer.FromName(command[0]);
            if (command.Length == 1)
            {
                if (playerFromName == null)
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("unhandcuff.player.not.found", command[0])}", Color.red);
                    return;
                }

                if (playerFromName.CSteamID == player.CSteamID)
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("unhandcuff.self")}", Color.red);
                    return;
                }

                // снять наручники с игрока
                JailTimePlugin.Instance.Prison.TakeOffHandcuffsFromPlayer(playerFromName.CSteamID);

                UnturnedChat.Say(playerFromName, $"{JailTimePlugin.Instance.Translate("unhandcuff.successful.to.player", player.CharacterName)}");
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("unhandcuff.successful", playerFromName.CharacterName)}");
            }
        }
    }
}
