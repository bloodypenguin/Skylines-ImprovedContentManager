using System;
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

        private static bool _ui_initialized;

        private static UIPanel _sortModePanel;
        private static UILabel _sortModeLabel;
        private static UIDropDown _sortOrderDropDown;
        private static UILabel _sortOrderLabel;
        private static UIPanel _sortOptions;

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
                return;
            }
            var contentManagerPanelGameObject = GameObject.Find("(Library) ContentManagerPanel");
            if (contentManagerPanelGameObject == null)
            {
                return;
            }
            var contentManagerPanel = contentManagerPanelGameObject.GetComponent<ContentManagerPanel>();
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
            if (categoryContainer == null)
            {
                return;
            }
            var mods = categoryContainer.Find("Mods");
            if (mods == null)
            {
                return; ;
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
            _ui_initialized = false;

            if (_sortModePanel != null)
            {
                Object.Destroy(_sortModePanel.gameObject);
                _sortModePanel = null;
            }
            if (_sortModeLabel != null)
            {
                Object.Destroy(_sortModeLabel.gameObject);
                _sortModeLabel = null;
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
            if (_sortOptions != null)
            {
                Object.Destroy(_sortOptions.gameObject);
                _sortOptions = null;
            }
            CategoryContentPanelDetour._pluginSortMode = SortMode.Alphabetical;
            CategoryContentPanelDetour._pluginSortOrder = SortOrder.Ascending;
        }

        private static void Initialize()
        {
            var moarGroupObj = GameObject.Find("MoarGroup");
            var moarGroup = moarGroupObj.GetComponent<UIPanel>();
            var moarLabel = moarGroup.Find<UILabel>("Moar");
            var moarButton = moarGroup.Find<UIButton>("Button");

            moarGroup.position = new Vector3(moarGroup.position.x, -6.0f, moarGroup.position.z);

            moarLabel.isVisible = false;
            moarButton.isVisible = false;

            var uiView = Object.FindObjectOfType<UIView>();

            _sortOptions = uiView.AddUIComponent(typeof(UIPanel)) as UIPanel;
            _sortOptions.transform.parent = moarGroup.transform;
            _sortOptions.size = new Vector2(200.0f, 24.0f);

            _sortModePanel = uiView.AddUIComponent(typeof(UIPanel)) as UIPanel;
            _sortModePanel.gameObject.name = "AssetsSortMode";
            _sortModePanel.transform.parent = _sortOptions.transform;
            _sortModePanel.name = "AssetsSortMode";
            _sortModePanel.AlignTo(_sortOptions, UIAlignAnchor.TopLeft);
            _sortModePanel.size = new Vector2(100.0f, 24.0f);
            _sortModePanel.autoLayout = false;

            var sortModeDropDown = UIUtils.CreateDropDownForEnum<SortMode>(_sortModePanel, "SortModeDropDown");
            sortModeDropDown.size = new Vector2(100.0f, 24.0f);
            sortModeDropDown.relativePosition = new Vector3(0.0f, 0.0f, 0.0f);
            sortModeDropDown.eventSelectedIndexChanged += (component, value) =>
            {
                sortModeDropDown.enabled = false;
                CategoryContentPanelDetour._pluginSortMode = (SortMode)value;
                RefreshPlugins();
                sortModeDropDown.enabled = true;
            };
            _sortModeLabel = UIUtils.CreateLabel(_sortModePanel);
            _sortModeLabel.text = "Sort by";
            _sortModeLabel.relativePosition = new Vector3(0.0f, -2.0f, 0.0f);


            _sortOrderDropDown = UIUtils.CreateDropDownForEnum<SortOrder>(_sortModePanel, "SortOrderDropDown");
            _sortOrderDropDown.size = new Vector2(100.0f, 24.0f);
            _sortOrderDropDown.relativePosition = new Vector3(100.0f, 0.0f);
            _sortOrderDropDown.eventSelectedIndexChanged += (component, value) =>
            {
                _sortOrderDropDown.enabled = false;
                CategoryContentPanelDetour._pluginSortOrder = (SortOrder)value;
                RefreshPlugins();
                _sortOrderDropDown.enabled = true;
            };

            _sortOrderLabel = UIUtils.CreateLabel(_sortOrderDropDown);
            _sortOrderLabel.text = "Direction";
            _sortOrderLabel.relativePosition = new Vector3(0.0f, -2.0f, 0.0f);
        }


        private static void RefreshPlugins()
        {
            var contentManagerPanelGameObject = GameObject.Find("(Library) ContentManagerPanel");
            if (contentManagerPanelGameObject == null)
            {
                return;
            }
            var contentManagerPanel = contentManagerPanelGameObject.GetComponent<ContentManagerPanel>();
            if (contentManagerPanel == null)
            {
                return;
            }
            contentManagerPanel.GetType().GetMethod("RefreshPlugins", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(contentManagerPanel, new object [] {});
        }
    }

}
