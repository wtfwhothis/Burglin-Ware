using UnityEngine;
using Unity.Netcode;
using GnomeCheat.Core;
using GnomeCheat.Utils;

namespace GnomeCheat.Actions
{
    public static class PlayerActions
    {
        public static void HealPlayer(PlayerNetworking player)
        {
            if (player == null) return;
            player.Health.Respawn();
            GnomeCheatMod.Log("Healed player");
        }

        public static void KillPlayer(PlayerNetworking player)
        {
            if (player == null) return;
            player.Health.TakeDamage(999f);
            GnomeCheatMod.Log("Killed player");
        }

        public static void ForceRagdoll(PlayerNetworking player)
        {
            if (player == null) return;
            player.ForceRagdoll();
            GnomeCheatMod.Log("Ragdoll forced");
        }

        public static void UnRagdoll(PlayerNetworking player)
        {
            if (player == null) return;
            player.ForceNotRagdoll();
            GnomeCheatMod.Log("Unragdolled");
        }

        public static void LaunchUp(PlayerNetworking player)
        {
            if (player == null) return;
            player.ForceRagdoll();
            player.AddRagdollVelocity(Vector3.up * 50f);
            GnomeCheatMod.Log("Launched player up");
        }

        public static void Explode(PlayerNetworking player)
        {
            if (player == null) return;
            player.AddExplosionForceToRagdollRpc(player.Position, 10f, 1000f);
            GnomeCheatMod.Log("Exploded player");
        }

        public static void Spin(PlayerNetworking player)
        {
            if (player == null) return;
            player.ForceRagdoll();
            player.AddRagdollVelocity(new Vector3(
                Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-20f, 20f)));
            GnomeCheatMod.Log("Spinning player");
        }

        public static void Freeze(PlayerNetworking player)
        {
            if (player == null) return;
            player.SetRagdollVelocity(Vector3.zero);
            GnomeCheatMod.Log("Froze player");
        }

        public static void DismemberHead(PlayerNetworking player)
        {
            if (player == null) return;
            player.Dismemberment.DismemberRpc(
                DismembermentController.DismemberSection.DismemberPart.Head, 100f, player.Position, 30f, 5f);
            GnomeCheatMod.Log("Dismembered player's head");
        }

        public static void DismemberLegs(PlayerNetworking player)
        {
            if (player == null) return;
            player.Dismemberment.DismemberRpc(
                DismembermentController.DismemberSection.DismemberPart.RightLeg |
                DismembermentController.DismemberSection.DismemberPart.LeftLeg,
                100f, player.Position, 30f, 5f);
            GnomeCheatMod.Log("Dismembered player's legs");
        }

        public static void SetOnFire(PlayerNetworking player)
        {
            var seh = PlayerHelper.GetStatusEffects(player);
            if (seh != null) { seh.SetOnFire(true); GnomeCheatMod.Log("Set player on fire"); }
        }

        public static void Extinguish(PlayerNetworking player)
        {
            var seh = PlayerHelper.GetStatusEffects(player);
            if (seh != null) { seh.SetOnFire(false); GnomeCheatMod.Log("Extinguished player"); }
        }

        public static void Stun(PlayerNetworking player, float duration)
        {
            var seh = PlayerHelper.GetStatusEffects(player);
            if (seh != null) { seh.StunFor(duration); GnomeCheatMod.Log($"Stunned player for {duration}s"); }
        }

        public static void Tie(PlayerNetworking player)
        {
            var seh = PlayerHelper.GetStatusEffects(player);
            if (seh != null) { seh.SetTied(true); GnomeCheatMod.Log("Tied player"); }
        }

        public static void Untie(PlayerNetworking player)
        {
            var seh = PlayerHelper.GetStatusEffects(player);
            if (seh != null) { seh.SetTied(false); GnomeCheatMod.Log("Untied player"); }
        }

        public static void Kick(PlayerNetworking player)
        {
            if (player == null || player.IsLocalPlayer) return;
            NetworkManager netManager = NetworkManager.Singleton;
            if (netManager == null || !netManager.IsServer)
            {
                GnomeCheatMod.LogError("You must be the host to kick players!");
                return;
            }
            ulong clientId = player.OwnerClientId;
            netManager.DisconnectClient(clientId);
            GnomeCheatMod.Log($"Kicked player (ClientId: {clientId})");
        }

        public static void TeleportToSpawn(PlayerNetworking player)
        {
            GnomeHouse house = Object.FindFirstObjectByType<GnomeHouse>();
            if (player != null && house != null)
            {
                player.Teleport(house.PlayerSpawnPosition);
                GnomeCheatMod.Log("Teleported to spawn");
            }
        }

