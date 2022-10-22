using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Main_menu.UIPopups
{
    public class UIPopupSetBid : UIPopup
    {
        [SerializeField] private Transform requestButtons;
        [SerializeField] private Loading loading;
        [SerializeField] private Search searchView;
        [SerializeField] private List<BidButton> buttons;
        [SerializeField] private UIPopupError uiPopupError;

        private string _bid;
        
        public new async void Show()
        {
            /*
            try
            {
                User user = await Near.GameContract.ContractMethods.Views.GetUser();
            
                if (user.is_available)
                {
                    UInt128 deposit = UInt128.Parse(user.deposit);
                    string formatDeposit = NearUtils.FormatNearAmount(deposit).ToString();
                    
                    searchView.SetBidText(formatDeposit);
                    requestButtons.gameObject.SetActive(false);
                    searchView.gameObject.SetActive(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            mainMenuView.ShowPopup(transform);
            */
            
            
            requestButtons.gameObject.SetActive(false);
            searchView.gameObject.SetActive(true);
            mainMenuView.ShowPopup(transform);
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
                    uiPopupError.Show();
                }
                else
                {
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
    }
}