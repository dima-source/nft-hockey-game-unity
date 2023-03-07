using System.Collections.Generic;
using System.Threading.Tasks;
using Near;
using Near.Models.Game;
using UI.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Main_menu
{
    public class MainMenu : UiComponent
    {
        [SerializeField] private List<Transform> popups;
        [SerializeField] private Transform loadingPopup;
        
        private RectTransform _instance;

        protected override void Initialize()
        {
            BindButton("PlayGame", () => ShowPrefabPopup("SelectBidPopup"));
            BindButton("Friends", () => ShowPrefabPopup("Friends"));
            BindButton("Mail", () => ShowPrefabPopup("Mail"));
            BindButton("ManageTeam", () => SceneManager.LoadScene("ManageTeam"));
            BindButton("TradeCards", () => SceneManager.LoadScene("Marketplace"));
            BindButton("Profile", () => SceneManager.LoadScene("Profile"));
            BindButton("Exit", SignOut);
        }

        private void BindButton(string buttonName, UnityAction action)
        {
            var button = UiUtils.FindChild<Button>(transform, buttonName);
            button.onClick.AddListener(action);
        }
        
        public void SignOut()
        {
            NearPersistentManager.Instance.SignOut();
            gameObject.SetActive(false);
            
            var currentPage = UiUtils.LoadResource<GameObject>("Prefabs/SignInMenu");
            Instantiate(currentPage, transform.parent);
        }
        
        public async void LoadAccountId()
        {
            loadingPopup.gameObject.SetActive(true);
            var accountID = NearPersistentManager.Instance.GetAccountId();

            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                loadingPopup.gameObject.SetActive(false);
                return;
            }

            var isGameAccountRegistered = await CheckGameAccount(accountID);
            if (!isGameAccountRegistered)
            {
                var isSuccess = await Near.GameContract.ContractMethods.Actions.RegisterAccount();
            }
            
            var isMarketAccountRegistered = await CheckMarketplaceAccount(accountID);
            
            if (!isMarketAccountRegistered)
            {
                //firstEntryPopup.gameObject.SetActive(true);
                ShowPrefabPopup("FirstEntry");
            }
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
        
        
        public void ShowPopup(Transform popupTransform)
        {
            foreach (Transform popup in popups)
            {
                popup.gameObject.SetActive(false);
            }

            popupTransform.gameObject.SetActive(true);
        }
        
        

        public void ShowPrefabPopup(string popupName)
        {
            var path = Configurations.PrefabsFolderPath + $"Popups/{popupName}";
            
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
            }
            
            var prefab = UiUtils.LoadResource<GameObject>(path);
            _instance = Instantiate(prefab, transform.parent).GetComponent<RectTransform>();
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