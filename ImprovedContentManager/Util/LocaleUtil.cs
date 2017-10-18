using System.Reflection;
using ColossalFramework;
using ColossalFramework.Globalization;

namespace ImprovedContentManager.Util
{
    public static class LocaleUtil
    {
        public static void AddLocale(string idBase, string key, string value)
        {
            var localeField = typeof(LocaleManager).GetField("m_Locale", BindingFlags.NonPublic | BindingFlags.Instance);
            var locale = (Locale)localeField.GetValue(SingletonLite<LocaleManager>.instance);
            var localeKey = new Locale.Key() { m_Identifier = $"{idBase}", m_Key = key };
            if (!locale.Exists(localeKey))
            {
                locale.AddLocalizedString(localeKey, value);
            }
        }
    }
}