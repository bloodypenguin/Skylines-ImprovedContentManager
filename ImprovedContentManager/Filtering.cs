using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.Packaging;
using ImprovedContentManager.Detours;
using ImprovedContentManager.Enums;
using ImprovedContentManager.Extensions;

namespace ImprovedContentManager
{
    public class Filtering
    {
        public static List<EntryData> FilterDisplayedAssets(List<EntryData> entryDataList, out Dictionary<AssetType, int>  assetTypeIndex)
        {
            var index = Enum.GetValues(typeof(AssetType)).Cast<AssetType>().ToDictionary(assetType => assetType, assetType => 0);

            assetTypeIndex = index;
            return entryDataList.Where(e =>
            {
                index[AssetType.All]++;
                CustomAssetMetaData customAssetMetaData;
                try
                {
                    customAssetMetaData = e?.asset?.Instantiate<CustomAssetMetaData>();
                }
                catch (Exception)
                {
                    index[AssetType.Unknown]++;
                    return CategoryContentPanelDetour._assetFilterMode == AssetType.Unknown;
                }
                if (customAssetMetaData == null)
                {
                    index[AssetType.Unknown]++;
                    return CategoryContentPanelDetour._assetFilterMode == AssetType.Unknown;
                }
                var tags = customAssetMetaData.steamTags;
                foreach (AssetType assetType in Enum.GetValues(typeof(AssetType)))
                {
                    if (assetType == AssetType.All || assetType == AssetType.Unknown)
                    {
                        continue;
                    }
                    if (ContainsTag(tags, assetType.GetEnumDescription<AssetType, AssetTypeAttribute>().SteamTag))
                    {
                        index[assetType]++;
                    }
                }

                return CategoryContentPanelDetour._assetFilterMode == AssetType.All ||
                ContainsTag(tags, CategoryContentPanelDetour._assetFilterMode.GetEnumDescription<AssetType, AssetTypeAttribute>().SteamTag);
            }).ToList();
        }

        private static bool ContainsTag(string[] haystack, string needle)
        {
            if (needle == null || haystack == null)
            {
                return false;
            }
            return haystack.Contains(needle);
        }
    }
}