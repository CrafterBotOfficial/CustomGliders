using CustomGliders.Models;
using CustomGliders.PlayerContentLoaders;
using System.Collections.Generic;
using UnityEngine;

namespace CustomGliders
{
    public class PlayerContentManager : MonoBehaviour
    {
        public Dictionary<string, CustomSkin> Skins = new Dictionary<string, CustomSkin>();

        private SkinLoader skinLoader;


        private async void Start()
        {
            Main.LogSource.LogMessage("Loading skins");
            skinLoader = new SkinLoader();
            var skins = await skinLoader.LoadSkins();
            foreach (var skin in skins)
            {
                string id = string.Join('.', skin.Package.Name, skin.Package.Author);
                if (Skins.ContainsKey(id))
                {
                    Main.LogSource.LogWarning("Duplicate skins found! " + id);
                    continue;
                }
                Skins.Add(id, skin);
            }
        }

#if DEBUG
        private string debug_selectedId;
        private Texture debug_texture = new Texture2D(0, 0);
        private void OnGUI()
        {
            GUILayout.Label("Skins:");
            foreach (var skin in Skins)
            {
                if (GUILayout.Button(skin.Key))
                {
                    debug_selectedId = skin.Key;
                    debug_texture = skin.Value.Texture;
                }
            }
            GUILayout.Box(debug_texture);
        }
#endif
    }
}
