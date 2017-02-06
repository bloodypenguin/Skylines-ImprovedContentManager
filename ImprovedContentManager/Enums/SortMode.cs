using System.ComponentModel;

namespace ImprovedContentManager.Enums
{
    public enum SortMode
    {
        [Description("Last updated")]
        LastUpdated = 0,
        [Description("Last subscribed")]
        LastSubscribed = 1,
        [Description("Active")]
        Active = 2,
        [Description("Location")]
        Location = 3
    }
}