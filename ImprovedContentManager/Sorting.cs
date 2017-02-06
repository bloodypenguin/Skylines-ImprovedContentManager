using System;
using ImprovedContentManager.Detours;
using ImprovedContentManager.Enums;
using ImprovedContentManager.Extensions;

namespace ImprovedContentManager
{
    public static class Sorting
    {
        public static int SortPluginsByLastUpdate(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (a?.pluginInfo).GetPluginLastModifiedDelta().CompareTo((b?.pluginInfo).GetPluginLastModifiedDelta()));
        }

        public static int SortPluginsByLastSubscribed(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (a1?.pluginInfo).GetPluginCreatedDelta().CompareTo((b1?.pluginInfo).GetPluginCreatedDelta()));
        }

        public static int SortPluginsByActive(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (a1?.pluginInfo).IsEnabled().CompareTo((b1?.pluginInfo).IsEnabled()), true);
        }

        public static int SortPluginsByLocation(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => 0, true); //TODO(earalov): actually compare file locations
        }

        public static int SortAssetsByLastUpdate(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (a?.asset).GetAssetLastModifiedDelta().CompareTo((b?.asset).GetAssetLastModifiedDelta()));
        }

        public static int SortAssetsByLastSubscribed(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (a?.asset).GetAssetCreatedDelta().CompareTo((b?.asset).GetAssetCreatedDelta()));
        }

        public static int SortAssetsByActive(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (b?.asset).IsEnabled().CompareTo((a?.asset).IsEnabled()), true);
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
            var ascending = CategoryContentPanelDetour._pluginSortOrder == SortOrder.Ascending;
            var diff = ascending ? comparsion.Invoke(a, b) : comparsion.Invoke(b, a);
            return diff == 0 || alphabeticalSort ? (ascending ? a.CompareNames(b) : b.CompareNames(a)) : diff;
        }
    }
}