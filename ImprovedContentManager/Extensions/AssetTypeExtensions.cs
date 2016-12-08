using System;

namespace ImprovedContentManager.Extensions
{
    public static class AssetTypeExtensions
    {
        public static TR GetEnumDescription<T, TR>(this T value) where TR : Attribute
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            var fi = value.GetType().GetField(value.ToString());
            var attributes = fi.GetCustomAttributes(typeof(TR), false);
            if (attributes.Length > 0)
                return (TR)attributes[0];
            throw new Exception($"No attributes of type {typeof(TR)} found for ${value}");
        }
    }
}