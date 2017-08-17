using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ColossalFramework;
using ColossalFramework.Packaging;
using ImprovedContentManager.Enums;
using ImprovedContentManager.Extensions;
using ImprovedContentManager.Redirection.Attributes;

namespace ImprovedContentManager.Detours
{
    [TargetType(typeof(CategoryContentPanel))]
    public class CategoryContentPanelDetour : CategoryContentPanel
    {

        [RedirectMethod]
        private void RefreshVisibleAssets()
        {
            this.m_VisibleAssets = new List<EntryData>();
            CategoryContentPanel.SearchTokens tokens = new CategoryContentPanel.SearchTokens(this.m_Search);
            foreach (EntryData asset in this.m_Assets)
            {
                if (asset.IsMatch(tokens))
                    this.m_VisibleAssets.Add(asset);
            }
            this.RefreshSelectCountLabel();
            //begin mod
            if (m_AssetType?.ToString() != nameof(CustomAssetMetaData))
            {
                return;
            }
            this.m_VisibleAssets = Filtering.FilterDisplayedAssets(this.m_VisibleAssets);
            //end mod
        }

        [RedirectMethod]
        public new void SetActiveAll(bool enabled)
        {
            //begin mod
            foreach (EntryData asset in this.m_VisibleAssets)
            {
                //end mod
                if (asset.IsActive() != enabled && Singleton<PopsManager>.exists)
                    Singleton<PopsManager>.instance.BufferUGCManaged(asset.GetNameForTelemetry(), enabled);
                asset.SetActive(enabled);
            }
            if (Singleton<PopsManager>.exists)
                Singleton<PopsManager>.instance.Push();
            this.RefreshEntries();
        }

        public static Dictionary<AssetType, int> CountAssets(CategoryContentPanel panel)
        {
            var index = Enum.GetValues(typeof(AssetType)).Cast<AssetType>().ToDictionary(assetType => assetType, assetType => 0);

            var entryDataList = (List<EntryData>)typeof(CategoryContentPanel).GetField("m_Assets",
                BindingFlags.NonPublic | BindingFlags.Instance).GetValue(panel);
            foreach (var e in entryDataList)
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
                    continue;
                }
                if (customAssetMetaData == null)
                {
                    index[AssetType.Unknown]++;
                    continue;
                }
                var tags = customAssetMetaData.steamTags;
                foreach (AssetType assetType in Enum.GetValues(typeof(AssetType)))
                {
                    if (assetType == AssetType.All || assetType == AssetType.Unknown)
                    {
                        continue;
                    }
                    if (Filtering.ContainsTag(tags,
                        assetType.GetEnumDescription<AssetType, AssetTypeAttribute>().SteamTag))
                    {
                        index[assetType]++;
                    }
                }
            }
            return index;
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
        private void RefreshSelectCountLabel()
        {
            UnityEngine.Debug.LogError("CategoryContentPanelDetour - Failed to detour RefreshEntries()");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [RedirectReverse]
        private void RefreshEntries()
        {
            UnityEngine.Debug.LogError("CategoryContentPanelDetour - Failed to detour RefreshEntries()");
        }

        private List<EntryData> m_Assets => (List<EntryData>)typeof(CategoryContentPanel).GetField("m_Assets",
            BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);

        private List<EntryData> m_VisibleAssets
        {
            get
            {
                return (List<EntryData>)typeof(CategoryContentPanel).GetField("m_VisibleAssets",
                    BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            }
            set
            {
                typeof(CategoryContentPanel).GetField("m_VisibleAssets",
                    BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, value);
            }
        }


        private Package.AssetType m_AssetType => (Package.AssetType)typeof(CategoryContentPanel).GetField("m_AssetType",
            BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);

        private string m_Search => (string)typeof(CategoryContentPanel).GetField("m_Search",
    BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
    }
}