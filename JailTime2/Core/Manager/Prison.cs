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
        private List<Player> prisoners = new List<Player>();

        public bool ArrestPlayer(CSteamID player, int arrestDuration)
        {
            UnturnedPlayer resultPlayer = UnturnedPlayer.FromCSteamID(player); // получаем игрока которого мы передали через стим айди

            if (IsPlayerContains(player))
            {
                return false;
            }

            int cellId = GetRandomCell(out Vector3SE position);

            prisoners.Add(new Player(resultPlayer.CSteamID, cellId, true, arrestDuration, new Vector3SE(resultPlayer.Position.x, resultPlayer.Position.y, resultPlayer.Position.z), DateTime.Now));

            resultPlayer.Player.teleportToLocation(GetRandomCellPosition().GetVector3(), resultPlayer.Rotation);
            return true;
        }
        public bool UnArrestPlayer(CSteamID player)
        {
            if (!IsPlayerContains(player))
            {
                return false;
            }

            Player resultPlayer = GetPlayerBySteamId(player);

            bool ignoreLastPosition = JailTimePlugin.Instance.Configuration.Instance.IgnoreLastPlayerPosition;

            Vector3SE lastPosition = resultPlayer.LastPositionBeforeArrest;
            Vector3SE spawnPointPosition = JailTimePlugin.Instance.Configuration.Instance.SpawnPointAfterArrest;

            UnturnedPlayer thisPlayer = UnturnedPlayer.FromCSteamID(player);
            
            if (ignoreLastPosition)
            {
                thisPlayer.Player.teleportToLocation(spawnPointPosition.GetVector3(), thisPlayer.Rotation);
            }
            else
            {
                thisPlayer.Player.teleportToLocation(lastPosition.GetVector3(), thisPlayer.Rotation);
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
            Player resultPlayer = prisoners.FirstOrDefault(p => p.SteamId == player);
            return resultPlayer;
        }
        public int GetRandomCell(out Vector3SE cellPosition)
        {
            Random rand = new Random();

            int minCellCount = JailTimePlugin.Instance.Configuration.Instance.Cells.Min(c => c.Id);
            int cellsCount = JailTimePlugin.Instance.Configuration.Instance.Cells.Count;

            int result = rand.Next(minCellCount, cellsCount);

            cellPosition = GetCellPositionById(result);
            return result;
        }
        public Vector3SE GetRandomCellPosition()
        {
            Random rand = new Random();

            int minCellCount = JailTimePlugin.Instance.Configuration.Instance.Cells.Min(c => c.Id);
            int maxCellCount = JailTimePlugin.Instance.Configuration.Instance.Cells.Max(c => c.Id);

            int result = rand.Next(minCellCount, maxCellCount);

            return GetCellPositionById(result);
        }
        public Vector3SE GetCellPositionById(int id)
        {
            Cell cell = JailTimePlugin.Instance.Configuration.Instance.Cells.FirstOrDefault(c => c.Id == id);
            return cell.Position;
        }
        public bool IsPlayerArrested(CSteamID player)
        {
            Player resultPlayer = prisoners.FirstOrDefault(p => p.SteamId == player);
            if (resultPlayer.IsArrested)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsPlayerContains(CSteamID player)
        {
            return prisoners.FirstOrDefault(p => p.SteamId == player) != null;
        }
        public void RemovePlayer(CSteamID player)
        {
            prisoners.Where(p => p.SteamId == player).ToList().ForEach(p => prisoners.Remove(p));
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
        public IEnumerable<Player> GetPrisoners()
        {
            return prisoners;
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
