using UnityEngine;
using System.Collections.Generic;
using GnomeCheat.Core;
using GnomeCheat.Actions;
using GnomeCheat.Utils;

namespace GnomeCheat.UI.Tabs
{
    public class OnlineTab
    {
        private readonly FeatureManager features;
        private int selectedPlayerIndex;
        private Vector2 scrollPos;
        private Vector2 playerScrollPos;
        private int selectedSpawnEnemy;
        private int spawnCount = 1;
        private readonly string[] spawnEnemyNames = { "Redcap", "Rat", "Roach", "Cat", "Human", "Mole" };

        public OnlineTab(FeatureManager features) => this.features = features;

        public void Draw()
        {
            GUILayout.Label("=== ONLINE PLAYERS ===", Styles.Box);
            GUILayout.Space(10);

            scrollPos = GUILayout.BeginScrollView(scrollPos);

            List<PlayerNetworking> allPlayers = PlayerHelper.GetAllPlayers();
            if (allPlayers.Count == 0) { GUILayout.Label("No players found", Styles.Label); GUILayout.EndScrollView(); return; }

            GUILayout.Label($"Total Players: {allPlayers.Count}", Styles.Label);
            GUILayout.Space(5);

            playerScrollPos = GUILayout.BeginScrollView(playerScrollPos, GUILayout.Height(100));
            for (int i = 0; i < allPlayers.Count; i++)
            {
                string name = PlayerHelper.GetPlayerName(allPlayers[i]);
                string local = allPlayers[i].IsLocalPlayer ? " [YOU]" : "";
                if (GUILayout.Toggle(selectedPlayerIndex == i, $"{name}{local}", Styles.Toggle))
                    selectedPlayerIndex = i;
            }
            GUILayout.EndScrollView();

            if (selectedPlayerIndex < allPlayers.Count)
            {
                PlayerNetworking target = allPlayers[selectedPlayerIndex];
                string targetName = PlayerHelper.GetPlayerName(target);

                GUILayout.Space(10);
                GUILayout.Label($"Actions for: {targetName}", Styles.Box);
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Heal", Styles.Button)) PlayerActions.HealPlayer(target);
                if (GUILayout.Button("Kill", Styles.Button)) PlayerActions.KillPlayer(target);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Ragdoll", Styles.Button)) PlayerActions.ForceRagdoll(target);
                if (GUILayout.Button("Unragdoll", Styles.Button)) PlayerActions.UnRagdoll(target);
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.Label("=== TROLL OPTIONS ===", Styles.Box);
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Explode", Styles.Button)) PlayerActions.Explode(target);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Dismember Head", Styles.Button)) PlayerActions.DismemberHead(target);
                if (GUILayout.Button("Dismember Legs", Styles.Button)) PlayerActions.DismemberLegs(target);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Throw Knife", Styles.Button)) PlayerActions.ThrowKnifeAt(target);
                if (GUILayout.Button("Shoot Player", Styles.Button)) PlayerActions.ShootAt(target);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Grenade", Styles.Button)) GrenadePlayer(target);
                if (GUILayout.Button("Blind", Styles.Button)) BlindPlayer(target);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                bool IsKamikazeActive = RCCarActions.IsKamikazeActive;
                bool newKamikazeState = GUILayout.Toggle(IsKamikazeActive, IsKamikazeActive ? "Stop RC Kamikaze" : "RC Kamikaze", Styles.Toggle);
                if (newKamikazeState != IsKamikazeActive)
                {
                    if (newKamikazeState) RCCarActions.StartKamikaze(target);
                    else RCCarActions.StopKamikaze();
                }
                GUILayout.EndHorizontal();

                bool tornadoActive = PlayerActions.IsTornadoActive;
                bool newTornado = GUILayout.Toggle(tornadoActive, tornadoActive ? "Stop Tornado" : "Start Tornado", Styles.Toggle);
                if (newTornado != tornadoActive)
                {
                    if (newTornado) PlayerActions.StartTornado(target);
                    else PlayerActions.StopTornado();
                }

                //bool freezeActive = PlayerActions.IsFreezeActive;
                // bool newFreeze = GUILayout.Toggle(freezeActive, freezeActive ? "Stop Freeze" : "Freeze Player", Styles.Toggle);
                //if (newFreeze != freezeActive)
                //{
                //if (newFreeze) PlayerActions.StartFreeze(target);
                //    else PlayerActions.StopFreeze();
                //}

                bool spectateActive = PlayerActions.IsSpectateActive;
                bool newSpectate = GUILayout.Toggle(spectateActive, spectateActive ? "Stop Spectate" : "Spectate Player", Styles.Toggle);
                if (newSpectate != spectateActive)
                {
                    if (newSpectate) PlayerActions.StartSpectate(target);
                    else PlayerActions.StopSpectate();
                }

                bool wasStalk = features.StalkModeEnabled && features.StalkTargetIndex == selectedPlayerIndex;
                bool newStalk = GUILayout.Toggle(wasStalk, wasStalk ? "Stop Headstand" : "Headstand (TP on Head)", Styles.Toggle);
                if (newStalk != wasStalk)
                {
                    features.StalkModeEnabled = newStalk;
                    features.StalkTargetIndex = newStalk ? selectedPlayerIndex : -1;
                }

