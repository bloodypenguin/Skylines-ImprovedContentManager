using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ColossalFramework;
using ColossalFramework.Packaging;
using ColossalFramework.UI;
using ImprovedContentManager.Detours;
using ImprovedContentManager.Enums;
using ImprovedContentManager.Extensions;
using ImprovedContentManager.Redirection;
using ImprovedContentManager.Util;
using UnityEngine;
using Object = UnityEngine.Object;
using UIUtils = ImprovedContentManager.Util.UIUtils;

namespace ImprovedContentManager.UI
{
    public class ImprovedAssetsPanel : MonoBehaviour
    {
        private static UIPanel _buttonsPanel;
        private static UIPanel _sortModePanel;
        private static UIDropDown _sortOrderDropDown;
        private static UILabel _sortOrderLabel;
        private static UIPanel _sortOptions;

        private static List<UIButton> _assetTypeButtons = new List<UIButton>();
        private static Dictionary<AssetType, UILabel> _assetTypeLabels = null;

        private static bool _ui_initialized;

        public static void Bootstrap()
        {
            var syncObject = GameObject.Find("ImprovedAssetsPanelSyncObject");
            if (syncObject != null)
            {
                return;
            }
            syncObject = new GameObject("ImprovedAssetsPanelSyncObject");
            syncObject.AddComponent<ImprovedAssetsPanel>();
        }