        public static void TeleportForward(PlayerNetworking player, float distance)
        {
            if (player == null) return;
            player.Teleport(player.Position + player.transform.forward * distance);
            GnomeCheatMod.Log($"Teleported forward {distance}m");
        }

        public static void TeleportUp(PlayerNetworking player, float distance)
        {
            if (player == null) return;
            player.Teleport(player.Position + Vector3.up * distance);
            GnomeCheatMod.Log($"Teleported up {distance}m");
        }

        public static void TeleportPlayerToMe(PlayerNetworking target)
        {
            PlayerNetworking local = PlayerHelper.GetLocalPlayer();
            if (target != null && local != null)
            {
                target.Teleport(local.Position);
                GnomeCheatMod.Log("Teleported player to you");
            }
        }

        public static void TeleportMeToPlayer(PlayerNetworking target)
        {
            PlayerNetworking local = PlayerHelper.GetLocalPlayer();
            if (target != null && local != null)
            {
                local.Teleport(target.Position);
                GnomeCheatMod.Log("Teleported you to player");
            }
        }

        public static void SpiderKidnap(PlayerNetworking player)
        {
            Spider spider = Object.FindFirstObjectByType<Spider>();
            if (spider != null && player != null)
            {
                spider.CurrentlyHeld = player;
                player.ApplyEffect(GameEntityBase.EffectType.Tie, 0f);
                GnomeCheatMod.Log("Spider kidnapped player");
            }
        }

        public static void VacuumKidnap(PlayerNetworking player)
        {
            VacuumAiLink vacuum = Object.FindFirstObjectByType<VacuumAiLink>();
            if (vacuum == null || player == null) return;
            PlayerMultiKidnapper mk = vacuum.GetComponent<PlayerMultiKidnapper>();
            if (mk != null)
            {
                mk.AddKidnapped(player);
                player.ApplyEffect(GameEntityBase.EffectType.Tie, 0f);
                GnomeCheatMod.Log("Vacuum kidnapped player");
            }
        }

        public static void HumanKidnap(PlayerNetworking player)
        {
            HumanAILink human = Object.FindFirstObjectByType<HumanAILink>();
            if (human != null && player != null)
            {
                human.kidnapper.CurrentlyHeld = player;
                player.ApplyEffect(GameEntityBase.EffectType.Tie, 0f);
                GnomeCheatMod.Log("Human kidnapped player");
            }
        }

        public static void ReleaseFromAll(PlayerNetworking player)
        {
            if (player == null) return;
            player.ApplyEffect(GameEntityBase.EffectType.Untie, 0f);

            Spider spider = Object.FindFirstObjectByType<Spider>();
            if (spider != null && spider.CurrentlyHeld == player) spider.CurrentlyHeld = null;

            HumanAILink human = Object.FindFirstObjectByType<HumanAILink>();
            if (human != null && human.PlayerInHand == player) human.ReleasePlayer();

            VacuumAiLink vacuum = Object.FindFirstObjectByType<VacuumAiLink>();
            if (vacuum != null) vacuum.GetComponent<PlayerMultiKidnapper>()?.RemoveAllKidnapped();

            GnomeCheatMod.Log("Released player from all kidnappers");
        }

