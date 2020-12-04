using JailTime2.Core;
using JailTime2.Core.Manager;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

namespace JailTime2
{
    public class JailTimePlugin : RocketPlugin<Configuration>
    {
        public static JailTimePlugin Instance;

        public Prison Prison;

        #region Load/Unload
        protected override void Load()
        {
            Instance = this;
            Prison = new Prison();

            UnturnedPlayerEvents.OnPlayerDeath += onPlayerDeath;
            U.Events.OnPlayerConnected += onPlayerConnected;
        }
        

        protected override void Unload()
        {
            Instance = null;

            UnturnedPlayerEvents.OnPlayerDeath -= onPlayerDeath;
            U.Events.OnPlayerConnected -= onPlayerConnected;
        }
        #endregion

        #region Translate
        public override TranslationList DefaultTranslations => new TranslationList()
         {
             { "jail.syntax",                                                                  "Попробуйте так: /jail [имя игрока] [время заключения]" },
             { "jail.player.not.found",                                                        "Игрок {0} не найден, попробуйте еще раз." },
             { "jail.player.self",                                                             "Вы не можете посадить самого себя." },
             { "jail.player.immune",                                                           "Игрок {0} имеет иммунитет, вы не можете его посадить в тюрьму." },
             { "jail.player.arrested",                                                         "Игрок {0} уже арестован, ему осталось {1}c." }, // {1} сколько времени осталось в тюряге
             { "jail.player.is.arrested",                                                      "Вы не можете арестоовать так-как вы арестованны." },
             { "jail.successful.arrested",                                                     "Вы успешно арестовали игрока {0} на {1}с." },
             { "jail.player.arrested.message",                                                 "Вас арестовал игрок {0} на {1}с." },



             { "unjail.player.syntax",                                                         "Попробуйте так: /unjail [имя игрока]" },
             { "unjail.player.not.found",                                                      "Игрок {0} не найден." },
             { "unjail.self",                                                                  "Вы не можете выпустить себя из тюрьмы." },
             { "unjail.player.is.not.arrested",                                                "Игрок {0} не арестован." },
             { "unjail.successful.unnarrested",                                                "Игрок {0} успешно освобожден из тюрьмы." },
             { "unjail.successful.player.unnarrested",                                         "Игрок {0} освободил вас из тюрьмы." },



             { "addcell.syntax",                                                               "Попробуйте так: /addcell | /addcell [id] [X] [Y] [Z]" },
             { "addcell.created.auto",                                                         "На месте где вы стоите создана новая клетка. {0}" }, // {1} ПОЗИЦИЯ ГДЕ ПОСТАВЛЕНА КЛЕТКА
             { "addcell.created",                                                              "Вы успешно создали новую клетку. (X({0}), Y({1}), Z({2}))" },
             { "addcell.contains",                                                             "Клетка {0}, уже существует." },



             { "addcell.info",                                                                 "/addcell (добавление по текущей позиции) | /addcell [X] [Y] [Z] - добавление клетки" },
             { "removecell.info",                                                              "/removecell [id] - удаление клетки по номеру" },
             { "jail.info",                                                                    "/jail [имя игрока] [время заключения] - заключить игрока на время" },
             { "unjail.info",                                                                  "/unjail [имя игрока] - исключить из тюрьмы игрока" },
             { "currentposition.info",                                                         "/position - показывает текущую позицию (может помощь при создании клетки)" },
             { "cells.info",                                                                   "/cells - показывает все созданные клетки" },
             { "handcuffs.info",                                                               "/handcuff (куда смотрит игрок) | /handcuff [имя игрока] - одеть наручники на игрока" },
             { "unhandcuffs.info",                                                             "/unhandcuff (куда смотрит игрок) | /handcuff [имя игрока] - снять наручники с игрока" },
             { "jailtime.info",                                                                "/jailtime (сработает на самого себя) | /jailtime [имя игрока] - посмотреть сколько времени осталось сидеть" },



             { "removecell.syntax",                                                            "/removecell [id]" }, 
             { "removecell.contains",                                                          "Клетка под номером {0} не существует." },
             { "removecell.removed",                                                           "Клетка под номером {0} успешно удалена." },



             { "cells",                                                                        "Номер клетки: {0}, позиция: (X: {1}, Y: {2}, Z: {3})" },



             { "handcuff.ray.player.not.found",                                                "Вы не смотрите на игрока, попробуйте еще раз." },
             { "handcuff.player.not.found",                                                    "Игрок {0} не найден." },
             { "handcuff.self",                                                                "Вы не можете надеть наручники самому себе." },
             { "handcuff.immune",                                                              "Вы не можете надеть наручники на этого игрока, он имеет иммунитет." },
             { "handcuff.successful",                                                          "Вы успешно одели наручники на игрока {0}." },
             { "handcuff.successful.to.player",                                                "Игрок {0} одел на вас наручники." },



             { "unhandcuff.ray.player.not.found",                                              "Вы не смотрите на игрока, попробуйте еще раз." },
             { "unhandcuff.player.not.found",                                                  "Игрок {0} не найден." },
             { "unhandcuff.self",                                                              "Вы не можете снять наручники самому себе." },
             { "unhandcuff.successful",                                                        "Вы успешно сняли наручники с игрока на которого вы смотрите." }, // {0} имя игрока
             { "unhandcuff.successful.to.player",                                              "Игрок {0} снял вам наручники." },



             { "jailtime.not.jailed",                                                          "Вы не находитесь в тюрьме."},
             { "jailtime.not.jailed.to.player",                                                "Игрок {0} не находится в тюрьме."},
             { "jailtime.successful",                                                          "Вам осталось {0}с."},
             { "jailtime.successful.to.player",                                                "Игроку {0} осталось {1}с."},
             { "jailtime.player.not.found",                                                    "Игрок {0} не найден."},



             { "arrested.suicide",                                                             "Вы все еще арестованы, вы не можете выбрать из тюрьмы, поэтому мы вернули вас обратно." },
         };
        #endregion

