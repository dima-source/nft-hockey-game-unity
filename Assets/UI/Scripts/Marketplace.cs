using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Scripts
{
    public class Marketplace : UiComponent
    {
        
        [Serializable]
        public class UserWallet
        {
            public string name;
            public float balance;
        }

        public UserWallet userWallet;
        [Range(1, 5)]
        [SerializeField]
        private int balanceFractionalDisplay = 2;
        
        private Popup _popup;
        private Dictionary<string, Transform> _pages;
        private TopBar _topBar;
        
        private TextInformation _userWalletName;
        private TextInformation _userWalletBalance;
        private TextInformation _breadcrumbs;
        
        protected override void Initialize()
        {
            _popup = Utils.FindChild<Popup>(transform, "Popup");
            _topBar = Utils.FindChild<TopBar>(transform, "TopBar");
            _userWalletName = Utils.FindChild<TextInformation>(transform, "Wallet");
            _userWalletBalance = Utils.FindChild<TextInformation>(transform, "Balance");
            _breadcrumbs = Utils.FindChild<TextInformation>(transform, "Breadcrumbs");
            InitializePages();
        }

        protected override void OnAwake()
        {
            _topBar.Bind("BuyPacks", () => SwitchPage("BuyPacks"));
            _topBar.Bind("BuyCards", () => SwitchPage("Statistics"));
            
            
            _topBar.Bind("Draft", () => ShowOnDevelopmentPopup("Draft"));
            _topBar.Bind("Objects", () => ShowOnDevelopmentPopup("Objects"));
        }

        private void ShowOnDevelopmentPopup(string pageName)
        {
            _popup.SetTitle(pageName);
            _popup.SetMessage($"The '{pageName}' is in development. We let you know when it will be available.");
            _popup.buttons = new[]
            {
                new Popup.ButtonView(Popup.ButtonType.Positive, "Okay")
            };
            
            // Switch page to the default one 
            _popup.onClose = () =>
            {
                _topBar.SetSelected("BuyPacks");
                SwitchPage("BuyPacks");
            };
            _popup.OnButtonClick(0, _popup.Close);
            _popup.Show();
        }
        
        private void SwitchPage(string pageId)
        {
            if (!_pages.ContainsKey(pageId))
            {
                throw new ApplicationException($"Unknown key '{pageId}'");
            }
            
            foreach (var page in _pages.Values)
            {
                page.gameObject.SetActive(false);
            }
            _pages[pageId].gameObject.SetActive(true);
        }

        private void InitializePages()
        {
            _pages = new();
            Transform pagesContainer = Utils.FindChild<Transform>(transform, "Main");
            _pages["BuyPacks"] = Utils.FindChild<Transform>(pagesContainer, "BuyPacks");
            _pages["Statistics"] = Utils.FindChild<Transform>(pagesContainer, "Statistics");
        }

        protected override void OnUpdate()
        {
            _userWalletName.text = userWallet.name;
            string pattern = "{0:0." + new String('0', balanceFractionalDisplay) + "}";
            _userWalletBalance.text = String.Format(pattern, userWallet.balance) + " <sprite=0>";
            _breadcrumbs.text = "Marketplace <sprite=1> " + _topBar.SelectedFormatted;
        }
    }
}
