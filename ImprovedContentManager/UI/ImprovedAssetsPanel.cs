using System;
using System.Collections.Generic;
using System.Reflection;
using ColossalFramework;
using ColossalFramework.UI;
using ImprovedContentManager.Detours;
using ImprovedContentManager.Enums;
using ImprovedContentManager.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;
using UIUtils = ImprovedContentManager.Util.UIUtils;

namespace ImprovedContentManager.UI
{
    public class ImprovedAssetsPanel : MonoBehaviour
    {
        private static readonly object LabelsLock = new object();
        private static CategoryContentPanel _categoryContainer;


        private static UIPanel _buttonsPanel;
        private static UIPanel _sortModePanel;
        private static UIPanel _actionsPanel;

        private static List<UIButton> _assetTypeButtons = new List<UIButton>();
        private static Dictionary<AssetType, UILabel> _assetTypeLabels = null;
        public static Dictionary<AssetType, int> _assetTypeIndex = new Dictionary<AssetType, int>();

        private static bool _ui_initialized;
        private static bool _itemsSorted;

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
                if (!_itemsSorted && _categoryContainer.gameObject.GetComponent<UIComponent>().isVisible)
                {
                    _categoryContainer.GetType().GetField("m_SortImpl", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_categoryContainer, new Comparison<EntryData>(
                        EntryData.SortByName));
                    RefreshAssets();
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

        public void OnDestroy()
        {
            _itemsSorted = false;
            _ui_initialized = false;
            _categoryContainer = null;

            if (_sortModePanel != null)
            {
                Object.Destroy(_sortModePanel.gameObject);
                _sortModePanel = null;
            }
            if (_buttonsPanel != null)
            {
                Object.Destroy(_buttonsPanel.gameObject);
                _buttonsPanel = null;
            }
            if (_actionsPanel != null)
            {
                Object.Destroy(_actionsPanel.gameObject);
                _actionsPanel = null;
            }

            Filtering._assetFilterMode = AssetType.All;

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
                var button = UIUtils.CreateButton(_buttonsPanel);
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
                    Filtering._assetFilterMode = type;

                    foreach (var item in _assetTypeButtons)
                    {
                        item.opacity = 0.25f;
                    }

                    button.opacity = 1.0f;
                    RefreshAssets();
                };

                _assetTypeButtons.Add(button);

                var label = uiView.AddUIComponent(typeof(UILabel)) as UILabel;
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

            _actionsPanel = uiView.AddUIComponent(typeof(UIPanel)) as UIPanel;
            _actionsPanel.transform.parent = moarGroup.transform;
            _actionsPanel.size = new Vector2(150.0f, 24.0f);

            var refreshCounters = UIUtils.CreateButton(_actionsPanel);
            refreshCounters.width = 150f;
            refreshCounters.text = "Count assets";

            var sortByPanel = uiView.FindUIComponent<UIPanel>("Assets").Find<UIPanel>("SortByPanel");

            _sortModePanel = uiView.AddUIComponent(typeof(UIPanel)) as UIPanel;
            _sortModePanel.gameObject.name = "AssetsSortMode";
            _sortModePanel.transform.parent = sortByPanel.transform;
            _sortModePanel.name = "AssetsSortMode";
            _sortModePanel.AlignTo(sortByPanel, UIAlignAnchor.TopLeft);
            _sortModePanel.size = new Vector2(120.0f, 24.0f);
            _sortModePanel.autoLayout = false;

            _categoryContainer = PanelUtil.GetCategoryContainer("m_AssetsContainer");
//            var dict = (Dictionary<string, Comparison<EntryData>>)_categoryContainer.GetType()
//                .GetField("m_SortTypeToImplDict", BindingFlags.NonPublic | BindingFlags.Static)
//                .GetValue(_categoryContainer);
//            dict["Active"] = Sorting.SortAssetsByActive;
//            dict["Last subscribed"] = Sorting.SortAssetsByLastSubscribed;
//            dict["Last updated"] = Sorting.SortAssetsByLastUpdate;
//            dict["File location"] = Sorting.SortAssetsByLocation;
//            var dropDown = (UIDropDown)_categoryContainer.GetType()
//                .GetField("m_SortBy", BindingFlags.NonPublic | BindingFlags.Instance)
//                .GetValue(_categoryContainer);
//            dropDown.AddItem("Active");
//            dropDown.AddItem("Last subscribed");
//            dropDown.AddItem("Last updated");
//            dropDown.AddItem("File location");
            refreshCounters.eventClick += (component, param) =>
            {
                SetAssetCountLabels();
            };
        }

        public static void RefreshAssets()
        {
            _categoryContainer.GetType().GetMethod("SortEntries", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_categoryContainer, new object[] { });
            _categoryContainer.GetType().GetMethod("RefreshVisibleAssets", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_categoryContainer, new object[] { });
            _categoryContainer.GetType().GetMethod("RefreshEntries", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_categoryContainer, new object[] { });
        }

        private static void SetAssetCountLabels()
        {

            lock (LabelsLock)
            {
                var assetTypeIndex = CategoryContentPanelDetour.CountAssets(_categoryContainer);
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