using System;
using System.Collections.Generic;
using System.Linq;
using ImprovedContentManager.Enums;
using ImprovedContentManager.Extensions;

namespace ImprovedContentManager
{
    public class Filtering
    {
        public static AssetType _assetFilterMode = AssetType.All;

        public static List<EntryData> FilterDisplayedAssets(List<EntryData> entryDataList)
        {
            return entryDataList.Where(e =>
            {
                CustomAssetMetaData customAssetMetaData;
                try
                {
                    customAssetMetaData = e?.asset?.Instantiate<CustomAssetMetaData>();
                }
                catch (Exception)
                {
                    return _assetFilterMode == AssetType.Unknown;
                }
                if (customAssetMetaData == null)
                {
                    return _assetFilterMode == AssetType.Unknown;
                }
                var tags = customAssetMetaData.steamTags;
                return _assetFilterMode == AssetType.All ||
                ContainsTag(tags, _assetFilterMode.GetEnumDescription<AssetType, AssetTypeAttribute>().SteamTag);
            }).ToList();
        }

        public static bool ContainsTag(string[] haystack, string needle)
        {
            if (needle == null || haystack == null)
            {
                return false;
            }
            return haystack.Contains(needle);
        }
    }
}