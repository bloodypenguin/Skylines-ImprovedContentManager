using System;
using System.IO;
using ColossalFramework.Packaging;

namespace ImprovedContentManager.Extensions
{
    public static class AssetExtensions
    {
        public static TimeSpan GetAssetLastModifiedDelta(this Package.Asset asset)
        {
            if (asset?.package == null)
            {
                return TimeSpan.MaxValue;
            }
            try
            {
                return DateTime.Now - File.GetLastWriteTime(asset.package.packagePath);
            }
            catch
            {
                return TimeSpan.MaxValue;
            }
        }

        public static TimeSpan GetAssetCreatedDelta(this Package.Asset asset)
        {
            if (asset?.package == null)
            {
                return TimeSpan.MaxValue;
            }
            try
            {
                return DateTime.Now - File.GetCreationTime(asset.package.packagePath);
            }
            catch
            {
                return TimeSpan.MaxValue;
            }
        }

        public static bool IsEnabled(this Package.Asset a)
        {
            return a != null && a.isEnabled;
        }
    }
}