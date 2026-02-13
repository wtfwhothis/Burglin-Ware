using UnityEngine;

namespace GnomeCheat.ESP
{
    public class ESPSettings
    {
        // Player ESP
        public bool PlayerEnabled;
        public bool PlayerBoxes = true;
        public bool PlayerNames = true;
        public bool PlayerHealth = true;
        public bool PlayerDistance = true;
        public bool PlayerTracers;

        // NPC ESP
        public bool NpcEnabled;
        public bool NpcBob = true;
        public bool NpcHuman = true;
        public bool NpcSpider = true;
        public bool NpcBoxes = true;
        public bool NpcNames = true;
        public bool NpcDistance = true;
        public bool NpcTracers;

        // Item ESP
        public bool ItemEnabled;
        public bool ItemNames = true;
        public bool ItemDistance = true;
        public bool ItemTracers;

        // General
        public float MaxDistance = 200f;
        public float UpdateInterval = 0.1f;

        // Colors
        public Color ColorPlayerOther = Color.green;
        public Color ColorPlayerLocal = Color.cyan;
        public Color ColorBob = Color.yellow;
        public Color ColorHuman = new Color(1f, 0.5f, 0f);
        public Color ColorSpider = Color.red;
        public Color ColorItem = new Color(0.5f, 0.5f, 1f);
    }
}
