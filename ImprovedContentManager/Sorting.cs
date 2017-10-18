using System;
using System.Linq;
using ColossalFramework.PlatformServices;
using ImprovedContentManager.Enums;
using ImprovedContentManager.Extensions;
using UnityEngine;

namespace ImprovedContentManager
{
    public static class Sorting
    {

        public static int SortPluginsByLastModified(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (a1?.pluginInfo).GetPluginLastModifiedDelta().CompareTo((b1?.pluginInfo).GetPluginLastModifiedDelta()));
        }

        public static int SortPluginsByLastSubscribed(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (a1?.pluginInfo).GetPluginCreatedDelta().CompareTo((b1?.pluginInfo).GetPluginCreatedDelta()));
        }

        public static int SortAssetsByLastModified(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (a1?.asset).GetAssetLastModifiedDelta().CompareTo((b1?.asset).GetAssetLastModifiedDelta()));
        }

        public static int SortAssetsByLastSubscribed(EntryData a, EntryData b)
        {
            return SortDirection(a, b, (a1, b1) => (a1?.asset).GetAssetCreatedDelta().CompareTo((b1?.asset).GetAssetCreatedDelta()));
        }

        public static int SortDirection(EntryData a, EntryData b, Comparison<EntryData> comparsion)
        {
            var diff = comparsion.Invoke(a, b);
            return diff == 0 ? EntryData.SortByName(a, b) : diff;
        }
    }
}