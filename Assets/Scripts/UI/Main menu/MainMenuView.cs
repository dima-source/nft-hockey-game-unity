using System.Collections.Generic;
using System.Threading.Tasks;
using Near;
using Near.Models.Game;
using NearClientUnity;
using NearClientUnity.Utilities;
using Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Main_menu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private FirstEntryPopup firstEntryPopup;
        [SerializeField] private SignInView signInView;

        [SerializeField] private Text accountId;
        [SerializeField] private Text balance;

        [SerializeField] private List<Transform> popups;

        public async void LoadAccountId()
        {
            string accountID = NearPersistentManager.Instance.GetAccountId();

            accountId.text = "Welcome, " + accountID + " !";

            AccountState accountState = await NearPersistentManager.Instance.GetAccountState();
            balance.text = "Your balance: " + NearUtils.FormatNearAmount(UInt128.Parse(accountState.Amount)) + " NEAR";

            var isGameAccountRegistered = await CheckGameAccount(accountID);
            if (!isGameAccountRegistered)
            {
                var isSuccess = await Near.GameContract.ContractMethods.Actions.RegisterAccount();
                if (!isSuccess)
                {
                    //TODO: Show popup something went wrong
                }
            }
            
            var isMarketAccountRegistered = await CheckMarketplaceAccount(accountID);
            /*
            if (!isMarketAccountRegistered)
            {
                firstEntryPopup.gameObject.SetActive(true);
            }*/
        }
        
        /// <summary>
        /// Registered or not
        /// </summary>
        private async Task<bool> CheckGameAccount(string accountID)
        {
            var userFilter = new UserFilter()
            {
                id = accountID
            };

            var users = await Near.GameContract.ContractMethods.Views.GetUsers(userFilter);

            if (users.Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Registered or not
        /// </summary>
        private async Task<bool> CheckMarketplaceAccount(string accountID)
        {
            var userFilter = new UserFilter()
            {
                id = accountID
            };

            var users = await Near.MarketplaceContract.ContractMethods.Views.GetUser(userFilter);

            if (users.Count == 0)
            {
                return false;
            }

            return true;
        }

        private async void GetTrained()
        {
        }

        public void TradeCards()
        {
            Game.LoadMarketplace();
        }

        public void LoadManageTeam()
        {
            SceneManager.LoadScene("ManageTeam");
        }

        public void LoadProfile()
        {
            SceneManager.LoadScene("Profile"); 
        }
        public void ShowPopup(Transform popupTransform)
        {
            foreach (Transform popup in popups)
            {
                popup.gameObject.SetActive(false);
            }

            popupTransform.gameObject.SetActive(true);
        }

        public void SignOut()
        {
            NearPersistentManager.Instance.SignOut();

            gameObject.SetActive(false);
            signInView.gameObject.SetActive(true);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}