using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Scripts
{
    public class TopBar : UiComponent
    {
        
        private class Entry
        {
            public UiButton button;
            private readonly TextInformation.BackgroundMaterial _defaultMaterial;

            public Entry(UiButton button)
            {
                this.button = button;
                _defaultMaterial = button.material;
            }

            public void SetDefault()
            {
                button.material = _defaultMaterial;
            }
            
        }

        private Dictionary<string, Entry> _buttons;

        protected override void Initialize()
        {
            _buttons = new();
            _buttons["GoBack"] = new Entry(Utils.FindChild<UiButton>(transform, "GoBack"));
            _buttons["BuyPacks"] = new Entry(Utils.FindChild<UiButton>(transform, "BuyPacks"));
            _buttons["BuyCards"] = new Entry(Utils.FindChild<UiButton>(transform, "BuyCards"));
            _buttons["SellCards"] = new Entry(Utils.FindChild<UiButton>(transform, "SellCards"));
            _buttons["OnSale"] = new Entry(Utils.FindChild<UiButton>(transform, "OnSale"));
            _buttons["Draft"] = new Entry(Utils.FindChild<UiButton>(transform, "Draft"));
            _buttons["Objects"] = new Entry(Utils.FindChild<UiButton>(transform, "Objects"));
        }
        
        protected override void OnAwake()
        {
            foreach (var item in _buttons.Values)
            {
                if (item.button.name == "GoBack") continue;

                item.button.AddOnClickListener(() =>
                {
                    foreach (var itemValue in _buttons.Values)
                    { 
                        itemValue.SetDefault();
                    }
                    item.button.material = TextInformation.BackgroundMaterial.AccentBackground1;
                });
            }
        }

    }
}
