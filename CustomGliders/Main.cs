using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace CustomGliders
{
    [BepInPlugin("crafterbot.gorillatag.customgliders", "Custom Gliders", "0.0.1")]
    [BepInDependency("org.legoandmars.gorillatag.utilla")] 
    public class Main : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;

        private void Start()
        {
            LogSource = Logger;
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        private void OnGameInitialized(object sender, System.EventArgs e)
        {
            if (NetworkSystem.Instance is not NetworkSystemPUN)
            {
                LogSource.LogFatal("Detected Fusion switch, aborting setup.");
                return;
            }
            Harmony.CreateAndPatchAll(typeof(Main).Assembly);
            new UnityEngine.GameObject("CustomGliderManager").AddComponent<SkinManager>();
        }
    }
}