using System.ComponentModel;

namespace ImprovedContentManager.Enums
{
    public enum SortMode
    {
        [Description("Name")]
        Alphabetical = 0,
        [Description("Last updated")]
        LastUpdated = 1,
        [Description("Last subscribed")]
        LastSubscribed = 2,
        [Description("Active")]
        Active = 3,
        [Description("Location")]
        Location = 4
    }
}