using ICities;
using ImprovedContentManager.Redirection;
using ImprovedContentManager.UI;

namespace ImprovedContentManager
{
    public class Mod : IUserMod
    {
        public Mod()
        {
            AssemblyRedirector.Deploy();
        }

        public string Name
        {
            get
            {
                ImprovedModsPanel.Bootstrap();
                ImprovedAssetsPanel.Bootstrap();
                return "Improved Content Manager";
            }
        }

        public string Description => "Enhanced mods and assets panels";
    }
}
