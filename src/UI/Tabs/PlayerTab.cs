using UnityEngine;
using GnomeCheat.Core;
using GnomeCheat.Actions;
using GnomeCheat.Utils;

namespace GnomeCheat.UI.Tabs
{
    public class PlayerTab
    {
        private readonly FeatureManager features;
        private Vector2 scrollPos;

        public PlayerTab(FeatureManager features) => this.features = features;

        public void Draw()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.Label("=== PLAYER OPTIONS (LOCAL) ===", Styles.Box);
            GUILayout.Space(10);

            var local = PlayerHelper.GetLocalPlayer();

            bool newGod = GUILayout.Toggle(features.GodModeEnabled, "God Mode", Styles.Toggle);
            if (newGod != features.GodModeEnabled) { features.GodModeEnabled = newGod; GnomeCheatMod.Log($"God Mode: {newGod}"); }

            bool newNoRag = GUILayout.Toggle(features.NoRagdollEnabled, "No Ragdoll", Styles.Toggle);
            if (newNoRag != features.NoRagdollEnabled) { features.NoRagdollEnabled = newNoRag; GnomeCheatMod.Log($"No Ragdoll: {newNoRag}"); }

            // bool newInvis = GUILayout.Toggle(features.InvisibilityEnabled, "Invisibility (Network)", Styles.Toggle);
            // if (newInvis != features.InvisibilityEnabled) { features.InvisibilityEnabled = newInvis; GnomeCheatMod.Log($"Invisibility: {newInvis}"); }

            bool newHands = GUILayout.Toggle(features.InfiniteHandsEnabled, "Infinite Arms (Max Reach + No Break)", Styles.Toggle);
            if (newHands != features.InfiniteHandsEnabled) { features.InfiniteHandsEnabled = newHands; GnomeCheatMod.Log($"Infinite Arms: {newHands}"); }

            // bool newStrong = GUILayout.Toggle(features.StrongArmsEnabled, "Strong Arms (Carry Everything)", Styles.Toggle);
            // if (newStrong != features.StrongArmsEnabled) { features.StrongArmsEnabled = newStrong; GnomeCheatMod.Log($"Strong Arms: {newStrong}"); }

            bool newFly = GUILayout.Toggle(features.FlyModeEnabled, "Fly Mode (WASD + Space/Ctrl)", Styles.Toggle);
            if (newFly != features.FlyModeEnabled) features.ToggleFlyMode(newFly);

            // bool newNoclip = GUILayout.Toggle(features.NoclipEnabled, "Noclip (Walk Through Walls)", Styles.Toggle);
            // if (newNoclip != features.NoclipEnabled) features.ToggleNoclip(newNoclip);

            if (features.FlyModeEnabled || features.NoclipEnabled)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Fly Speed: {features.FlySpeed:F1}", Styles.Label);
                features.FlySpeed = GUILayout.HorizontalSlider(features.FlySpeed, 1f, 50f, Styles.Slider, Styles.SliderThumb, GUILayout.Width(150));
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Full Heal", Styles.Button)) PlayerActions.HealPlayer(local);
            if (GUILayout.Button("Kill Player", Styles.Button)) PlayerActions.KillPlayer(local);
            GUILayout.Space(10);
            if (GUILayout.Button("Force Ragdoll", Styles.Button)) PlayerActions.ForceRagdoll(local);
            if (GUILayout.Button("Unragdoll", Styles.Button)) PlayerActions.UnRagdoll(local);

            GUILayout.Space(10);
            GUILayout.Label("=== SPEED ===", Styles.Box);
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Speed: {features.SpeedMultiplier:F1}x", Styles.Label);
            features.SpeedMultiplier = GUILayout.HorizontalSlider(features.SpeedMultiplier, 0.5f, 5f, Styles.Slider, Styles.SliderThumb, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("1x", Styles.Button)) features.SpeedMultiplier = 1f;
            if (GUILayout.Button("2x", Styles.Button)) features.SpeedMultiplier = 2f;
            if (GUILayout.Button("3x", Styles.Button)) features.SpeedMultiplier = 3f;
            if (GUILayout.Button("5x", Styles.Button)) features.SpeedMultiplier = 5f;
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("=== TOGGLES ===", Styles.Box);
            GUILayout.Space(5);
            features.AntiKidnapEnabled = GUILayout.Toggle(features.AntiKidnapEnabled, "Anti-Kidnap (Auto Untie)", Styles.Toggle);
            features.InfiniteStaminaEnabled = GUILayout.Toggle(features.InfiniteStaminaEnabled, "Infinite Stamina", Styles.Toggle);

            GUILayout.Space(10);
            GUILayout.Label("=== DISGUISE ===", Styles.Box);
            GUILayout.Space(5);

            bool newBob = GUILayout.Toggle(features.BobDisguiseEnabled, "Bob Disguise", Styles.Toggle);
            if (newBob != features.BobDisguiseEnabled) { features.BobDisguiseEnabled = newBob; if (newBob) features.MoleDisguiseEnabled = false; }

            GUILayout.EndScrollView();
        }
    }
}