        public void Update()
        {
            if (Singleton<LoadingManager>.instance.m_loadedEnvironment != null)
            {
                Destroy(gameObject);
            }
            if (_ui_initialized)
            {
                if (CategoryContentPanelDetour.refreshLabelsFlag)
                {
                    SetAssetCountLabels(CategoryContentPanelDetour._assetTypeIndex);
                    CategoryContentPanelDetour.refreshLabelsFlag = false;
                }
            } else {
                var contentManagerPanelGameObject = GameObject.Find("(Library) ContentManagerPanel");
                var contentManagerPanel = contentManagerPanelGameObject?.GetComponent<ContentManagerPanel>();
                if (contentManagerPanel == null)
                {
                    return;
                }
                var categoryContainerGameObject = GameObject.Find("CategoryContainer");
                var categoryContainer = categoryContainerGameObject?.GetComponent<UITabContainer>();
                var mods = categoryContainer?.Find("Assets");
                if (mods == null)
                {
                    return; ;
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

        }

        public void OnDestroy()
        {
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
            if (_buttonsPanel != null)
            {
                Object.Destroy(_buttonsPanel.gameObject);
                _buttonsPanel = null;
            }
            if (_sortOptions != null)
            {
                Object.Destroy(_sortOptions.gameObject);
                _sortOptions = null;
            }

            CategoryContentPanelDetour._assetFilterMode = AssetType.All;
            CategoryContentPanelDetour._assetSortOrder = SortOrder.Ascending;

            _assetTypeButtons = new List<UIButton>();
            lock (LabelsLock)
            {
                _assetTypeLabels = new Dictionary<AssetType, UILabel>();
            }
        }

        private static void Initialize()
        {
            var moarGroup = GameObject.Find("Assets").GetComponent<UIPanel>().Find<UIPanel>("MoarGroup");
            var uiView = Object.FindObjectOfType<UIView>();

            var moarLabel = moarGroup.Find<UILabel>("Moar");
            var moarButton = moarGroup.Find<UIButton>("Button");

            moarGroup.position = new Vector3(moarGroup.position.x, -6.0f, moarGroup.position.z);

            moarLabel.isVisible = false;
            moarButton.isVisible = false;

            const float buttonHeight = 18.0f;

            _buttonsPanel = uiView.AddUIComponent(typeof(UIPanel)) as UIPanel;
            _buttonsPanel.transform.parent = moarGroup.transform;
            _buttonsPanel.size = new Vector2(400.0f, buttonHeight * 2);

            var assetTypes = (AssetType[])Enum.GetValues(typeof(AssetType));

            lock (LabelsLock)
            {
                _assetTypeLabels = new Dictionary<AssetType, UILabel>();
            }

            _assetTypeButtons = new List<UIButton>();
            var count = 0;
            var columnCount = ((assetTypes.Length - 1) / 2) + ((assetTypes.Length - 1) % 2); //-1 because no LUT
            foreach (var assetType in assetTypes)
            {
                if (assetType == AssetType.ColorLut)
                {
                    continue;
                }

                count++;
                var buttonsRow = count <= columnCount ? 0 : 1;
                var button = _buttonsPanel.AddUIComponent(typeof(UIButton)) as UIButton;
                button.size = new Vector2(buttonHeight, buttonHeight);
                button.tooltip = assetType.ToString();

                button.focusedFgSprite = button.normalFgSprite;
                button.pressedFgSprite = button.normalFgSprite;
                switch (assetType)
                {
                    case AssetType.All:
                        button.text = "ALL";
                        break;
                    case AssetType.Unknown:
                        button.text = "N/A";
                        break;
                    default:
                        button.normalFgSprite = assetType.GetEnumDescription<AssetType, AssetTypeAttribute>().SpriteName;
                        button.hoveredFgSprite = $"{assetType.GetEnumDescription<AssetType, AssetTypeAttribute>().SpriteName} Hovered";
                        break;
                }
                button.textScale = 0.5f;
                button.foregroundSpriteMode = UIForegroundSpriteMode.Scale;
                button.scaleFactor = 1.0f;
                button.isVisible = true;

                button.transform.parent = _buttonsPanel.transform;
                button.AlignTo(_buttonsPanel, UIAlignAnchor.TopLeft);


                button.relativePosition = new Vector3((buttonHeight + 2.0f) * ((count - 1) % columnCount), (buttonHeight * buttonsRow) - 2.0f);


                if (assetType == AssetType.Residential)
                {
                    button.color = Color.green;
                }
                else if (assetType == AssetType.Commercial)
                {
                    button.color = new Color32(100, 100, 255, 255);
                }
                else if (assetType == AssetType.Industrial)
                {
                    button.color = Color.yellow;
                }
                else if (assetType == AssetType.Office)
                {
                    button.color = new Color32(0, 255, 255, 255);
                }

                button.focusedColor = button.color;
                button.hoveredColor = button.color;
                button.disabledColor = button.color;
                button.pressedColor = button.color;

                button.opacity = 0.25f;
                if (assetType == AssetType.All)
                {
                    button.opacity = 1.0f;
                }

                var type = assetType;
                button.eventClick += (component, param) =>
                {
                    CategoryContentPanelDetour._assetFilterMode = type;

                    foreach (var item in _assetTypeButtons)
                    {
                        item.opacity = 0.25f;
                    }

                    button.opacity = 1.0f;
                    CategoryContentPanelDetour.dontRefreshLabels = true;
                    RefreshAssets();
                    CategoryContentPanelDetour.dontRefreshLabels = false;
                };

                _assetTypeButtons.Add(button);

                var label = uiView.AddUIComponent(typeof(UILabel)) as UILabel;
                label.text = "N/A";
                label.AlignTo(button, UIAlignAnchor.TopRight);
                label.relativePosition = new Vector3(16.0f, 0.0f, 0.0f);
                label.zOrder = 7;
                label.textScale = 0.5f;
                label.textColor = Color.white;
                lock (LabelsLock)
                {
                    _assetTypeLabels.Add(assetType, label);
                }
            }

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

            //TODO(earalov): add sort mode drop down items and add comparators to dictionary


            _sortOrderDropDown = UIUtils.CreateDropDownForEnum<SortOrder>(_sortModePanel, "SortOrderDropDown");
            _sortOrderDropDown.size = new Vector2(100.0f, 24.0f);
            _sortOrderDropDown.relativePosition = new Vector3(100.0f, 0.0f);
            _sortOrderDropDown.eventSelectedIndexChanged += (component, value) =>
            {
                _sortOrderDropDown.enabled = false;
                CategoryContentPanelDetour._assetSortOrder = (SortOrder)value;
                CategoryContentPanelDetour.dontRefreshLabels = true;
                RefreshAssets();
                CategoryContentPanelDetour.dontRefreshLabels = false;
                _sortOrderDropDown.enabled = true;
            };

            _sortOrderLabel = UIUtils.CreateLabel(_sortOrderDropDown);
            _sortOrderLabel.text = "Direction";
            _sortOrderLabel.relativePosition = new Vector3(0.0f, -2.0f, 0.0f);
        }

        private static void RefreshAssets()
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
            object m_AssetsContainer =
                contentManagerPanel.GetType()
                    .GetField("m_AssetsContainer", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(contentManagerPanel);
            contentManagerPanel.GetType().GetMethod("RefreshCategory", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(contentManagerPanel, new object[] { m_AssetsContainer });
        }

        private static readonly object LabelsLock = new object();
        public static void SetAssetCountLabels(Dictionary<AssetType, int> assetTypeIndex)
        {
            lock (LabelsLock)
            {
                if (_assetTypeLabels == null)
                {
                    return;
                }
                foreach (var assetType in _assetTypeLabels.Keys)
                {
                    var label = _assetTypeLabels[assetType];
                    label.text = assetTypeIndex[assetType].ToString();
                }
            }

        }
    }
}