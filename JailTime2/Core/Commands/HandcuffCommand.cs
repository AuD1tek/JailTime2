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
    public class HandcuffCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "handcuff";

        public string Help => "";

        public string Syntax => "/handcuff [playername] | /handcuff [на кого смотрит игрок]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "handcuff.immune" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length == 0)
            {
                RaycastInfo rayinfo = DamageTool.raycast(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), 3f, RayMasks.PLAYER_INTERACT, player.Player);

                if (rayinfo.player == null)
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("handcuff.ray.player.not.found")}", Color.red);
                    return;
                }
                UnturnedPlayer toPlayer = UnturnedPlayer.FromPlayer(rayinfo.player);

                if (toPlayer.HasPermission(Permissions[0]))
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("handcuff.immune")}");
                    return;
                }

                // одеть наручники на игрока
                JailTimePlugin.Instance.Prison.HandcuffToPlayer(toPlayer.CSteamID);

                UnturnedChat.Say(toPlayer, $"{JailTimePlugin.Instance.Translate("handcuff.successful.to.player")}");
                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("handcuff.successful", toPlayer.CharacterName)}");
            }

            UnturnedPlayer playerFromName = UnturnedPlayer.FromName(command[0]);
            if (command.Length == 1)
            {
                if (playerFromName == null)
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("handcuff.player.not.found", command[0])}", Color.red);
                    return;
                }

                if (playerFromName.CSteamID == player.CSteamID)
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("handcuff.self")}", Color.red);
                    return;
                }

                if (playerFromName.HasPermission(Permissions[0]))
                {
                    UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("handcuff.immune")}", Color.red);
                    return;
                }

                // одеть наручники на игрока
                JailTimePlugin.Instance.Prison.HandcuffToPlayer(playerFromName.CSteamID);

                UnturnedChat.Say(player, $"{JailTimePlugin.Instance.Translate("handcuff.successful", playerFromName.CharacterName)}", Color.yellow);
                UnturnedChat.Say(playerFromName, $"{JailTimePlugin.Instance.Translate("handcuff.successful.to.player", playerFromName.CharacterName)}", Color.red);
            }
        }
    }
}
