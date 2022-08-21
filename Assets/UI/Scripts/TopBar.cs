using System;
using System.Collections.Generic;
using System.Text;

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

        // Default selected
        private string _selected = "BuyPacks";

        public string SelectedFormatted
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < _selected.Length; i++)
                {
                    builder.Append(_selected[i]);
                    if (i < _selected.Length - 1 && Char.IsLower(_selected[i]) && 
                        Char.IsUpper(_selected[i + 1]))
                    {
                        builder.Append(' ');
                    }
                }

                return builder.ToString();
            }
        }

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

        public void Bind(string buttonId, Action action)
        {
            if (!_buttons.ContainsKey(buttonId))
            {
                throw new ApplicationException($"Unknown key '{buttonId}'");
            }
            _buttons[buttonId].button.onClick = () =>
            {
                SetSelected(buttonId);
                action();
            };
        }
        
        public void BindAll(Action[] actions)
        {
            if (actions.Length != _buttons.Count)
            {
                throw new ApplicationException("Invalid 'pages' length");
            }

            int index = 0;
            foreach (var item in _buttons.Values)
            {
                Bind(item.button.name, actions[index]);
                index++;
            }
        }

        public void SetSelected(string buttonId)
        {
            if (!_buttons.ContainsKey(buttonId))
            {
                throw new ApplicationException($"Unknown key '{buttonId}'");
            }
            
            foreach (var itemValue in _buttons.Values)
            { 
                itemValue.SetDefault();
            }
            
            _selected = buttonId;
            _buttons[buttonId].button.material = TextInformation.BackgroundMaterial.PrimaryBackground;
        }

    }
}
