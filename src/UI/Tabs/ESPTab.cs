using UnityEngine;
using GnomeCheat.ESP;

namespace GnomeCheat.UI.Tabs
{
    public class ESPTab
    {
        private readonly ESPSettings s;
        private int subTab;

        public ESPTab(ESPSettings settings) => s = settings;

        public void Draw()
        {
            GUILayout.Label("=== ESP / VISUALS ===", Styles.Box);
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Players", subTab == 0 ? Styles.TabActive : Styles.Tab)) subTab = 0;
            if (GUILayout.Button("NPCs", subTab == 1 ? Styles.TabActive : Styles.Tab)) subTab = 1;
            if (GUILayout.Button("Items", subTab == 2 ? Styles.TabActive : Styles.Tab)) subTab = 2;
            if (GUILayout.Button("Settings", subTab == 3 ? Styles.TabActive : Styles.Tab)) subTab = 3;
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            switch (subTab)
            {
                case 0: DrawPlayerESP(); break;
                case 1: DrawNpcESP(); break;
                case 2: DrawItemESP(); break;
                case 3: DrawSettings(); break;
            }
        }

        private void DrawPlayerESP()
        {
            GUILayout.Label("=== PLAYER ESP ===", Styles.Box);
            GUILayout.Space(5);
            s.PlayerEnabled = GUILayout.Toggle(s.PlayerEnabled, "Enable Player ESP", Styles.Toggle);
            if (!s.PlayerEnabled) return;
            GUILayout.Space(5);
            s.PlayerBoxes = GUILayout.Toggle(s.PlayerBoxes, "Show Boxes", Styles.Toggle);
            s.PlayerNames = GUILayout.Toggle(s.PlayerNames, "Show Names", Styles.Toggle);
            s.PlayerHealth = GUILayout.Toggle(s.PlayerHealth, "Show Health", Styles.Toggle);
            s.PlayerDistance = GUILayout.Toggle(s.PlayerDistance, "Show Distance", Styles.Toggle);
            s.PlayerTracers = GUILayout.Toggle(s.PlayerTracers, "Show Tracers", Styles.Toggle);
        }

        private void DrawNpcESP()
        {
            GUILayout.Label("=== NPC ESP ===", Styles.Box);
            GUILayout.Space(5);
            s.NpcEnabled = GUILayout.Toggle(s.NpcEnabled, "Enable NPC ESP", Styles.Toggle);
            if (!s.NpcEnabled) return;
            GUILayout.Space(5);
            GUILayout.Label("NPCs to Track:", Styles.Label);
            s.NpcBob = GUILayout.Toggle(s.NpcBob, "Bob (Gnome Thief)", Styles.Toggle);
            s.NpcHuman = GUILayout.Toggle(s.NpcHuman, "Human (Neighbor)", Styles.Toggle);
            s.NpcSpider = GUILayout.Toggle(s.NpcSpider, "Spider", Styles.Toggle);
            GUILayout.Space(5);
            GUILayout.Label("Display Options:", Styles.Label);
            s.NpcBoxes = GUILayout.Toggle(s.NpcBoxes, "Show Boxes", Styles.Toggle);
            s.NpcNames = GUILayout.Toggle(s.NpcNames, "Show Names", Styles.Toggle);
            s.NpcDistance = GUILayout.Toggle(s.NpcDistance, "Show Distance", Styles.Toggle);
            s.NpcTracers = GUILayout.Toggle(s.NpcTracers, "Show Tracers", Styles.Toggle);
        }

        private void DrawItemESP()
        {
            GUILayout.Label("=== ITEM ESP ===", Styles.Box);
            GUILayout.Space(5);
            s.ItemEnabled = GUILayout.Toggle(s.ItemEnabled, "Enable Item ESP", Styles.Toggle);
            if (!s.ItemEnabled) return;
            GUILayout.Space(5);
            s.ItemNames = GUILayout.Toggle(s.ItemNames, "Show Names", Styles.Toggle);
            s.ItemDistance = GUILayout.Toggle(s.ItemDistance, "Show Distance", Styles.Toggle);
            s.ItemTracers = GUILayout.Toggle(s.ItemTracers, "Show Tracers", Styles.Toggle);
        }

        private void DrawSettings()
        {
            GUILayout.Label("=== ESP SETTINGS ===", Styles.Box);
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Max Distance: {s.MaxDistance:F0}m", Styles.Label);
            s.MaxDistance = GUILayout.HorizontalSlider(s.MaxDistance, 10f, 500f, Styles.Slider, Styles.SliderThumb, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Update Rate: {s.UpdateInterval:F2}s", Styles.Label);
            s.UpdateInterval = GUILayout.HorizontalSlider(s.UpdateInterval, 0.05f, 0.5f, Styles.Slider, Styles.SliderThumb, GUILayout.Width(150));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("Quick Toggle:", Styles.Box);
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("All ON", Styles.Button)) { s.PlayerEnabled = true; s.NpcEnabled = true; s.ItemEnabled = true; }
            if (GUILayout.Button("All OFF", Styles.Button)) { s.PlayerEnabled = false; s.NpcEnabled = false; s.ItemEnabled = false; }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("All Tracers ON", Styles.Button)) { s.PlayerTracers = true; s.NpcTracers = true; s.ItemTracers = true; }
            if (GUILayout.Button("All Tracers OFF", Styles.Button)) { s.PlayerTracers = false; s.NpcTracers = false; s.ItemTracers = false; }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("All Boxes ON", Styles.Button)) { s.PlayerBoxes = true; s.NpcBoxes = true; }
            if (GUILayout.Button("All Boxes OFF", Styles.Button)) { s.PlayerBoxes = false; s.NpcBoxes = false; }
            GUILayout.EndHorizontal();
        }
    }
}
