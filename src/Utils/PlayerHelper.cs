using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GnomeCheat.Utils
{
    public static class PlayerHelper
    {
        private static Dictionary<PlayerNetworking, string> playerNames = new Dictionary<PlayerNetworking, string>();

        public static PlayerNetworking GetLocalPlayer()
        {
            return GetAllPlayers().FirstOrDefault(p => p.IsLocalPlayer);
        }

        public static List<PlayerNetworking> GetAllPlayers()
        {
            return Object.FindObjectsByType<PlayerNetworking>(FindObjectsSortMode.None).ToList();
        }

        public static string GetPlayerName(PlayerNetworking player)
        {
            if (playerNames.ContainsKey(player))
                return playerNames[player];

            string name = "Loading...";

            if (player.SteamPlayer != null)
            {
                if (player.SteamPlayer.TryGetName(out string tempName))
                {
                    name = tempName;
                    playerNames[player] = name;
                }
                else
                {
                    LoadPlayerNameAsync(player);
                }
            }

            return name;
        }

        private static async void LoadPlayerNameAsync(PlayerNetworking player)
        {
            try
            {
                string name = await player.SteamPlayer.WaitForName();
                playerNames[player] = name;
                Core.GnomeCheatMod.Log($"Loaded player name: {name}");
            }
            catch (System.Exception e)
            {
                Core.GnomeCheatMod.LogError($"Failed to load player name: {e.Message}");
            }
        }

        public static StatusEffectHandler GetStatusEffects(PlayerNetworking player)
        {
            if (player == null) return null;
            StatusEffectHandler handler = null;
            try { handler = player.StatusEffects; } catch { }
            if (handler == null)
                handler = player.GetComponentInChildren<StatusEffectHandler>();
            return handler;
        }

        public static List<RCCar> GetAllRCCars()
        {
            return Object.FindObjectsByType<RCCar>(FindObjectsSortMode.None).ToList();
        }
    }
}
