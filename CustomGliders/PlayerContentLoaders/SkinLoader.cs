using CustomGliders.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomGliders.PlayerContentLoaders
{
    public class SkinLoader : PlayerContentLoader
    {
        public SkinLoader()
        {
            base.ContentDirectoryPath = Path.Combine(Path.GetDirectoryName(typeof(Main).Assembly.Location), "Skins");
            base.FileExtension = "skin";
        }

        public CustomSkin[] LoadSkins()
        {
            List<CustomSkin> skins = new List<CustomSkin>();
            string[] files = GetFiles();
            foreach (string file in files)
            {
                try
                {
                    PlayerContent<CustomSkinPackage>? userContent = LoadZIP<CustomSkinPackage>(file, "texture.png");
                    if (userContent.HasValue)
                    {
                        var texture = new Texture2D(0, 0);
                        texture.filterMode = FilterMode.Point;
                        if (!texture.LoadImage(userContent.Value.ContentResourceFile, true))
                            Main.LogSource.LogWarning("Couldn't parse image! " + file);

                        skins.Add(new() { Package = userContent.Value.Package, Texture = texture });
                    }
                }
                catch (Exception ex)
                {
                    Main.LogSource.LogError($"Unexpected error occured while opening {file} {ex}");
                }
            }
            return skins.ToArray();
        }
    }
}
