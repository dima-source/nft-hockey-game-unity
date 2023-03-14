using System;
using Near;
using NearClientUnity.Utilities;
using Runtime;
using UI.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Main_menu.UIPopups
{
    public class UIPopupSetBid : UiComponent 
    {
        [SerializeField] private Transform requestButtons;
        [SerializeField] private Loading loading;
        [SerializeField] private Search searchView;
        [SerializeField] private UIPopupError uiPopupError;

        private Bids _bids;
        
        protected override void Initialize()
        {
            _bids = UiUtils.FindChild<Bids>(transform, "Bids");
            
            BidButton("ConfirmButton", SetBid);
            BidButton("CancelButton", _bids.CancelBid);
            BidButton("GoBack", GoBack);
        }

        protected override async void OnAwake()
        {
            try
            {
                var user = await Near.GameContract.ContractMethods.Views.GetUser();
            
                if (user.is_available)
                {
                    var deposit = UInt128.Parse(user.deposit);
                    var formatDeposit = NearUtils.FormatNearAmount(deposit).ToString();
                    
                    _bids.ChangeBid(formatDeposit);
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
        }

        private void BidButton(string buttonName, UnityAction action)
        {
            var button = UiUtils.FindChild<Button>(transform, buttonName);
            button.onClick.AddListener(action);
        }
        
        private async void SetBid()
        {
            var bid = _bids.Bid;
            if (bid == "")
            {
                return;
            }
            
            try
            {
                loading.gameObject.SetActive(true);
                uiPopupError.gameObject.SetActive(false); 
                
                var res = await Near.GameContract.ContractMethods.Actions.MakeAvailable(bid);
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
            searchView.SetBidText(bid);
            requestButtons.gameObject.SetActive(false);
        }

        private void GoBack()
        {
            Destroy(gameObject);
        }
    }
}