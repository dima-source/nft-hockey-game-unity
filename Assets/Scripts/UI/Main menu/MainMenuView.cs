using System.Collections.Generic;
using System.Threading.Tasks;
using DevToDev.Analytics;
using Near;
using Near.Models.Game;
using Runtime;
using UI.Main_menu.UIPopups;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Main_menu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private FirstEntryPopupAnimation firstEntryPopup;
        [SerializeField] private SignInView signInView;

        [SerializeField] private List<Transform> popups;
        [SerializeField] private Transform loadingPopup;

        public async void LoadAccountId()
        {
            loadingPopup.gameObject.SetActive(true);
            string accountID = NearPersistentManager.Instance.GetAccountId();

            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                loadingPopup.gameObject.SetActive(false);
                return;
            }

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
            
            if (!isMarketAccountRegistered)
            {
                firstEntryPopup.gameObject.SetActive(true);
            }
            DTDAnalytics.SetUserId(accountID);
            loadingPopup.gameObject.SetActive(false);
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
            #if UNITY_STANDALONE
                Application.Quit();
            #endif
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}