        public static void ThrowKnifeAt(PlayerNetworking target)
        {
            if (target == null) return;

            GameObject knifePrefab = null;
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.name.Equals("Knife", System.StringComparison.OrdinalIgnoreCase) && obj.GetComponent<NetworkObject>() != null)
                {
                    knifePrefab = obj;
                    break;
                }
            }

            if (knifePrefab == null) { GnomeCheatMod.LogError("Knife prefab not found!"); return; }

            Vector3 spawnPos = target.Position + Vector3.up * 5f + new Vector3(
                Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

            try
            {
                GameObject knife = Object.Instantiate(knifePrefab, spawnPos, Quaternion.LookRotation(Vector3.down));
                NetworkObject netObj = knife.GetComponent<NetworkObject>();
                if (netObj != null)
                {
                    netObj.Spawn();
                    Rigidbody rb = knife.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Vector3 dir = (target.Position + Vector3.up * 1f - spawnPos).normalized;
                        rb.linearVelocity = dir * 40f;
                    }
                    GnomeCheatMod.Log("Threw knife at player");
                }
                else
                    Object.Destroy(knife);
            }
            catch (System.Exception e) { GnomeCheatMod.LogError($"Knife throw failed: {e.Message}"); }
        }

        public static void ShootAt(PlayerNetworking target)
        {
            if (target == null) return;

            PlayerNetworking local = PlayerHelper.GetLocalPlayer();
            if (local == null) return;

            Gun[] allGuns = Object.FindObjectsByType<Gun>(FindObjectsSortMode.None);
            Gun gun = null;
            foreach (Gun g in allGuns)
            {
                if (g.gameObject.activeInHierarchy)
                {
                    gun = g;
                    break;
                }
            }

            if (gun == null)
            {
                GameObject gunPrefab = null;
                foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
                {
                    if (obj.name.Equals("Gun", System.StringComparison.OrdinalIgnoreCase) && obj.GetComponent<NetworkObject>() != null)
                    {
                        gunPrefab = obj;
                        break;
                    }
                }

                if (gunPrefab == null) { GnomeCheatMod.LogError("Gun prefab not found!"); return; }

                Vector3 spawnPos = local.Position + Vector3.up * 100f;
                try
                {
                    GameObject spawned = Object.Instantiate(gunPrefab, spawnPos, Quaternion.identity);
                    NetworkObject netObj = spawned.GetComponent<NetworkObject>();
                    if (netObj != null)
                    {
                        netObj.Spawn();
                        gun = spawned.GetComponent<Gun>();
                    }
                    else
                    {
                        Object.Destroy(spawned);
                        return;
                    }
                }
                catch (System.Exception e) { GnomeCheatMod.LogError($"Gun spawn failed: {e.Message}"); return; }
            }

            if (gun == null) return;

            gun.SetBulletCount(Mathf.Max(gun.BulletCount, 10));

            var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;

            Transform bulletSource = (Transform)typeof(Gun).GetField("bulletSource", flags)?.GetValue(gun);
            if (bulletSource == null) { GnomeCheatMod.LogError("bulletSource not found!"); return; }

            Vector3 targetCenter = target.Position + Vector3.up * 1f;
            Vector3 shootOrigin = targetCenter - (targetCenter - bulletSource.position).normalized * 3f;

            gun.transform.position = shootOrigin;
            bulletSource.position = shootOrigin;
            bulletSource.forward = (targetCenter - shootOrigin).normalized;

            var doShoot = typeof(Gun).GetMethod("DoShoot", flags);
            if (doShoot != null)
            {
                doShoot.Invoke(gun, new object[] { bulletSource.forward });
                GnomeCheatMod.Log("Shot at player");
            }
            else
            {
                gun.ShootGunRpc(default(RpcParams));
                GnomeCheatMod.Log("Shot at player (RPC fallback)");
            }
        }

        private static PlayerNetworking tornadoTarget;
        private static bool tornadoActive;
        private static float tornadoAngle;

        public static void StartTornado(PlayerNetworking target)
        {
            if (target == null) return;
            tornadoTarget = target;
            tornadoActive = true;
            tornadoAngle = 0f;
            GnomeCheatMod.Log("Tornado started on player");
        }

        public static void StopTornado()
        {
            tornadoActive = false;
            tornadoTarget = null;
            GnomeCheatMod.Log("Tornado stopped");
        }

        public static bool IsTornadoActive => tornadoActive;

        public static void UpdateTornado()
        {
            if (!tornadoActive || tornadoTarget == null) { tornadoActive = false; return; }

            tornadoAngle += Time.deltaTime * 360f;
            float radius = 3f;
            float height = Mathf.Sin(Time.time * 2f) * 3f + 5f;

            Vector3 basePos = tornadoTarget.Position;
            Vector3 spinPos = basePos + new Vector3(
                Mathf.Cos(tornadoAngle * Mathf.Deg2Rad) * radius,
                height,
                Mathf.Sin(tornadoAngle * Mathf.Deg2Rad) * radius
            );

            tornadoTarget.ForceRagdoll();
            tornadoTarget.SetRagdollVelocity(Vector3.zero);

            Vector3 dir = (spinPos - tornadoTarget.Puppet.puppetMaster.muscles[0].rigidbody.position);
            tornadoTarget.AddForceToRagdoll(dir * 50f, ForceMode.Acceleration);
            tornadoTarget.AddForceToRagdoll(Vector3.up * 20f, ForceMode.Acceleration);
        }

        private static PlayerNetworking freezeTarget;
        private static bool freezeActive;
        private static Vector3 freezePosition;

        public static void StartFreeze(PlayerNetworking target)
        {
            if (target == null) return;
            freezeTarget = target;
            freezeActive = true;
            freezePosition = target.Position;
            GnomeCheatMod.Log("Freeze started on player");
        }

        public static void StopFreeze()
        {
            freezeActive = false;
            freezeTarget = null;
            GnomeCheatMod.Log("Freeze stopped");
        }

        public static bool IsFreezeActive => freezeActive;

        public static void UpdateFreeze()
        {
            if (!freezeActive || freezeTarget == null) { freezeActive = false; return; }

            freezeTarget.ForceRagdoll();
            freezeTarget.SetRagdollVelocity(Vector3.zero);

            if (freezeTarget.Puppet?.puppetMaster != null)
            {
                foreach (var muscle in freezeTarget.Puppet.puppetMaster.muscles)
                {
                    if (muscle.rigidbody != null)
                    {
                        muscle.rigidbody.linearVelocity = Vector3.zero;
                        muscle.rigidbody.angularVelocity = Vector3.zero;
                        muscle.rigidbody.position = freezePosition + (muscle.rigidbody.position - freezeTarget.Position);
                    }
                }
            }
        }

        private static PlayerNetworking spectateTarget;
        private static bool spectateActive;
        private static Vector3 spectateOriginalPos;

        public static void StartSpectate(PlayerNetworking target)
        {
            if (target == null) return;
            var local = GnomeCheat.Utils.PlayerHelper.GetLocalPlayer();
            if (local == null) return;
            spectateTarget = target;
            spectateActive = true;
            spectateOriginalPos = local.Position;
            GnomeCheatMod.Log("Spectating player");
        }

        public static void StopSpectate()
        {
            if (spectateActive)
            {
                var local = GnomeCheat.Utils.PlayerHelper.GetLocalPlayer();
                if (local != null)
                    local.Actor.Teleport(spectateOriginalPos);
            }
            spectateActive = false;
            spectateTarget = null;
            GnomeCheatMod.Log("Spectate stopped");
        }

        public static bool IsSpectateActive => spectateActive;

        public static void UpdateSpectate()
        {
            if (!spectateActive || spectateTarget == null) { spectateActive = false; return; }

            var local = GnomeCheat.Utils.PlayerHelper.GetLocalPlayer();
            if (local == null) return;

            Vector3 targetPos = spectateTarget.Position;
            Vector3 offset = new Vector3(0f, 3f, -5f);

            Camera cam = Camera.main;
            if (cam != null)
            {
                Vector3 camBack = -cam.transform.forward;
                offset = camBack * 5f + Vector3.up * 3f;
            }

            Vector3 spectatePos = targetPos + offset;
            local.Actor.Teleport(spectatePos);
        }

        public static void CrashAll()
        {

            // ultra uhq crash method lol found randomly ingame combining beta test features

            var local = GnomeCheat.Utils.PlayerHelper.GetLocalPlayer();
            if (local == null) return;

            var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;

            Transform parent = local.transform.parent;
            if (parent != null)
            {
                var np = parent.GetComponent<RootMotion.Dynamics.NetworkPuppet>();
                if (np != null)
                {
                    var targetRootField = typeof(RootMotion.Dynamics.NetworkPuppet).GetField("targetRoot", flags);
                    if (targetRootField != null)
                    {
                        Transform targetRoot = (Transform)targetRootField.GetValue(np);
                        if (targetRoot != null)
                            targetRoot.localScale = Vector3.zero;
                    }
                }
            }

            if (local.Puppet?.puppetMaster != null)
            {
                foreach (var muscle in local.Puppet.puppetMaster.muscles)
                {
                    if (muscle.rigidbody != null)
                        muscle.rigidbody.transform.localScale = Vector3.zero;
                }
            }

            local.ToggleGraphics(false);

            if (parent != null)
            {
                var np = parent.GetComponent<RootMotion.Dynamics.NetworkPuppet>();
                if (np != null)
                {
                    var targetRootField = typeof(RootMotion.Dynamics.NetworkPuppet).GetField("targetRoot", flags);
                    if (targetRootField != null)
                    {
                        Transform targetRoot = (Transform)targetRootField.GetValue(np);
                        if (targetRoot != null)
                            targetRoot.localScale = Vector3.one;
                    }
                }
            }

            if (local.Puppet?.puppetMaster != null)
            {
                foreach (var muscle in local.Puppet.puppetMaster.muscles)
                {
                    if (muscle.rigidbody != null)
                        muscle.rigidbody.transform.localScale = Vector3.one;
                }
            }

            local.ToggleGraphics(true);

            local.Health.Respawn(); // force resync lol

            GnomeCheatMod.Log("Crash exploit executed!");
        }
    }
}