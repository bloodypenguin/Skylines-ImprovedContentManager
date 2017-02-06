using System.Reflection;
using ColossalFramework.UI;
using UnityEngine;

namespace ImprovedContentManager.UI
{
    public static class PanelUtil
    {
        public static CategoryContentPanel GetCategoryContainer(string fieldName)
        {
            var contentManagerPanelGameObject = GameObject.Find("(Library) ContentManagerPanel");
            var contentManagerPanel = contentManagerPanelGameObject?.GetComponent<ContentManagerPanel>();
            var uiComponent =
                (UIComponent)contentManagerPanel?.GetType()
                    .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(contentManagerPanel);
            return uiComponent?.GetComponent<CategoryContentPanel>();
        }
    }
}