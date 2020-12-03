using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailTime2.Core.Commands
{
    public class CurrentPositionCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "position";

        public string Help => "";

        public string Syntax => "/position";

        public List<string> Aliases => new List<string> { "pos" };

        public List<string> Permissions => new List<string> { "show.pos" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length == 0)
            {
                UnturnedChat.Say(player, $"Current Position: X: {player.Position.x}, Y: {player.Position.y}, Z: {player.Position.z}");
            }
        }
    }
}
