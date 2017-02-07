using System;
using System.Linq;
using ImprovedContentManager.Enums;
using ImprovedContentManager.Extensions;
using UnityEngine;

namespace ImprovedContentManager
{
    public static class Sorting
    {
        public static SortOrder _pluginSortOrder = SortOrder.Ascending;
        public static SortOrder _assetSortOrder = SortOrder.Ascending;


        public static int SortPluginsByLastUpdate(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (a1?.pluginInfo).GetPluginLastModifiedDelta().CompareTo((b1?.pluginInfo).GetPluginLastModifiedDelta()));
        }

        public static int SortPluginsByLastSubscribed(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (a1?.pluginInfo).GetPluginCreatedDelta().CompareTo((b1?.pluginInfo).GetPluginCreatedDelta()));
        }

        public static int SortPluginsByActive(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (b1?.pluginInfo).IsEnabled().CompareTo((a1?.pluginInfo).IsEnabled()), true);
        }

        public static int SortPluginsByLocation(EntryData a, EntryData b) //TODO(earalov): fix sorting
        {
            return SortDirection(a, b, (a1, b1) =>
            {
                Debug.Log((a1?.pluginInfo)?.GetAssemblies().First()?.GetFiles().First()?.Name);
                var aIsWorkshop = (a1?.pluginInfo)?.GetAssemblies().First()?.GetFiles().First()?.Name?.Contains("workshop") ?? false;
                var bIsWorkshop = (b1?.pluginInfo)?.GetAssemblies().First()?.GetFiles().First()?.Name?.Contains("workshop") ?? false;
                if (aIsWorkshop && bIsWorkshop)
                {
                    return 0;
                }

                if (aIsWorkshop)
                {
                    return 1;
                }

                if (bIsWorkshop)
                {
                    return -1;
                }

                return 0;
            }, true);
        }

        public static int SortAssetsByLastUpdate(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (a1?.asset).GetAssetLastModifiedDelta().CompareTo((b1?.asset).GetAssetLastModifiedDelta()));
        }

        public static int SortAssetsByLastSubscribed(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (a1?.asset).GetAssetCreatedDelta().CompareTo((b1?.asset).GetAssetCreatedDelta()));
        }

        public static int SortAssetsByActive(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (b1?.asset).IsEnabled().CompareTo((a1?.asset).IsEnabled()), true);
        }

        public static int SortAssetsByLocation(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) =>
            {
                var aIsWorkshop = (a1?.asset)?.package?.packagePath?.Contains("workshop") ?? false;
                var bIsWorkshop = (b1?.asset)?.package?.packagePath?.Contains("workshop") ?? false;
                if (aIsWorkshop && bIsWorkshop)
                {
                    return 0;
                }

                if (aIsWorkshop)
                {
                    return 1;
                }

                if (bIsWorkshop)
                {
                    return -1;
                }

                return 0;
            }, true);
        }

        public static int SortDirection(EntryData a, EntryData b, Comparison<EntryData> comparsion, bool alphabeticalSort = false)
        {
            var ascending = _pluginSortOrder == SortOrder.Ascending;
            var diff = @ascending ? comparsion.Invoke(a, b) : comparsion.Invoke(b, a);
            return diff == 0 && alphabeticalSort ? a.CompareNames(b) : diff;
        }
    }
}