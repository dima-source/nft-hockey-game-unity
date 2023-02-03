using System;
using System.Collections.Generic;
using Near;
using Near.Models.Tokens;
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

        [SerializeField] private Transform buyPacksScrollable;
        [SerializeField] private Transform buyPacksNonScrollable;

        private Dictionary<string, Transform> _pages;
        private TopBar _topBar;
        private PackAnimation _packAnimation;
        
        private TextMeshProUGUI _userWalletName;
        private TextMeshProUGUI _userWalletBalance;
        private TextMeshProUGUI _breadcrumbs;

        public Transform popupLoading;
        public Transform popupAnimation;
        public TopBar TopBar => _topBar;
        
        protected override void Initialize()
        {
            _topBar = UiUtils.FindChild<TopBar>(transform, "TopBar");
            _userWalletName = UiUtils.FindChild<TextMeshProUGUI>(transform, "Wallet");
            _userWalletBalance = UiUtils.FindChild<TextMeshProUGUI>(transform, "Balance");
            _breadcrumbs = UiUtils.FindChild<TextMeshProUGUI>(transform, "Breadcrumbs");
            _packAnimation = UiUtils.FindChild<PackAnimation>(transform, "SoldPopupAnimation");
            buyPacksNonScrollable = UiUtils.FindChild<Transform>(transform, "BuyPacksNonScrollable");
            buyPacksScrollable = UiUtils.FindChild<Transform>(transform, "BuyPacksScrollable");
            
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

        private void FindPacks()
        {
            List<Transform> containers = new List<Transform>()
            {
                UiUtils.FindChild<Transform>(buyPacksScrollable, "Content"),
                UiUtils.FindChild<Transform>(buyPacksNonScrollable, "Content")
            };
            List<PackTypes> packTypes = new List<PackTypes>()
            {
                PackTypes.Bronze,
                PackTypes.Silver,
                PackTypes.Gold,
                PackTypes.Platinum,
                PackTypes.Brilliant
            };
            List<string> descriptions = new List<string>()
            {
                "20%  chance to get an uncommon card",
                "5%  chance to get a super rare card",
                "5%  chance to get an unique card",
                "10%  chance to get an exclusive card",
                "20%  chance to get an exclusive card"
            };
            List<int> prices = new List<int>() {7, 10, 13, 15, 20};
            for (int containerNumber = 0; containerNumber < 2; containerNumber++)
            {
                var container = containers[containerNumber];
                var content = UiUtils.FindChild<Transform>(container, "Content").GetComponentsInChildren<PackView>();
                var previousVisibility = container.gameObject.activeSelf;
                container.gameObject.SetActive(true);
                for (int i = 0; i < 5; i++)
                {
                    PackView pack = content[i];
                    pack.SetData(
                        packTypes[i],
                        prices[i],
                        descriptions[i],
                        ShowBuyingPack,
                        ShowBoughtPack
                        );
                }
                container.gameObject.SetActive(previousVisibility);
            }
        }

        // -> PackAnimation
        public void BuyButton(string packType)
        {
            if (packType == "Bronze")
            {
                ShowBuyingPack(PackTypes.Bronze);
            }
            else if (packType == "Silver")
            {
                ShowBuyingPack(PackTypes.Silver);
            }
            else if (packType == "Gold")
            {
                ShowBuyingPack(PackTypes.Gold);
            }
            else if (packType == "Platinum")
            {
                ShowBuyingPack(PackTypes.Platinum);
            }
            else if (packType == "Brilliant")
            {
                ShowBuyingPack(PackTypes.Brilliant);
            }
        }
        
        public async void ShowBuyingPack(PackTypes packType)
        {
            // TODO @udovenkodima7@gmail.com enable loading popup
            popupLoading.gameObject.SetActive(true);
            // todosomething
            List<Token> tokens = await Near.MarketplaceContract.ContractMethods.Actions.BuyPack(((int)packType).ToString());
            
            ShowBoughtPack(tokens, packType);
            Debug.Log("Buying pack");
        }

        public void ShowBoughtPack(List<Token> tokens, PackTypes packType)
        {
            popupLoading.gameObject.SetActive(false);
            popupAnimation.gameObject.SetActive(true);
            _packAnimation.SetData(tokens);
            _packAnimation.Play();
            // TODO @udovenkodima7@gmail.com disable loading popup and show popup with pack opening animation
            Debug.Log($"Bought {packType.ToString()} pack");
        }

        private void InitializePages()
        {
            _pages = new();
            Transform pagesContainer = UiUtils.FindChild<Transform>(transform, "Main");
            _pages["BuyPacks"] = UiUtils.FindChild<Transform>(pagesContainer, "BuyPacks");
            _pages["CardDisplay"] = UiUtils.FindChild<Transform>(pagesContainer, "CardDisplay");
            _pages["FilterCards"] = UiUtils.FindChild<Transform>(pagesContainer, "FilterCards");
            FindPacks();
        }

        private void ShowPacksContent()
        {
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
