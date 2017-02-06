using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using ColossalFramework;
using ColossalFramework.Packaging;
using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ImprovedContentManager.Enums;
using ImprovedContentManager.Extensions;
using ImprovedContentManager.Redirection.Attributes;
using UnityEngine;

namespace ImprovedContentManager.Detours
{
    //TODO(earalov): add direction support for standard sortings
    [TargetType(typeof(CategoryContentPanel))]
    public class CategoryContentPanelDetour : CategoryContentPanel
    {
        public static SortOrder _pluginSortOrder = SortOrder.Ascending;

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
                if (!(this.m_AssetType == UserAssetType.SaveGameMetaData) || !PackageHelper.IsDemoModeSave(filterAsset))
                {
                    if (this.m_OnlyMain)
                    {
                        if (!filterAsset.isMainAsset)
                            continue;
                    }
                    try
                    {
                        this.m_Assets.Add(new EntryData(filterAsset));
                    }
                    catch
                    {
                        Debug.LogError((object)("Failed to create content manager entry for asset " + filterAsset.name));
                    }
                }
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
            //end mod
        }

        [RedirectMethod]
        public void SetActiveAll(bool enabled)
        {
            //begin mod
            using (List<EntryData>.Enumerator enumerator = this.m_VisibleAssets.GetEnumerator())
            {
                //end mod
                while (enumerator.MoveNext())
                    enumerator.Current.SetActive(enabled);
            }
            this.RefreshEntries();
        }

        [RedirectMethod]
        public static int SortByName(CategoryContentPanel panel, EntryData a, EntryData b)
        {
            //begin mod
            return Sorting.SortDirection(a, b, (a1, b1) => a1.CompareNames(b1));
            //end mod
        }

        [RedirectMethod]
        public static int SortByAuthor(CategoryContentPanel panel, EntryData a, EntryData b)
        {
            //begin mod
            return Sorting.SortDirection(a, b, (a1, b1) => a1.CompareAuthors(b1), true);
            //end mod
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [RedirectReverse]
        private void RefreshEntries()
        {
            UnityEngine.Debug.LogError("CategoryContentPanelDetour - Failed to detour RefreshEntries()");
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

        private List<EntryData> m_VisibleAssets => (List<EntryData>)typeof(CategoryContentPanel).GetField("m_VisibleAssets",
            BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);


        private Package.AssetType m_AssetType => (Package.AssetType)typeof(CategoryContentPanel).GetField("m_AssetType",
            BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);

        private bool m_OnlyMain => (bool)typeof(CategoryContentPanel).GetField("m_OnlyMain",
            BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
    }
}