using System;
using System.ComponentModel;
using ColossalFramework.UI;
using ImprovedContentManager.Extensions;
using UnityEngine;

namespace ImprovedContentManager.Util
{
    public static class UIUtils
    {
        public static UILabel CreateLabel(UIComponent parent)
        {
            var label = UIView.GetAView().AddUIComponent(typeof(UILabel)) as UILabel;
            label.transform.parent = parent.transform;
            label.AlignTo(parent, UIAlignAnchor.TopLeft);
            label.textColor = Color.white;
            label.textScale = 0.5f;
            return label;
        }


        public static UIDropDown CreateDropDown(UIComponent parent)
        {
            UIDropDown dropDown = parent.AddUIComponent<UIDropDown>();
            dropDown.size = new Vector2(90f, 27f);
            dropDown.listBackground = "StylesDropboxListbox";
            dropDown.itemHeight = 22;
            dropDown.itemHover = "ListItemHover";
            dropDown.itemHighlight = "ListItemHighlight";
            dropDown.normalBgSprite = "StylesDropbox";
            dropDown.disabledBgSprite = "";
            dropDown.hoveredBgSprite = "";
            dropDown.focusedBgSprite = "";
            dropDown.listWidth = 90;
            dropDown.listHeight = 200;
            dropDown.foregroundSpriteMode = UIForegroundSpriteMode.Stretch;
            dropDown.popupColor = new Color32(45, 52, 61, 255);
            dropDown.popupTextColor = new Color32(170, 170, 170, 255);
            dropDown.zOrder = 1;
            dropDown.verticalAlignment = UIVerticalAlignment.Middle;
            dropDown.horizontalAlignment = UIHorizontalAlignment.Left;
            dropDown.selectedIndex = 0;
            dropDown.textFieldPadding = new RectOffset(7, 28, 4, 0);
            dropDown.itemPadding = new RectOffset(4, 4, 4, 4);
            dropDown.listPadding = new RectOffset(4, 4, 4, 4);
            dropDown.listPosition = UIDropDown.PopupListPosition.Automatic;
            dropDown.triggerButton = dropDown;

            dropDown.eventSizeChanged += (c, t) =>
            {
                dropDown.listWidth = (int)t.x;
            };
            return dropDown;
        }

        public static UIDropDown CreateDropDownForEnum<T>(UIComponent parent, string name)
        {
            var dropdown = CreateDropDown(parent);
            dropdown.name = name;
            dropdown.size = new Vector2(120.0f, 16.0f);
            dropdown.textScale = 0.7f;

            var enumValues = Enum.GetValues(typeof(T));
            dropdown.items = new string[enumValues.Length];

            var i = 0;
            foreach (var value in enumValues)
            {
                dropdown.items[i] = ((T)value).GetEnumDescription<T, DescriptionAttribute>().Description;
                i++;
            }
            dropdown.selectedIndex = 0;
            return dropdown;
        }
    }
}