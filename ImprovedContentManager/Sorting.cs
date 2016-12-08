using System;
using System.Collections.Generic;
using ColossalFramework.Packaging;
using ImprovedContentManager.Detours;
using ImprovedContentManager.Enums;
using ImprovedContentManager.Extensions;
using ImprovedContentManager.Util;

namespace ImprovedContentManager
{
    public static class Sorting
    {
        public static void SortDisplayedPlugins(List<EntryData> entryDataList)
        {
            Func<EntryData, EntryData, int> comparerLambda;
            var alphabeticalSort = false;
            switch (CategoryContentPanelDetour._pluginSortMode)
            {
                case SortMode.Alphabetical:
                    comparerLambda = (a, b) => (a).CompareNames(b);
                    alphabeticalSort = true;
                    break;
                case SortMode.LastUpdated:
                    comparerLambda =
                        (a, b) => (a?.pluginInfo).GetPluginLastModifiedDelta().CompareTo((b?.pluginInfo).GetPluginLastModifiedDelta());
                    break;
                case SortMode.LastSubscribed:
                    comparerLambda =
                        (a, b) => (a?.pluginInfo).GetPluginCreatedDelta().CompareTo((b?.pluginInfo).GetPluginCreatedDelta());
                    break;
                case SortMode.Active:
                    comparerLambda = (a, b) => (b?.pluginInfo).IsEnabled().CompareTo((a?.pluginInfo).IsEnabled());
                    break;
                case SortMode.Location: //TODO(earalov): add sorting by location
                    comparerLambda = (a, b) => (a).CompareNames(b);
                    alphabeticalSort = true;
                    break;
                default:
                    throw new Exception($"Unknown sort mode: '{CategoryContentPanelDetour._pluginSortMode}'");
            }
            entryDataList.Sort(new FunctionalComparer<EntryData>((a, b) =>
            {
                var diff =
                    (CategoryContentPanelDetour._pluginSortOrder == SortOrder.Ascending
                        ? comparerLambda
                        : (arg1, arg2) => -comparerLambda(arg1, arg2))(a, b);
                return diff != 0 || alphabeticalSort ? diff : (a).CompareNames(b);
            }));
        }

        public static void SortDisplayedAssets(List<EntryData> entryDataList)
        {

            Func<EntryData, EntryData, int> comparerLambda;
            var alphabeticalSort = false;

            switch (CategoryContentPanelDetour._assetSortMode)
            {
                case SortMode.Alphabetical:
                    comparerLambda = (a, b) => (a).CompareNames(b);
                    alphabeticalSort = true;
                    break;
                case SortMode.LastUpdated:
                    comparerLambda = (a, b) => (a?.asset).GetAssetLastModifiedDelta().CompareTo((b?.asset).GetAssetLastModifiedDelta());
                    break;
                case SortMode.LastSubscribed:
                    comparerLambda = (a, b) => (a?.asset).GetAssetCreatedDelta().CompareTo((b?.asset).GetAssetCreatedDelta());
                    break;
                case SortMode.Active:
                    comparerLambda = (a, b) => (b?.asset).IsEnabled().CompareTo((a?.asset).IsEnabled());
                    break;
                case SortMode.Location:
                    comparerLambda = (a, b) =>
                    {
                        var aIsWorkshop = (a?.asset)?.package?.packagePath?.Contains("workshop") ?? false;
                        var bIsWorkshop = (b?.asset)?.package?.packagePath?.Contains("workshop") ?? false;
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
                    };
                    break;
                default:
                    return;
            }
            entryDataList.Sort(new FunctionalComparer<EntryData>((a, b) =>
            {
                var diff = (CategoryContentPanelDetour._assetSortOrder == SortOrder.Ascending ? comparerLambda : (arg1, arg2) => -comparerLambda(arg1, arg2))(a, b);
                return diff != 0 || alphabeticalSort ? diff : (a).CompareNames(b);

            }));
        }
    }
}