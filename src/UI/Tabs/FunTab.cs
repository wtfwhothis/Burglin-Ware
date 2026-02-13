using UnityEngine;
using System.Collections.Generic;
using GnomeCheat.Core;
using GnomeCheat.Actions;
using GnomeCheat.Utils;

namespace GnomeCheat.UI.Tabs
{
    public class FunTab
    {
        private readonly FeatureManager features;
        private int subTab;
        private int selectedCarIndex;
        private Vector2 carScrollPos;

        public FunTab(FeatureManager features) => this.features = features;

        public void Draw()
        {
            GUILayout.Label("=== MISCS ===", Styles.Box);
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("RC Cars", subTab == 0 ? Styles.TabActive : Styles.Tab)) subTab = 0;
            if (GUILayout.Button("Vacuum", subTab == 1 ? Styles.TabActive : Styles.Tab)) subTab = 1;
            if (GUILayout.Button("Spider", subTab == 2 ? Styles.TabActive : Styles.Tab)) subTab = 2;
            if (GUILayout.Button("Mass", subTab == 3 ? Styles.TabActive : Styles.Tab)) subTab = 3;
            if (GUILayout.Button("World", subTab == 4 ? Styles.TabActive : Styles.Tab)) subTab = 4;
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            switch (subTab)
            {
                case 0: DrawRCCars(); break;
                case 1: DrawVacuum(); break;
                case 2: DrawSpider(); break;
                case 3: DrawMassActions(); break;
                case 4: DrawWorld(); break;
            }
        }

        private void DrawRCCars()
        {
            GUILayout.Label("=== RC CARS ===", Styles.Box);
            GUILayout.Space(10);

            List<RCCar> allCars = PlayerHelper.GetAllRCCars();
            if (allCars.Count == 0)
            {
                GUILayout.Label("No RC Cars in scene", Styles.Label);
                GUILayout.Space(5);
                if (GUILayout.Button("Spawn RC Car", Styles.Button)) SpawnRCCar();
                return;
            }

            GUILayout.Label($"Total RC Cars: {allCars.Count}", Styles.Label);
            GUILayout.Space(5);

            carScrollPos = GUILayout.BeginScrollView(carScrollPos, GUILayout.Height(150));
            for (int i = 0; i < allCars.Count; i++)
            {
                string driver = allCars[i].CurrentDriver != null ? $" (Driver: {PlayerHelper.GetPlayerName(allCars[i].CurrentDriver)})" : " (Empty)";
                if (GUILayout.Toggle(selectedCarIndex == i, $"RC Car #{i + 1}{driver}", Styles.Toggle))
                    selectedCarIndex = i;
            }
            GUILayout.EndScrollView();

            if (selectedCarIndex < allCars.Count)
            {
                RCCar car = allCars[selectedCarIndex];
                GUILayout.Space(10);
                GUILayout.Label($"RC Car #{selectedCarIndex + 1}", Styles.Box);
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("TP to Me", Styles.Button)) RCCarActions.TeleportToMe(car);
                if (GUILayout.Button("TP Me to Car", Styles.Button)) RCCarActions.TeleportMeTo(car);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Launch Up", Styles.Button)) RCCarActions.LaunchUp(car);
                if (GUILayout.Button("Flip", Styles.Button)) RCCarActions.Flip(car);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Spin", Styles.Button)) RCCarActions.Spin(car);
                if (GUILayout.Button("Freeze", Styles.Button)) RCCarActions.Freeze(car);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Super Speed", Styles.Button)) RCCarActions.SuperSpeed(car);
                if (GUILayout.Button("Duplicate", Styles.Button)) RCCarActions.Duplicate(car);
                GUILayout.EndHorizontal();

                if (car.CurrentDriver != null)
                    if (GUILayout.Button("Eject Driver", Styles.Button)) RCCarActions.EjectDriver(car);

                GUILayout.Space(5);
                if (GUILayout.Button("Delete Car", Styles.Button)) RCCarActions.Delete(car);
            }

            GUILayout.Space(10);
            GUILayout.Label("=== SPAWN ===", Styles.Box);
            GUILayout.Space(5);
            if (GUILayout.Button("Spawn RC Car", Styles.Button))
            {
                SpawnRCCar();
            }

