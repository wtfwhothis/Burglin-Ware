using UnityEngine;
using MelonLoader;
using GnomeCheat.UI;
using GnomeCheat.ESP;

namespace GnomeCheat.Core
{
    public class GnomeCheatMod : MelonMod
    {
        public static GnomeCheatMod Instance { get; private set; }

        private MenuManager menuManager;
        private FeatureManager featureManager;
        private ESPRenderer espRenderer;
        private Overlay overlay;

        public override void OnInitializeMelon()
        {
            Instance = this;
            featureManager = new FeatureManager();
            espRenderer = new ESPRenderer();
            menuManager = new MenuManager(featureManager, espRenderer);
            overlay = new Overlay();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
                menuManager.ToggleMenu();

            if (Input.GetKeyDown(KeyCode.N))
                menuManager.ToggleMenu();

            featureManager.OnUpdate();
            Actions.PlayerActions.UpdateTornado();
            Actions.PlayerActions.UpdateFreeze();
            Actions.PlayerActions.UpdateSpectate();
            Actions.RCCarActions.UpdateKamikaze();
        }

        public override void OnGUI()
        {
            overlay.Draw();
            espRenderer.Draw();

            if (menuManager.IsVisible)
                menuManager.Draw();
        }

        public static void Log(string msg) => Instance?.LoggerInstance.Msg(msg);
        public static void LogError(string msg) => Instance?.LoggerInstance.Error(msg);
    }
}