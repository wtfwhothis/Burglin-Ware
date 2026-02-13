using UnityEngine;
using GnomeCheat.Actions;
using GnomeCheat.Utils;

namespace GnomeCheat.UI.Tabs
{
    public class TeleportTab
    {
        public void Draw()
        {
            GUILayout.Label("=== TELEPORT (LOCAL) ===", Styles.Box);
            GUILayout.Space(10);

            var local = PlayerHelper.GetLocalPlayer();
            if (GUILayout.Button("Teleport to Spawn", Styles.Button)) PlayerActions.TeleportToSpawn(local);
            if (GUILayout.Button("Teleport Forward 10m", Styles.Button)) PlayerActions.TeleportForward(local, 10f);
            if (GUILayout.Button("Teleport Up 5m", Styles.Button)) PlayerActions.TeleportUp(local, 5f);
        }
    }
}
