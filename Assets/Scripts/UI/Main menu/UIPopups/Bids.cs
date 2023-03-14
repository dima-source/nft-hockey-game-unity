using System;
using System.Collections.Generic;
using TMPro;
using UI.Scripts;
using UnityEngine.UI;

namespace UI.Main_menu.UIPopups
{
    public class Bids : UiComponent
    {
        private List<Button> _buttons;
        private Button _activeButton;

        public string Bid
        {
            get
            {
                if (_activeButton == null) return "";
                var textBid = UiUtils.FindChild<TextMeshProUGUI>(_activeButton.transform, "Text");
                return textBid.text;
            }
        }

        protected override void Initialize()
        {
            _buttons = new List<Button>();
            for (int i = 1; i < transform.childCount; i++)
            {
                var buttonName = "Bid" + i;
                var button = UiUtils.FindChild<Button>(transform, buttonName);
                var i1 = i;
                button.onClick.AddListener(() => {ChangeBid(i1.ToString());});
                _buttons.Add(button);
            }
        }

        public void ChangeBid(string bid)
        {
            CancelBid();

            foreach (var button in _buttons)
            {
                if (button.name == "Bid" + bid)
                {
                    var outline = button.GetComponent<Outline>();
                    outline.enabled = true;
                    
                    _activeButton = button;
                    return;
                }
            }

            throw new Exception("Bid: " + bid + "not found");
        }

        public void CancelBid()
        {
            if (_activeButton == null) return;
            
            var activeOutline = _activeButton.GetComponent<Outline>();
            activeOutline.enabled = false;
        }
    }
}