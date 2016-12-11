using System;
using System.Collections.Generic;
using System.Reflection;
using ColossalFramework;
using ColossalFramework.Packaging;
using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
using ImprovedContentManager.Enums;
using ImprovedContentManager.Redirection.Attributes;
using UnityEngine;

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
        private void RefreshAssetsImpl()
        {
            this.m_Assets = new List<EntryData>();
            foreach (Package.Asset filterAsset in PackageManager.FilterAssets(this.m_AssetType))
            {
                if (!this.m_OnlyMain || filterAsset.isMainAsset)
                    this.m_Assets.Add(new EntryData(filterAsset));
            }
            //begin mod
            if (m_AssetType?.ToString() != nameof(CustomAssetMetaData))
            {
                return;
            }
            this.m_Assets = Filtering.FilterDisplayedAssets(this.m_Assets, out _assetTypeIndex);
            if (!dontRefreshLabels)
            {
                refreshLabelsFlag = true;
            }
            Sorting.SortDisplayedAssets(this.m_Assets);
            //end mod
        }


        [RedirectMethod]
        private void RefreshAssetsModImpl()
        {
            this.m_Assets = new List<EntryData>();
            foreach (PluginManager.PluginInfo info in Singleton<PluginManager>.instance.GetPluginsInfo())
            {
                try
                {
                    this.m_Assets.Add(new EntryData(info));
                }
                catch
                {
                    Debug.LogError((object)("Failed to create content manager entry for mod " + info.name));
                }
            }
            //begin mod
            Sorting.SortDisplayedPlugins(this.m_Assets);
            //end mod
        }

        private List<EntryData> m_Assets
        {
            get
            {
                return (List<EntryData>)typeof(CategoryContentPanel).GetField("m_Assets",
            BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            }
            set
            {
                typeof(CategoryContentPanel).GetField("m_Assets",
            BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, value);
            }
        }

        private Package.AssetType m_AssetType => (Package.AssetType)typeof(CategoryContentPanel).GetField("m_AssetType",
            BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);

        private bool m_OnlyMain => (bool)typeof(CategoryContentPanel).GetField("m_OnlyMain",
            BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
    }
}