using UnityEngine;
using GnomeCheat.Core;
using GnomeCheat.Actions;
using GnomeCheat.Utils;

namespace GnomeCheat.UI.Tabs
{
    public class NPCsTab
    {
        private int subTab;
        private int selectedEnemyIndex;
        private int spawnCount = 1;
        private Vector2 scrollPos;
        private readonly string[] enemyNames = { "Redcap", "Rat", "Roach", "Cat", "Human", "Mole" };

        public void Draw()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("NPCs", subTab == 0 ? Styles.TabActive : Styles.Tab)) subTab = 0;
            if (GUILayout.Button("Spawner", subTab == 1 ? Styles.TabActive : Styles.Tab)) subTab = 1;
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            switch (subTab)
            {
                case 0: DrawNPCs(); break;
                case 1: DrawSpawner(); break;
            }
        }

        private void DrawNPCs()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.Label("=== NPC CONTROL ===", Styles.Box);
            GUILayout.Space(10);

            GUILayout.Label("--- BOB (Gnome Thief) ---", Styles.Box);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("TP Bob to Me", Styles.Button)) NPCActions.TeleportBobToMe();
            if (GUILayout.Button("TP Bob Away", Styles.Button)) NPCActions.TeleportBobAway();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Kill Bob", Styles.Button)) NPCActions.KillBob();
            if (GUILayout.Button("Make Bob Drop", Styles.Button)) NPCActions.MakeBobDrop();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("--- HUMAN (Neighbor) ---", Styles.Box);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("TP Human to Me", Styles.Button)) NPCActions.TeleportHumanToMe();
            if (GUILayout.Button("TP Human Away", Styles.Button)) NPCActions.TeleportHumanAway();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Kill Human", Styles.Button)) NPCActions.KillHuman();
            if (GUILayout.Button("Release Player", Styles.Button)) NPCActions.HumanReleasePlayer();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Drop Gun", Styles.Button)) NPCActions.HumanDropGun();
            if (GUILayout.Button("Make Naked", Styles.Button)) NPCActions.MakeHumanNaked();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("--- CAT ---", Styles.Box);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("TP Cat to Me", Styles.Button)) NPCActions.TeleportCatToMe();
            if (GUILayout.Button("Cat Attack Me", Styles.Button)) NPCActions.CatAttackPlayer(PlayerHelper.GetLocalPlayer());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Spam Meow", Styles.Button)) NPCActions.SpamCatMeow();
            if (GUILayout.Button("Spam Hiss", Styles.Button)) NPCActions.SpamCatHiss();
            GUILayout.EndHorizontal();

            // GUILayout.Space(10);
            // if (GUILayout.Button("Find All NPCs", Styles.Button)) NPCActions.FindAllNPCs();

            GUILayout.Space(10);
            GUILayout.Label("=== KILL ALL ===", Styles.Box);
            GUILayout.Space(5);
            if (GUILayout.Button("Kill All Enemies", Styles.Button))
            {
                int count = 0;
                foreach (var entity in Object.FindObjectsByType<GameEntityAI>(FindObjectsSortMode.None))
                {
                    if (entity == null) continue;
                    try
                    {
                        var health = entity.GetComponent<HealthBase>();
                        if (health != null && !entity.IsDead) { health.Kill(); count++; }
                    }
                    catch { }
                }
                GnomeCheatMod.Log($"Killed {count} enemies");
            }

            if (GUILayout.Button("Despawn All Enemies", Styles.Button))
            {
                try
                {
                    var director = Object.FindFirstObjectByType<AiDirector>();
                    if (director != null) director.DespawnEnemies();
                    GnomeCheatMod.Log("Despawned all enemies");
                }
                catch (System.Exception e) { GnomeCheatMod.LogError($"Despawn failed: {e.Message}"); }
            }

            GUILayout.EndScrollView();
        }

        private void DrawSpawner()
        {
            GUILayout.Label("=== ENEMY SPAWNER ===", Styles.Box);
            GUILayout.Space(10);

            GUILayout.Label("Select Enemy:", Styles.Label);
            GUILayout.Space(5);

            for (int i = 0; i < enemyNames.Length; i++)
            {
                if (GUILayout.Button(enemyNames[i], selectedEnemyIndex == i ? Styles.TabActive : Styles.Button))
                    selectedEnemyIndex = i;
            }

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Count: {spawnCount}", Styles.Label);
            spawnCount = Mathf.RoundToInt(GUILayout.HorizontalSlider(spawnCount, 1, 50, Styles.Slider, Styles.SliderThumb, GUILayout.Width(150)));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            if (GUILayout.Button($"Spawn {spawnCount}x {enemyNames[selectedEnemyIndex]}", Styles.Button))
            {
                SpawnEnemies(enemyNames[selectedEnemyIndex], spawnCount);
            }

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("x1", Styles.Button)) SpawnEnemies(enemyNames[selectedEnemyIndex], 1);
            if (GUILayout.Button("x5", Styles.Button)) SpawnEnemies(enemyNames[selectedEnemyIndex], 5);
            if (GUILayout.Button("x10", Styles.Button)) SpawnEnemies(enemyNames[selectedEnemyIndex], 10);
            if (GUILayout.Button("x25", Styles.Button)) SpawnEnemies(enemyNames[selectedEnemyIndex], 25);
            GUILayout.EndHorizontal();
        }

        private void SpawnEnemies(string enemyName, int count)
        {
            try
            {
                var director = Object.FindFirstObjectByType<AiDirector>();
                if (director == null) { GnomeCheatMod.LogError("AiDirector not found"); return; }

                var local = PlayerHelper.GetLocalPlayer();
                if (local == null) return;

                var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
                var configs = (AiDirector.EnemySpawnConfig[])typeof(AiDirector).GetField("enemyConfigs", flags)?.GetValue(director);
                if (configs == null) return;

                foreach (var config in configs)
                {
                    if (config.entityData != null && config.entityData.name == enemyName)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Vector3 offset = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
                            config.SpawnNew(local.Position + local.transform.forward * 3f + offset);
                        }
                        GnomeCheatMod.Log($"Spawned {count}x {enemyName}");
                        return;
                    }
                }
                GnomeCheatMod.LogError($"Enemy config '{enemyName}' not found");
            }
            catch (System.Exception e) { GnomeCheatMod.LogError($"Spawn failed: {e.Message}"); }
        }
    }
}