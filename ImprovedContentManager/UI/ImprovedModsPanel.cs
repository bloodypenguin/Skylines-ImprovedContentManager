using System;
using System.Collections.Generic;
using System.Reflection;
using ColossalFramework;
using ColossalFramework.UI;
using ImprovedContentManager.Detours;
using ImprovedContentManager.Enums;
using ImprovedContentManager.Util;
using UnityEngine;
using Object = UnityEngine.Object;
using UIUtils = ImprovedContentManager.Util.UIUtils;

namespace ImprovedContentManager.UI
{

    public class ImprovedModsPanel : MonoBehaviour
    {
        private static CategoryContentPanel _categoryContainer;

        private static bool _ui_initialized;
        private static bool _itemsSorted;

        private static UIPanel _sortModePanel;
        private static UIDropDown _sortOrderDropDown;
        private static UILabel _sortOrderLabel;

        public static void Bootstrap()
        {
            var syncObject = GameObject.Find("ImprovedModsPanelSyncObject");
            if (syncObject != null)
            {
                return;
            }
            syncObject = new GameObject("ImprovedModsPanelSyncObject");
            syncObject.AddComponent<ImprovedModsPanel>();
        }


        public void Update()
        {
            if (Singleton<LoadingManager>.instance.m_loadedEnvironment != null)
            {
                Destroy(gameObject);
            }
            if (_ui_initialized)
            {
                if (!_itemsSorted && _categoryContainer.gameObject.GetComponent<UIComponent>().isVisible)
                {
                    _categoryContainer.GetType().GetField("m_SortImpl", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_categoryContainer, new Comparison<EntryData>(
                        (a, b) => CategoryContentPanelDetour.SortByName(_categoryContainer, a, b)));
                    RefreshMods();
                    _itemsSorted = true;
                }
                return;
            }
            var contentManagerPanelGameObject = GameObject.Find("(Library) ContentManagerPanel");
            var contentManagerPanel = contentManagerPanelGameObject?.GetComponent<ContentManagerPanel>();
            if (contentManagerPanel == null)
            {
                return;
            }
            var categoryContainerGameObject = GameObject.Find("CategoryContainer");
            if (categoryContainerGameObject == null)
            {
                return;
            }
            var categoryContainer = categoryContainerGameObject.GetComponent<UITabContainer>();
            var mods = categoryContainer?.Find("Mods");
            if (mods == null)
            {
                return;
            }
            var sortByPanel = mods.Find("SortByPanel");
            var sortBy = sortByPanel?.Find("SortBy");
            if (sortBy == null)
            {
                return;
            }
            var modsList = mods.Find("Content");
            if (modsList == null)
            {
                return;
            }
            var moarGroupObj = GameObject.Find("MoarGroup");
            if (moarGroupObj == null)
            {
                return;
            }
            Initialize();
            _ui_initialized = true;
        }


        public void OnDestroy()
        {
            _itemsSorted = false;
            _categoryContainer = null;
            _ui_initialized = false;

            if (_sortModePanel != null)
            {
                Object.Destroy(_sortModePanel.gameObject);
                _sortModePanel = null;
            }
            if (_sortOrderDropDown != null)
            {
                Object.Destroy(_sortOrderDropDown.gameObject);
                _sortOrderDropDown = null;
            }
            if (_sortOrderLabel != null)
            {
                Object.Destroy(_sortOrderLabel.gameObject);
                _sortOrderLabel = null;
            }
            CategoryContentPanelDetour._pluginSortOrder = SortOrder.Ascending;
        }

        private static void Initialize()
        {
            var uiView = Object.FindObjectOfType<UIView>();
            var sortByPanel = uiView.FindUIComponent<UIPanel>("Mods").Find<UIPanel>("SortByPanel");

            _sortModePanel = uiView.AddUIComponent(typeof(UIPanel)) as UIPanel;
            _sortModePanel.gameObject.name = "AssetsSortMode";
            _sortModePanel.transform.parent = sortByPanel.transform;
            _sortModePanel.name = "AssetsSortMode";
            _sortModePanel.AlignTo(sortByPanel, UIAlignAnchor.TopLeft);
            _sortModePanel.size = new Vector2(120.0f, 24.0f);
            _sortModePanel.autoLayout = false;
            _sortModePanel.relativePosition = new Vector3(100, 0);

            _sortOrderDropDown = UIUtils.CreateDropDownForEnum<SortOrder>(_sortModePanel, "SortOrderDropDown");
            _sortOrderDropDown.size = new Vector2(120.0f, 24.0f);
            _sortOrderDropDown.relativePosition = new Vector3(100.0f, 0.0f);
            _sortOrderDropDown.eventSelectedIndexChanged += (component, value) =>
            {
                _sortOrderDropDown.enabled = false;
                CategoryContentPanelDetour._pluginSortOrder = (SortOrder)value;
                RefreshMods();
                _sortOrderDropDown.enabled = true;
            };

            _sortOrderLabel = UIUtils.CreateLabel(_sortModePanel);
            _sortOrderLabel.relativePosition = new Vector3(0.0f, 9.0f);
            _sortOrderLabel.text = "Direction";

            _categoryContainer = PanelUtil.GetCategoryContainer("m_ModsContainer");
            var dict = (Dictionary<string, Comparison<EntryData>>)_categoryContainer.GetType()
                .GetField("m_SortTypeToImplDict", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(_categoryContainer);
            dict["Active"] = Sorting.SortPluginsByActive;
            dict["Last subscribed"] = Sorting.SortPluginsByLastSubscribed;
            dict["Last updated"] = Sorting.SortPluginsByLastUpdate;
            dict["File location"] = Sorting.SortPluginsByLocation;
            var dropDown = (UIDropDown)_categoryContainer.GetType()
                .GetField("m_SortBy", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(_categoryContainer);
            dropDown.AddItem("Active");
            dropDown.AddItem("Last subscribed");
            dropDown.AddItem("Last updated");
            //dropDown.AddItem("File location"); //TODO(earalov): add sorting by location for plugins
        }


        public static void RefreshMods()
        {
            _categoryContainer.GetType().GetMethod("SortEntries", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_categoryContainer, new object[] {});
            _categoryContainer.GetType().GetMethod("RefreshVisibleAssets", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_categoryContainer, new object[] { });
            _categoryContainer.GetType().GetMethod("RefreshEntries", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_categoryContainer, new object[] { });
        }
    }

}
