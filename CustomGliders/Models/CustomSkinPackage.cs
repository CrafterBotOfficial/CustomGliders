using System;

namespace CustomGliders.Models
{
    public class CustomSkinPackage
    {
        public string Name;
        public string Author;
        public string Description;

        [NonSerialized] // TODO: Check if this is required
        internal string RelativeTexturePath;
    }
}