        #region Events
        private void onPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {
            if (Prison.IsPlayerContains(player.CSteamID))
            {
                Core.Player prisoner = Prison.GetPlayerBySteamId(player.CSteamID);
                Cell cell = Prison.GetCellById(prisoner.CellId);

                player.Player.teleportToLocation(cell.Position, player.Rotation);

                UnturnedChat.Say(player, $"{Translate("arrested.suicide")}", Color.red);
            }
        }
        private void onPlayerConnected(UnturnedPlayer player)
        {
            if (Instance.Configuration.Instance.BanArrestedOnReconnect)
            {
                if (Prison.IsPlayerContains(player.CSteamID))
                {
                    player.Ban($"{Instance.Configuration.Instance.BanArrestedReasonOnReconnect}", Instance.Configuration.Instance.BanDurationOnReconnect);
                }
            }
        }
        #endregion

        #region FixedUpdate
        public void FixedUpdate()
        {
            foreach (UnturnedPlayer player in Provider.clients.Select(s => UnturnedPlayer.FromSteamPlayer(s)))
            {
                Core.Player prisoner = Prison.GetPlayerBySteamId(player.CSteamID);

                if (prisoner != null)
                {
                    if (Vector3.Distance(player.Position, Prison.GetCellPositionById(prisoner.CellId)) > Instance.Configuration.Instance.WalkDistance)
                        player.Player.teleportToLocation(Prison.GetCellPositionById(prisoner.CellId), player.Rotation);

                    if ((DateTime.Now - prisoner.Date).TotalSeconds >= prisoner.Duration)
                    {
                        Prison.UnArrestPlayer(player);
                        Prison.TakeOffHandcuffsFromPlayer(prisoner.SteamId);
                    }
                }
            }
        }
        #endregion

    }
}
