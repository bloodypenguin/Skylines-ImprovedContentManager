using System;
using ColossalFramework.PlatformServices;

namespace ImprovedContentManager.Extensions
{
    public static class EntryDataExtensions
    {
        public static int CompareNames(this EntryData a, EntryData b)
        {
            if (a?.entryName == null)
            {
                return 1;
            }

            if (b?.entryName == null)
            {
                return -1;
            }
            return string.Compare(a.entryName, b.entryName, StringComparison.InvariantCultureIgnoreCase);
        }

        public static int CompareAuthors(this EntryData a, EntryData b)
        {
            if (a.publishedFileId != PublishedFileId.invalid && b.publishedFileId == PublishedFileId.invalid)
                return 1;
            if (a.publishedFileId == PublishedFileId.invalid && b.publishedFileId != PublishedFileId.invalid)
                return -1;
            if (a.publishedFileId != PublishedFileId.invalid && b.publishedFileId != PublishedFileId.invalid)
            {
                if (a.authorName == null)
                {
                    return 1;
                }
                if (b.authorName == null)
                {
                    return -1;
                }
                return a.authorName.CompareTo(b.authorName);
            }
            if (!(a.publishedFileId == PublishedFileId.invalid) || !(b.publishedFileId == PublishedFileId.invalid))
                return 0;
            if (a.IsBuiltin() && !b.IsBuiltin())
                return -1;
            if (!a.IsBuiltin() && b.IsBuiltin())
                return 1;
            return 0;
        }
    }
}