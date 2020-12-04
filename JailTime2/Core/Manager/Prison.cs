using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace JailTime2.Core.Manager
{
    public class Prison
    {
        public XMLDatabase Database = new XMLDatabase();
        

        public bool ArrestPlayer(UnturnedPlayer player, int arrestDuration)
        {
            if (IsPlayerContains(player.CSteamID))
            {
                return false;
            }

            int cellId = GetRandomCell(out Vector3SE position);
            Database.AddPrisoner(new Player(player.CSteamID, cellId, arrestDuration, player.Position, DateTime.Now));

            player.Player.teleportToLocation(GetRandomCellPosition(), player.Rotation);
            return true;
        }
        public bool UnArrestPlayer(UnturnedPlayer player)
        {
            if (!IsPlayerContains(player.CSteamID))
            {
                return false;
            }

            Player resultPlayer = GetPlayerBySteamId(player.CSteamID);
            bool ignoreLastPosition = JailTimePlugin.Instance.Configuration.Instance.IgnoreLastPlayerPosition;

            
            if (ignoreLastPosition)
            {
                player.Player.teleportToLocation(JailTimePlugin.Instance.Configuration.Instance.SpawnPointAfterArrest, player.Rotation);
            }
            else
            {
                player.Player.teleportToLocation(resultPlayer.Position, player.Rotation);
            }

            RemovePlayer(resultPlayer.SteamId);
            return true;
        }
        public void CreateCell(int id, Vector3SE position)
        {
            JailTimePlugin.Instance.Configuration.Instance.Cells.Add(new Cell(id, position));
        }
        public void RemoveCell(int id)
        {
            JailTimePlugin.Instance.Configuration.Instance.Cells.Where(c => c.Id == id).ToList().ForEach(c => JailTimePlugin.Instance.Configuration.Instance.Cells.Remove(c));
        }



        public Player GetPlayerBySteamId(CSteamID player)
        {
            return Database.GetPrisoner(player);
        }
        public int GetRandomCell(out Vector3SE cellPosition)
        {
            int minCellCount = JailTimePlugin.Instance.Configuration.Instance.Cells.Min(c => c.Id);
            int cellsCount = JailTimePlugin.Instance.Configuration.Instance.Cells.Count;

            int result = UnityEngine.Random.Range(minCellCount, cellsCount);

            cellPosition = GetCellPositionById(result);
            return result;
        }
        public Vector3SE GetRandomCellPosition()
        {
            int minCellCount = JailTimePlugin.Instance.Configuration.Instance.Cells.Min(c => c.Id);
            int maxCellCount = JailTimePlugin.Instance.Configuration.Instance.Cells.Max(c => c.Id);

            int result = UnityEngine.Random.Range(minCellCount, maxCellCount);

            return GetCellPositionById(result);
        }
        public Vector3SE GetCellPositionById(int id)
        {
            Cell cell = JailTimePlugin.Instance.Configuration.Instance.Cells.FirstOrDefault(c => c.Id == id);
            return cell.Position;
        }
        public bool IsPlayerContains(CSteamID player)
        {
            return Database.GetPrisoner(player) != null;
        }
        public bool RemovePlayer(CSteamID player)
        {
            return Database.RemovePrisoner(player);
        }
        public Cell GetCellById(int id)
        {
            Cell cell = JailTimePlugin.Instance.Configuration.Instance.Cells.FirstOrDefault(c => c.Id == id);
            return cell;
        }
        public bool IsCellContains(int id)
        {
            return JailTimePlugin.Instance.Configuration.Instance.Cells.FirstOrDefault(c => c.Id == id) != null;
        }
        public IEnumerable<Cell> GetCellsById(int id)
        {
            var cells = JailTimePlugin.Instance.Configuration.Instance.Cells.Where(c => c.Id == id);
            return cells;
        }
        public void TakeOffHandcuffsFromPlayer(CSteamID player)
        {
            UnturnedPlayer resultPlayer = UnturnedPlayer.FromCSteamID(player);

            resultPlayer.Player.animator.sendGesture(EPlayerGesture.ARREST_STOP, true);
        }
        public void HandcuffToPlayer(CSteamID player)
        {
            UnturnedPlayer resultPlayer = UnturnedPlayer.FromCSteamID(player);

            resultPlayer.Player.animator.sendGesture(EPlayerGesture.ARREST_START, true);
        }
    }
}