            GUILayout.Space(10);
            GUILayout.Label("=== MASS ACTIONS ===", Styles.Box);
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Launch All", Styles.Button)) RCCarActions.LaunchAll();
            if (GUILayout.Button("Super Speed All", Styles.Button)) RCCarActions.SuperSpeedAll();
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Delete All", Styles.Button)) RCCarActions.DeleteAll();
        }

        private void DrawVacuum()
        {
            GUILayout.Label("=== VACUUM ===", Styles.Box);
            GUILayout.Space(10);

            VacuumAiLink vacuum = Object.FindFirstObjectByType<VacuumAiLink>();
            if (vacuum == null) { GUILayout.Label("Vacuum not found in scene", Styles.Label); return; }

            GUILayout.Label($"Status: {(vacuum.Off ? "OFF" : "ON")}", Styles.Label);
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("TP Vacuum to Me", Styles.Button)) NPCActions.TeleportVacuumToMe();
            if (GUILayout.Button("TP Me to Vacuum", Styles.Button)) NPCActions.TeleportMeToVacuum();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            if (GUILayout.Button("Toggle Vacuum ON/OFF", Styles.Button, GUILayout.Height(40))) NPCActions.ToggleVacuum();
            GUILayout.Space(10);
            if (GUILayout.Button("Release All Players", Styles.Button)) NPCActions.VacuumReleaseAll();
        }

        private void DrawSpider()
        {
            GUILayout.Label("=== SPIDER ===", Styles.Box);
            GUILayout.Space(10);

            Spider spider = Object.FindFirstObjectByType<Spider>();
            if (spider == null) { GUILayout.Label("Spider not found in scene", Styles.Label); return; }

            GUILayout.Label($"Spider Status: {(spider.CurrentlyHeld != null ? "Holding Player" : "Free")}", Styles.Label);
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("TP Spider to Me", Styles.Button)) NPCActions.TeleportSpiderToMe();
            if (GUILayout.Button("TP Me to Spider", Styles.Button)) NPCActions.TeleportMeToSpider();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            if (GUILayout.Button("Kill Spider", Styles.Button)) NPCActions.KillSpider();
            if (spider.CurrentlyHeld != null)
                if (GUILayout.Button("Release Player", Styles.Button)) NPCActions.SpiderRelease();
        }

        private void DrawMassActions()
        {
            GUILayout.Label("=== MASS ACTIONS (Host) ===", Styles.Box);
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Kill All", Styles.Button)) MassActions.KillAll();
            if (GUILayout.Button("Heal All", Styles.Button)) MassActions.HealAll();
            GUILayout.EndHorizontal();

            // GUILayout.BeginHorizontal();
            // if (GUILayout.Button("Ragdoll All", Styles.Button)) MassActions.RagdollAll();
            // if (GUILayout.Button("Launch All", Styles.Button)) MassActions.LaunchAll();
            // GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Set All on Fire", Styles.Button)) MassActions.FireAll();
            if (GUILayout.Button("Extinguish All", Styles.Button)) MassActions.ExtinguishAll();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Stun All (5s)", Styles.Button)) MassActions.StunAll(5f);
            if (GUILayout.Button("Tie All", Styles.Button)) MassActions.TieAll();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Untie All", Styles.Button)) MassActions.UntieAll();
            if (GUILayout.Button("TP All to Me", Styles.Button)) MassActions.TeleportAllToMe();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("=== CHAOS ===", Styles.Box);
            GUILayout.Space(5);

            if (GUILayout.Button("Item Rain (30 items)", Styles.Button)) SpawnerActions.ItemRain(30);
            if (GUILayout.Button("Grenade Rain (15)", Styles.Button)) SpawnerActions.GrenadeRain(15);

            GUILayout.Space(5);
            if (GUILayout.Button("LAG ALL (500 objects)", Styles.Button)) SpawnerActions.LagAll(500);
            if (GUILayout.Button("MEGA LAG (2000 objects)", Styles.Button)) SpawnerActions.LagAll(2000);
        }

        private void DrawWorld()
        {
            GUILayout.Label("=== WORLD ===", Styles.Box);
            GUILayout.Space(10);

            if (GUILayout.Button("Open All Doors (Force Push)", Styles.Button))
            {
                int count = 0;
                foreach (var pd in Object.FindObjectsByType<PushableDoor>(FindObjectsSortMode.None))
                {
                    if (pd == null || pd.Rb == null) continue;
                    try
                    {
                        pd.Rb.AddForce(pd.transform.forward * 500f, ForceMode.Impulse);
                        count++;
                    }
                    catch { }
                }
                GnomeCheat.Core.GnomeCheatMod.Log($"Force-pushed {count} doors");
            }

            if (GUILayout.Button("Explode All Grenades", Styles.Button))
            {
                int count = 0;
                foreach (var g in Object.FindObjectsByType<Grenade>(FindObjectsSortMode.None))
                {
                    if (g == null) continue;
                    try { g.InstantExplodeRpc(); count++; } catch { }
                }
                GnomeCheat.Core.GnomeCheatMod.Log($"Exploded {count} grenades");
            }

            GUILayout.Space(15);
            GUILayout.Label("=== EXPLOITS ===", Styles.Box);
            GUILayout.Space(5);

            if (GUILayout.Button("Crash All (Render Corrupt)", Styles.Button))
            {
                PlayerActions.CrashAll();
            }
        }

        private void SpawnRCCar()
        {
            try
            {
                var local = PlayerHelper.GetLocalPlayer();
                if (local == null) return;

                GameObject prefab = null;
                foreach (var obj in Resources.FindObjectsOfTypeAll<RCCar>())
                {
                    if (obj != null) { prefab = obj.gameObject; break; }
                }
                if (prefab == null) { GnomeCheat.Core.GnomeCheatMod.LogError("No RC Car prefab found"); return; }

                Vector3 spawnPos = local.Position + local.transform.forward * 3f;
                GameObject car = Object.Instantiate(prefab, spawnPos, Quaternion.identity);
                var netObj = car.GetComponentInChildren<Unity.Netcode.NetworkObject>();
                if (netObj != null) netObj.Spawn();
                GnomeCheat.Core.GnomeCheatMod.Log("Spawned RC Car");
            }
            catch (System.Exception e) { GnomeCheat.Core.GnomeCheatMod.LogError($"RC Spawn failed: {e.Message}"); }
        }
    }
}