using System;
using System.Collections.Generic;
using Near;
using NearClientUnity;
using NearClientUnity.Utilities;
using TMPro;
using UnityEngine;

namespace UI.Scripts
{
    public class Marketplace : UiComponent
    {
        
        [Serializable]
        public class UserWallet
        {
            public string name;
            public double balance;
        }

        public UserWallet userWallet;
        [Range(1, 5)]
        [SerializeField]
        private int balanceFractionalDisplay = 2;

        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform buyPacksScrollable;
        [SerializeField] private Transform buyPacksNonScrollable;
        
        private Dictionary<string, Transform> _pages;
        private TopBar _topBar;
        
        private TextMeshProUGUI _userWalletName;
        private TextMeshProUGUI _userWalletBalance;
        private TextMeshProUGUI _breadcrumbs;

        public TopBar TopBar => _topBar;
        
        protected override void Initialize()
        {
            _topBar = Utils.FindChild<TopBar>(transform, "TopBar");
            _userWalletName = Utils.FindChild<TextMeshProUGUI>(transform, "Wallet");
            _userWalletBalance = Utils.FindChild<TextMeshProUGUI>(transform, "Balance");
            _breadcrumbs = Utils.FindChild<TextMeshProUGUI>(transform, "Breadcrumbs");
            mainCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
            buyPacksNonScrollable = Utils.FindChild<Transform>(transform, "BuyPacksNonScrollable");
            buyPacksScrollable = Utils.FindChild<Transform>(transform, "BuyPacksScrollable");
            InitializePages();
        }

        protected override void OnAwake()
        {
            SwitchPage(_topBar.NowPage);
            
            _topBar.Bind("BuyPacks", () => SwitchPage("BuyPacks"));
            _topBar.Bind("BuyCards", () => SwitchPage("FilterCards"));
            _topBar.Bind("SellCards", () => SwitchPage("FilterCards"));
            _topBar.Bind("OnSale", () => SwitchPage("FilterCards"));
            _topBar.Bind("Draft", () => ShowOnDevelopmentPopup("Draft"));
            _topBar.Bind("Objects", () => ShowOnDevelopmentPopup("Objects"));
            OnUpdate();
        }

        private void ShowOnDevelopmentPopup(string pageName)
        {
            string message = $"The '{pageName}' is in development. We let you know when it will be available.";
            Popup popup = GetComponent<RectTransform>().GetDefaultOk(pageName, message, () =>
            {
                // Switch page to the default one 
                _topBar.SetSelected("BuyPacks");
                SwitchPage("BuyPacks");
            });
            popup.Show();
        }
        
        public Transform SwitchPage(string pageId)
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
            return _pages[pageId];
        }

        private void InitializePages()
        {
            _pages = new();
            Transform pagesContainer = Utils.FindChild<Transform>(transform, "Main");
            _pages["BuyPacks"] = Utils.FindChild<Transform>(pagesContainer, "BuyPacks");
            _pages["CardDisplay"] = Utils.FindChild<Transform>(pagesContainer, "CardDisplay");
            _pages["FilterCards"] = Utils.FindChild<Transform>(pagesContainer, "FilterCards");
        }

        private void ShowPacksContent()
        {
            Debug.Log(Camera.main.aspect);
            if (Camera.main.aspect >= 1.5) // ratio 16:9 and 3:2
            {
                buyPacksNonScrollable.gameObject.SetActive(true);
                buyPacksScrollable.gameObject.SetActive(false);
            }
            else // ratio ~4:3
            {
                buyPacksNonScrollable.gameObject.SetActive(false);
                buyPacksScrollable.gameObject.SetActive(true);
            }
        }

        protected override async void OnUpdate()
        {
            userWallet.name = NearPersistentManager.Instance.GetAccountId();
            AccountState accountState = await NearPersistentManager.Instance.GetAccountState();
            userWallet.balance = NearUtils.FormatNearAmount(UInt128.Parse(accountState.Amount));
            
            _userWalletName.text = userWallet.name;
            string pattern = "{0:0." + new String('0', balanceFractionalDisplay) + "}";
            _userWalletBalance.text = String.Format(pattern, userWallet.balance) + " <sprite name=NearLogo>";
            _breadcrumbs.text = "Marketplace <sprite name=RightArrow> " + _topBar.SelectedFormatted;
            
            ShowPacksContent();
        }
    }
}
