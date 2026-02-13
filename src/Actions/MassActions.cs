using UnityEngine;
using GnomeCheat.Core;
using GnomeCheat.Utils;

namespace GnomeCheat.Actions
{
    public static class MassActions
    {
        public static void KillAll()
        {
            foreach (var p in PlayerHelper.GetAllPlayers())
                if (!p.IsLocalPlayer) PlayerActions.KillPlayer(p);
            GnomeCheatMod.Log("Killed all players");
        }

        public static void HealAll()
        {
            foreach (var p in PlayerHelper.GetAllPlayers())
                PlayerActions.HealPlayer(p);
            GnomeCheatMod.Log("Healed all players");
        }

        public static void RagdollAll()
        {
            foreach (var p in PlayerHelper.GetAllPlayers())
                if (!p.IsLocalPlayer) PlayerActions.ForceRagdoll(p);
            GnomeCheatMod.Log("Ragdolled all players");
        }

        public static void LaunchAll()
        {
            foreach (var p in PlayerHelper.GetAllPlayers())
                if (!p.IsLocalPlayer) PlayerActions.LaunchUp(p);
            GnomeCheatMod.Log("Launched all players");
        }

        public static void FireAll()
        {
            foreach (var p in PlayerHelper.GetAllPlayers())
            {
                var seh = PlayerHelper.GetStatusEffects(p);
                seh?.SetOnFire(true);
            }
            GnomeCheatMod.Log("Set all players on fire");
        }

        public static void ExtinguishAll()
        {
            foreach (var p in PlayerHelper.GetAllPlayers())
            {
                var seh = PlayerHelper.GetStatusEffects(p);
                seh?.SetOnFire(false);
            }
            GnomeCheatMod.Log("Extinguished all players");
        }

        public static void StunAll(float duration)
        {
            foreach (var p in PlayerHelper.GetAllPlayers())
            {
                if (p.IsLocalPlayer) continue;
                PlayerHelper.GetStatusEffects(p)?.StunFor(duration);
            }
            GnomeCheatMod.Log($"Stunned all players for {duration}s");
        }

        public static void TieAll()
        {
            foreach (var p in PlayerHelper.GetAllPlayers())
            {
                if (p.IsLocalPlayer) continue;
                PlayerHelper.GetStatusEffects(p)?.SetTied(true);
            }
            GnomeCheatMod.Log("Tied all players");
        }

        public static void UntieAll()
        {
            foreach (var p in PlayerHelper.GetAllPlayers())
                PlayerHelper.GetStatusEffects(p)?.SetTied(false);
            GnomeCheatMod.Log("Untied all players");
        }

        public static void TeleportAllToMe()
        {
            PlayerNetworking local = PlayerHelper.GetLocalPlayer();
            if (local == null) return;
            foreach (var p in PlayerHelper.GetAllPlayers())
                if (!p.IsLocalPlayer) p.Teleport(local.Position + Vector3.up * 2f);
            GnomeCheatMod.Log("Teleported all players to you");
        }
    }
}
