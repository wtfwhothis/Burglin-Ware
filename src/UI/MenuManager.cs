using UnityEngine;
using GnomeCheat.Core;
using GnomeCheat.ESP;
using GnomeCheat.UI.Tabs;

namespace GnomeCheat.UI
{
    public class MenuManager
    {
        public bool IsVisible { get; private set; }

        private Rect menuRect = new Rect(20, 20, 600, 700);
        private int currentTab;

        private readonly FeatureManager features;
        private readonly ESPRenderer espRenderer;

        // Tabs
        private readonly ItemsTab itemsTab;
        private readonly ObjectsTab objectsTab;
        private readonly PlayerTab playerTab;
        private readonly TeleportTab teleportTab;
        private readonly OnlineTab onlineTab;
        private readonly NPCsTab npcsTab;
        private readonly FunTab funTab;
        private readonly ESPTab espTab;

        public MenuManager(FeatureManager features, ESPRenderer espRenderer)
        {
            this.features = features;
            this.espRenderer = espRenderer;

            itemsTab = new ItemsTab();
            objectsTab = new ObjectsTab();
            playerTab = new PlayerTab(features);
            teleportTab = new TeleportTab();
            onlineTab = new OnlineTab(features);
            npcsTab = new NPCsTab();
            funTab = new FunTab(features);
            espTab = new ESPTab(espRenderer.Settings);
        }

        public void ToggleMenu() => IsVisible = !IsVisible;

        public void Draw()
        {
            Styles.Init();
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            menuRect = GUI.Window(0, menuRect, DrawWindow, "", Styles.Background);
        }

        private void DrawWindow(int id)
        {
            GUI.DrawTexture(new Rect(0, 0, menuRect.width, menuRect.height), Styles.TexDark);
            GUILayout.BeginVertical();
            GUILayout.Label("BURGLIN'WARE", Styles.Title);

            GUILayout.BeginHorizontal();
            string[] tabNames = { "Items", "Objects", "Self", "Teleport", "Online", "NPCs", "Miscs", "Visuals" };
            for (int i = 0; i < tabNames.Length; i++)
            {
                if (GUILayout.Button(tabNames[i], currentTab == i ? Styles.TabActive : Styles.Tab))
                    currentTab = i;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(8);

            switch (currentTab)
            {
                case 0: itemsTab.Draw(); break;
                case 1: objectsTab.Draw(); break;
                case 2: playerTab.Draw(); break;
                case 3: teleportTab.Draw(); break;
                case 4: onlineTab.Draw(); break;
                case 5: npcsTab.Draw(); break;
                case 6: funTab.Draw(); break;
                case 7: espTab.Draw(); break;
            }

            GUILayout.EndVertical();
            GUI.DragWindow();
        }
    }
}