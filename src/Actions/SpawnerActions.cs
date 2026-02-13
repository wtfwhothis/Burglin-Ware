using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using GnomeCheat.Core;
using GnomeCheat.Utils;

namespace GnomeCheat.Actions
{
    public static class SpawnerActions
    {
        public static List<string> LoadItemsList()
        {
            List<string> items = new List<string>();
            AllItems allItems = Resources.FindObjectsOfTypeAll<AllItems>().FirstOrDefault();

            if (allItems?.items != null)
            {
                foreach (ItemData item in allItems.items)
                    items.Add(item.Name);
                GnomeCheatMod.Log($"Total: {items.Count} items loaded");
            }
            else
                GnomeCheatMod.LogError("AllItems not found!");

            return items;
        }

        public static void SpawnItem(string itemName)
        {
            AllItems allItems = Resources.FindObjectsOfTypeAll<AllItems>().FirstOrDefault();
            if (allItems == null) { GnomeCheatMod.LogError("AllItems not found!"); return; }

            PlayerNetworking player = PlayerHelper.GetLocalPlayer();
            if (player == null) { GnomeCheatMod.LogError("Local player not found!"); return; }

            Vector3 spawnPos = player.Position + player.transform.forward * 2f + Vector3.up * 1f;
            try
            {
                ItemInstance spawned = allItems.SpawnItemInstance(itemName, spawnPos);
                GnomeCheatMod.Log(spawned != null ? $"Spawned: {itemName}" : $"Failed to spawn: {itemName}");
            }
            catch (System.Exception e) { GnomeCheatMod.LogError($"Exception: {e.Message}"); }
        }

        public static void SpawnObject(string objectName)
        {
            PlayerNetworking player = PlayerHelper.GetLocalPlayer();
            if (player == null) { GnomeCheatMod.LogError("Local player not found!"); return; }

            GameObject prefab = FindNetworkPrefab(objectName);
            if (prefab == null) { GnomeCheatMod.LogError($"Object prefab not found: {objectName}"); return; }

            Vector3 spawnPos = player.Position + player.transform.forward * 2f + Vector3.up * 1f;
            SpawnNetworkObject(prefab, spawnPos, Quaternion.identity);
        }

