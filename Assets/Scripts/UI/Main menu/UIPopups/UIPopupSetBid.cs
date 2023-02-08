using System;
using System.Collections.Generic;
using Near;
using NearClientUnity.Utilities;
using Runtime;
using UI.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI.Main_menu.UIPopups
{
    public class UIPopupSetBid : UIPopup
    {
        [SerializeField] private Transform requestButtons;
        [SerializeField] private Loading loading;
        [SerializeField] private Search searchView;
        [SerializeField] private List<BidButton> buttons;
        [SerializeField] private UIPopupError uiPopupError;
        
        private Button canselButton;
        private Button confirmButton;
        private RectTransform _instance;
        private string _bid;
        
        
        protected override void Initialize()
        {
            canselButton = UiUtils.FindChild<Button>(transform, "CancelButton");
            confirmButton = UiUtils.FindChild<Button>(transform, "ConfirmButton");
            canselButton.onClick.AddListener(CancelBid);
            confirmButton.onClick.AddListener(SetBid);
        }
        
        public new async void Show(Transform parent)
        {
            
            try
            {
                var user = await Near.GameContract.ContractMethods.Views.GetUser();
            
                if (user.is_available)
                {
                    var deposit = UInt128.Parse(user.deposit);
                    var formatDeposit = NearUtils.FormatNearAmount(deposit).ToString();
                    
                    searchView.SetBidText(formatDeposit);
                    requestButtons.gameObject.SetActive(false);
                    searchView.gameObject.SetActive(true);
                } 
                else if (user.games.Count != 0 && user.games[0].winner_index == null)
                {
                    Game.LoadGame();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            //GetDefaultSelectBid(parent);
            //mainMenuView.ShowPopup(transform);
        }
        
        public async void SetBid()
        {
            if (_bid == "")
            {
                return;
            }
            
            try
            {
                loading.gameObject.SetActive(true);
                uiPopupError.gameObject.SetActive(false); 
                
                await Near.GameContract.ContractMethods.Actions.MakeAvailable(_bid);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("NotEnoughBalance"))
                {
                    uiPopupError.SetTitle("Not enough money. \nFund deposit or select another bid");
                    uiPopupError.Show();
                }
                else
                {
                    uiPopupError.SetTitle("Something went wrong");
                    uiPopupError.Show();
                    Debug.Log(e.Message);
                }
                loading.gameObject.SetActive(false);
                
                return;
            }

            loading.gameObject.SetActive(false);
            searchView.gameObject.SetActive(true);
            searchView.SetBidText(_bid);
            requestButtons.gameObject.SetActive(false);
        }
        
        public void ChangeActiveButton(BidButton newActiveButton)
        {
            foreach (BidButton bidButton in buttons)
            {
                bidButton.image.sprite = bidButton.defaultSprite;
                bidButton.image.color = bidButton.defaultColor;
            }

            newActiveButton.image.sprite = newActiveButton.activeSprite;
            newActiveButton.image.color = newActiveButton.activeColor;
            _bid = newActiveButton.bid;
        }

        public void CancelBid()
        {
            _bid = "";
            foreach (BidButton bidButton in buttons)
            {
                bidButton.image.sprite = bidButton.defaultSprite;
                bidButton.image.color = bidButton.defaultColor;
            } 
        }
        
        
        public void GetDefaultSelectBid(Transform parent)
        {
            string PATH = Configurations.PrefabsFolderPath + "Popups/SelectBidPopup";
            
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
            }

            GameObject prefab = UiUtils.LoadResource<GameObject>(PATH);
            _instance = Instantiate(prefab, parent).GetComponent<RectTransform>();
        }

    }
}