                GUILayout.Space(5);
                GUILayout.Label("=== SPAWN NPC ===", Styles.Box);
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                for (int i = 0; i < spawnEnemyNames.Length; i++)
                {
                    if (GUILayout.Button(spawnEnemyNames[i], selectedSpawnEnemy == i ? Styles.TabActive : Styles.Button))
                        selectedSpawnEnemy = i;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label($"Count: {spawnCount}", Styles.Label);
                spawnCount = Mathf.RoundToInt(GUILayout.HorizontalSlider(spawnCount, 1, 20, Styles.Slider, Styles.SliderThumb, GUILayout.Width(120)));
                GUILayout.EndHorizontal();

                if (GUILayout.Button($"Spawn {spawnCount}x {spawnEnemyNames[selectedSpawnEnemy]} on Player", Styles.Button))
                {
                    SpawnEnemiesOnPlayer(target, spawnEnemyNames[selectedSpawnEnemy], spawnCount);
                }

                GUILayout.Space(5);
                GUILayout.Label("=== KIDNAPPER ===", Styles.Box);
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Spider Kidnap", Styles.Button)) PlayerActions.SpiderKidnap(target);
                if (GUILayout.Button("Vacuum Kidnap", Styles.Button)) PlayerActions.VacuumKidnap(target);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Human Kidnap", Styles.Button)) PlayerActions.HumanKidnap(target);
                if (GUILayout.Button("Release All", Styles.Button)) PlayerActions.ReleaseFromAll(target);
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                if (GUILayout.Button("Teleport to Me", Styles.Button)) PlayerActions.TeleportPlayerToMe(target);
                if (GUILayout.Button("Teleport Me to Them", Styles.Button)) PlayerActions.TeleportMeToPlayer(target);

                GUILayout.Space(5);
                GUILayout.Label("=== CHAOS ===", Styles.Box);
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Set on Fire", Styles.Button)) PlayerActions.SetOnFire(target);
                if (GUILayout.Button("Extinguish", Styles.Button)) PlayerActions.Extinguish(target);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Stun (5s)", Styles.Button)) PlayerActions.Stun(target, 5f);
                if (GUILayout.Button("Stun (15s)", Styles.Button)) PlayerActions.Stun(target, 15f);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Tie Up", Styles.Button)) PlayerActions.Tie(target);
                if (GUILayout.Button("Untie", Styles.Button)) PlayerActions.Untie(target);
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Rain Grenades", Styles.Button)) SpawnerActions.RainGrenadesOnPlayer(target);

                GUILayout.Space(5);
                if (!target.IsLocalPlayer)
                {
                    if (GUILayout.Button("Kick Player (Host Only)", Styles.Button)) PlayerActions.Kick(target);
                }
            }

            GUILayout.EndScrollView();
        }

        private void SpawnEnemiesOnPlayer(PlayerNetworking target, string enemyName, int count)
        {
            try
            {
                var director = Object.FindFirstObjectByType<AiDirector>();
                if (director == null) { GnomeCheat.Core.GnomeCheatMod.LogError("AiDirector not found"); return; }

                var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
                var configs = (AiDirector.EnemySpawnConfig[])typeof(AiDirector).GetField("enemyConfigs", flags)?.GetValue(director);
                if (configs == null) return;

                foreach (var config in configs)
                {
                    if (config.entityData != null && config.entityData.name == enemyName)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Vector3 offset = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
                            config.SpawnNew(target.Position + offset);
                        }
                        GnomeCheat.Core.GnomeCheatMod.Log($"Spawned {count}x {enemyName} on player");
                        return;
                    }
                }
                GnomeCheat.Core.GnomeCheatMod.LogError($"Enemy config '{enemyName}' not found");
            }
            catch (System.Exception e) { GnomeCheat.Core.GnomeCheatMod.LogError($"Spawn failed: {e.Message}"); }
        }

        private void GrenadePlayer(PlayerNetworking target)
        {
            try
            {
                GameObject prefab = null;
                foreach (var g in Resources.FindObjectsOfTypeAll<Grenade>())
                {
                    if (g != null) { prefab = g.gameObject; break; }
                }
                if (prefab == null) { GnomeCheat.Core.GnomeCheatMod.LogError("No Grenade prefab found"); return; }

                GameObject obj = Object.Instantiate(prefab, target.Position + Vector3.up * 1f, Quaternion.identity);
                var netObj = obj.GetComponentInChildren<Unity.Netcode.NetworkObject>();
                if (netObj != null) netObj.Spawn();

                var grenade = obj.GetComponentInChildren<Grenade>();
                if (grenade != null) grenade.InstantExplodeRpc();

                GnomeCheat.Core.GnomeCheatMod.Log("Grenade on player!");
            }
            catch (System.Exception e) { GnomeCheat.Core.GnomeCheatMod.LogError($"Grenade failed: {e.Message}"); }
        }

        private void BlindPlayer(PlayerNetworking target)
        {
            try
            {
                var pc = target.GetComponent<PlayerController>();
                if (pc != null)
                {
                    pc.SetBlind();
                    GnomeCheat.Core.GnomeCheatMod.Log("Blinded player!");
                }
            }
            catch (System.Exception e) { GnomeCheat.Core.GnomeCheatMod.LogError($"Blind failed: {e.Message}"); }
        }
    }
}