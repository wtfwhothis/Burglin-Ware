using UnityEngine;
using System.Collections.Generic;
using GnomeCheat.Actions;

namespace GnomeCheat.UI.Tabs
{
    public class ItemsTab
    {
        private List<string> itemList = new List<string>();
        private int selectedIndex;
        private Vector2 scrollPos;

        public void Draw()
        {
            GUILayout.Label("=== ITEM SPAWNER (Resources) ===", Styles.Box);
            GUILayout.Space(10);

            if (itemList.Count == 0)
            {
                if (GUILayout.Button("Load Items List", Styles.Button, GUILayout.Height(40)))
                    itemList = SpawnerActions.LoadItemsList();
            }
            else
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(400));
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (GUILayout.Toggle(selectedIndex == i, itemList[i], Styles.Toggle))
                        selectedIndex = i;
                }
                GUILayout.EndScrollView();
                GUILayout.Space(10);
                if (GUILayout.Button($"Spawn: {itemList[selectedIndex]}", Styles.Button, GUILayout.Height(40)))
                    SpawnerActions.SpawnItem(itemList[selectedIndex]);
            }
        }
    }
}