        public static void FindAllObjects()
        {
            GnomeCheatMod.Log("===== SEARCHING FOR OBJECTS =====");
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.GetComponent<Knife>() != null || obj.GetComponent<StealableObject>() != null ||
                    obj.name.Contains("Gun") || obj.name.Contains("Weapon") ||
                    obj.name.Contains("Knife") || obj.name.Contains("Taser") || obj.name.Contains("Grenade"))
                {
                    GnomeCheatMod.Log($"Found: {obj.name} (NetworkObject: {obj.GetComponent<NetworkObject>() != null})");
                }
            }
            GnomeCheatMod.Log("===== END SEARCH =====");
        }

        public static async void RainGrenadesOnPlayer(PlayerNetworking target)
        {
            if (target == null) return;
            GameObject prefab = FindNetworkPrefab("Grenade");
            if (prefab == null) { GnomeCheatMod.LogError("Grenade prefab not found!"); return; }

            for (int i = 0; i < 10; i++)
            {
                Vector3 pos = target.Position + Vector3.up * 8f + new Vector3(
                    Random.Range(-3f, 3f), Random.Range(0f, 3f), Random.Range(-3f, 3f));
                SpawnNetworkObject(prefab, pos, Quaternion.identity);
                await System.Threading.Tasks.Task.Delay(200);
            }
            GnomeCheatMod.Log("Rained 10 grenades on player");
        }

        public static async void ItemRain(int count)
        {
            PlayerNetworking local = PlayerHelper.GetLocalPlayer();
            if (local == null) return;

            string[] rainItems = { "Knife", "Fork", "Pan", "Egg", "Slipper", "Underpants",
                "Toaster", "Plunger", "ToiletPaper", "Globe", "Bread", "Teapot",
                "BearToy", "CatToy", "BallToy", "RobotToy", "TrashBucket", "Hairdryer" };

            var prefabs = BuildPrefabCache(rainItems);
            if (prefabs.Count == 0) { GnomeCheatMod.LogError("No item prefabs found!"); return; }

            string[] available = prefabs.Keys.ToArray();
            int spawned = 0;

            for (int i = 0; i < count; i++)
            {
                string item = available[Random.Range(0, available.Length)];
                Vector3 pos = local.Position + new Vector3(Random.Range(-8f, 8f), Random.Range(10f, 18f), Random.Range(-8f, 8f));
                if (SpawnNetworkObject(prefabs[item], pos, Random.rotation)) spawned++;
                await System.Threading.Tasks.Task.Delay(100);
            }
            GnomeCheatMod.Log($"Item Rain: spawned {spawned} items!");
        }

        public static async void GrenadeRain(int count)
        {
            PlayerNetworking local = PlayerHelper.GetLocalPlayer();
            if (local == null) return;
            GameObject prefab = FindNetworkPrefab("Grenade");
            if (prefab == null) { GnomeCheatMod.LogError("Grenade prefab not found!"); return; }

            for (int i = 0; i < count; i++)
            {
                Vector3 pos = local.Position + new Vector3(Random.Range(-6f, 6f), Random.Range(8f, 14f), Random.Range(-6f, 6f));
                SpawnNetworkObject(prefab, pos, Random.rotation);
                await System.Threading.Tasks.Task.Delay(150);
            }
            GnomeCheatMod.Log($"Grenade Rain: {count} grenades!");
        }

        public static async void LagAll(int count)
        {
            var allPlayers = PlayerHelper.GetAllPlayers();
            if (allPlayers.Count == 0) return;

            string[] spamItems = { "Grenade", "Knife", "Fork", "Pan", "Egg", "Toaster", "Plunger", "Globe", "TrashBucket" };
            var prefabs = BuildPrefabCache(spamItems);
            if (prefabs.Count == 0) { GnomeCheatMod.LogError("No prefabs found for lag!"); return; }

            string[] available = prefabs.Keys.ToArray();
            int spawned = 0;

            for (int i = 0; i < count; i++)
            {
                PlayerNetworking target = allPlayers[Random.Range(0, allPlayers.Count)];
                string item = available[Random.Range(0, available.Length)];
                Vector3 pos = target.Position + new Vector3(Random.Range(-3f, 3f), Random.Range(0.5f, 4f), Random.Range(-3f, 3f));

                if (SpawnNetworkObject(prefabs[item], pos, Random.rotation)) spawned++;
                if (spawned % 20 == 0) await System.Threading.Tasks.Task.Delay(10);
            }
            GnomeCheatMod.Log($"LAG ALL: spawned {spawned} objects on {allPlayers.Count} players!");
        }

        // === Helpers ===
        private static GameObject FindNetworkPrefab(string name)
        {
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.name.Equals(name, System.StringComparison.OrdinalIgnoreCase) && obj.GetComponent<NetworkObject>() != null)
                    return obj;
            }
            return null;
        }

        private static Dictionary<string, GameObject> BuildPrefabCache(string[] names)
        {
            var cache = new Dictionary<string, GameObject>();
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (string name in names)
            {
                foreach (GameObject obj in allObjects)
                {
                    if (obj.name.Equals(name, System.StringComparison.OrdinalIgnoreCase) && obj.GetComponent<NetworkObject>() != null)
                    {
                        cache[name] = obj;
                        break;
                    }
                }
            }
            return cache;
        }

        private static bool SpawnNetworkObject(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            try
            {
                GameObject obj = Object.Instantiate(prefab, pos, rot);
                NetworkObject netObj = obj.GetComponent<NetworkObject>();
                if (netObj != null) { netObj.Spawn(); return true; }
                else { Object.Destroy(obj); return false; }
            }
            catch (System.Exception e)
            {
                GnomeCheatMod.LogError($"Spawn failed: {e.Message}");
                return false;
            }
        }
    }
}
