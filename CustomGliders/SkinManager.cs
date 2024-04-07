using CustomGliders.Models;
using CustomGliders.PlayerContentLoaders;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace CustomGliders
{
    public class SkinManager : MonoBehaviour
    {
        public const string GliderSkinKey = "CustomGliderSkin";

        public static SkinManager Instance;

        public Dictionary<string, CustomSkin> Skins = new Dictionary<string, CustomSkin>();
        public string EquipedSkin;

        private SkinLoader skinLoader;

        private Texture defaultLeafTexture;

        private void Start()
        {
            Instance = this;

            Main.LogSource.LogMessage("Loading skins");
            skinLoader = new SkinLoader();
            var skins = skinLoader.LoadSkins();
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

        #region Skin management
        public void EquipSkin(string id)
        {
            Main.LogSource.LogMessage($"Equiping {id}");
            UnequipSkin();
        }

        public void EquipSkin(Player player, string id)
        {
            UnequipSkin();
            
        }

        private void UnequipSkin()
        {
            if (EquipedSkin != string.Empty)
            {
                Main.LogSource.LogMessage($"Unequiping {EquipedSkin}");
                foreach (var glider in GameObject.FindObjectsOfType<GliderHoldable>())
                {
                    Main.LogSource.LogDebug("Enumerating " + glider.name);
                    var material = glider.transform.Find("").GetComponent<MeshRenderer>().material;
                    if (glider.ownershipGuard is object && glider.ownershipGuard.currentOwner.CustomProperties.TryGetValue(GliderSkinKey, out object expectedTextureId) && expectedTextureId is string str && str == material.mainTexture.name)
                    {
                        Main.LogSource.LogDebug("Expected texture, skipping");
                        continue;
                    }
                    material.mainTexture = defaultLeafTexture;
                }
                EquipedSkin = string.Empty;
            }
        }

        private void PatchAllLeafs()
        {
            // Todo impliment
        }
        #endregion

        private bool IsHoldingGlider()
        {
            return EquipmentInteractor.hasInstance && EquipmentInteractor.instance.leftHandHeldEquipment is GliderHoldable || EquipmentInteractor.instance.rightHandHeldEquipment is GliderHoldable;
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
