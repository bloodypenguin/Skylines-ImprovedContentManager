using System;
using ICities;
using ImprovedContentManager.Redirection;
using ImprovedContentManager.UI;
using ImprovedContentManager.Util;

namespace ImprovedContentManager
{
    public class Mod : IUserMod
    {
        public Mod()
        {
            try
            {
                AssemblyRedirector.Deploy();
                LocaleUtil.AddLocale("PANEL_SORT_UPDATED", null, "Time Updated");
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
                AssemblyRedirector.Revert();
            }
        }

        public string Name
        {
            get
            {
                try
                {
                    ImprovedModsPanel.Bootstrap();
                    ImprovedAssetsPanel.Bootstrap();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
                return "Improved Content Manager";
            }
        }

        public string Description => "Enhanced mods and assets panels";
    }
}