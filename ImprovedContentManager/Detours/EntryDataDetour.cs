using ColossalFramework.Packaging;
using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
using ImprovedContentManager.Redirection.Attributes;

namespace ImprovedContentManager.Detours
{
    [TargetType(typeof(EntryData))]
    public class EntryDataDetour : EntryData
    {
        [RedirectMethod]
        public new static int SortByPath(EntryData a, EntryData b)
        {
            if (a.IsBuiltin() && !b.IsBuiltin())
                return -1;
            if (!a.IsBuiltin() && b.IsBuiltin())
                return 1;
            if (a.IsBuiltin() && b.IsBuiltin())
                return EntryData.SortByName(a, b);
            if (a.publishedFileId == PublishedFileId.invalid && b.publishedFileId != PublishedFileId.invalid)
                return 1;
            if (a.publishedFileId != PublishedFileId.invalid && b.publishedFileId == PublishedFileId.invalid)
                return -1;
            if (a.publishedFileId != PublishedFileId.invalid && b.publishedFileId != PublishedFileId.invalid)
                return EntryData.SortByName(a, b);
            //begin mod
            string packagePath1 = a.package?.packagePath;
            string packagePath2 = b.package?.packagePath;
            //end mod
            if (packagePath1 == packagePath2)
                return EntryData.SortByName(a, b);
            return string.Compare(packagePath1, packagePath2);
        }

        public EntryDataDetour(Package.Asset asset) : base(asset)
        {
            //to satisfy compiler
        }

        public EntryDataDetour(PluginManager.PluginInfo info) : base(info)
        {
            //to satisfy compiler
        }
    }
}