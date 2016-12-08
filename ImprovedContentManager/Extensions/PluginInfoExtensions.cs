using System;
using System.IO;
using ColossalFramework.Plugins;

namespace ImprovedContentManager.Extensions
{
    public static class PluginInfoExtensions
    {
        public static TimeSpan GetPluginLastModifiedDelta(this PluginManager.PluginInfo plugin)
        {
            if (plugin == null)
            {
                return TimeSpan.MaxValue;
            }
            try
            {
                var lastModified = DateTime.MinValue;
                foreach (var file in Directory.GetFiles(plugin.modPath))
                {
                    if (Path.GetExtension(file) != ".dll")
                    {
                        continue;
                    }
                    var tmp = File.GetLastWriteTime(file);
                    if (tmp > lastModified)
                    {
                        lastModified = tmp;
                    }
                }
                return DateTime.Now - lastModified;
            }
            catch
            {
                return TimeSpan.MaxValue;
            }

        }

        public static TimeSpan GetPluginCreatedDelta(this PluginManager.PluginInfo plugin)
        {
            if (plugin == null)
            {
                return TimeSpan.MaxValue;
            }

            try
            {
                var created = DateTime.MinValue;
                foreach (var file in Directory.GetFiles(plugin.modPath))
                {
                    if (Path.GetExtension(file) != ".dll") continue;
                    var tmp = File.GetCreationTime(file);
                    if (tmp > created)
                    {
                        created = tmp;
                    }
                }
                return DateTime.Now - created;
            }
            catch
            {
                return TimeSpan.MaxValue;
            }

        }



        public static bool IsEnabled(this PluginManager.PluginInfo a)
        {
            return a != null && a.isEnabled;
        }
    }
}