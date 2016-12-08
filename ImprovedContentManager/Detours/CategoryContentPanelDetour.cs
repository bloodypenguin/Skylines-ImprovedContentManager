using System;
using System.Collections.Generic;
using System.Reflection;
using ColossalFramework;
using ColossalFramework.Packaging;
using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
using ImprovedContentManager.Enums;
using ImprovedContentManager.Redirection.Attributes;

namespace ImprovedContentManager.Detours
{
    [TargetType(typeof(CategoryContentPanel))]
    public class CategoryContentPanelDetour : CategoryContentPanel
    {
        public static SortMode _pluginSortMode = SortMode.Alphabetical;
        public static SortOrder _pluginSortOrder = SortOrder.Ascending;

        public static SortMode _assetSortMode = SortMode.Alphabetical;
        public static SortOrder _assetSortOrder = SortOrder.Ascending;
        public static AssetType _assetFilterMode = AssetType.All;

        public static Dictionary<AssetType, int> _assetTypeIndex = new Dictionary<AssetType, int>();
        public static bool dontRefreshLabels = false;
        public static bool refreshLabelsFlag = false;

        [RedirectMethod]
        private List<EntryData> RefreshAssetsImpl()
        {
            List<EntryData> entryDataList = new List<EntryData>();
            Package.AssetType[] assetTypeArray = new Package.AssetType[1]
            {
                m_AssetType
            };
            foreach (Package.Asset filterAsset in PackageManager.FilterAssets(assetTypeArray))
            {
                if (!this.m_OnlyMain || filterAsset.isMainAsset)
                {
                    EntryData entryData = new EntryData(filterAsset);
                    entryDataList.Add(entryData);
                }
            }
            //begin mod
            if (m_AssetType?.ToString() != nameof(CustomAssetMetaData))
            {
                return entryDataList;
            }
            entryDataList = Filtering.FilterDisplayedAssets(entryDataList, out _assetTypeIndex);
            if (!dontRefreshLabels)
            {
                refreshLabelsFlag = true;
            }
            Sorting.SortDisplayedAssets(entryDataList);
            //end mod
            return entryDataList;
        }

        [RedirectMethod]
        private List<EntryData> RefreshAssetsModImpl()
        {
            List<EntryData> entryDataList = new List<EntryData>();
            foreach (PluginManager.PluginInfo info in Singleton<PluginManager>.instance.GetPluginsInfo())
            {
                EntryData entryData = new EntryData(info);
                entryDataList.Add(entryData);
            }
            //begin mod
            Sorting.SortDisplayedPlugins(entryDataList);
            //end mod
            return entryDataList;
        }



        [RedirectMethod]
        private List<EntryData> RefreshAssetsWorkshopImpl()
        {
            List<EntryData> entryDataList = new List<EntryData>();
            PublishedFileId[] subscribedItems = PlatformService.workshop.GetSubscribedItems();
            if (subscribedItems != null)
            {
                foreach (PublishedFileId id in subscribedItems)
                {
                    EntryData entryData = new EntryData(id);
                    entryDataList.Add(entryData);
                }
            }
            return entryDataList;
        }

        private Package.AssetType m_AssetType => (Package.AssetType)typeof(CategoryContentPanel).GetField("m_AssetType",
            BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);

        private bool m_OnlyMain => (bool)typeof(CategoryContentPanel).GetField("m_OnlyMain",
            BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
    }
}