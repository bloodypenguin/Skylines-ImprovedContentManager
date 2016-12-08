using System;

namespace ImprovedContentManager
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class AssetTypeAttribute : Attribute
    {

        public AssetTypeAttribute(string steamTag, string spriteName)
        {
            SteamTag = steamTag;
            SpriteName = spriteName;
        }

        public string SteamTag { get; }
        public string SpriteName { get; }
    }
}