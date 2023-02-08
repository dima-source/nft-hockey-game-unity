using System.Collections.Generic;
using System.Threading.Tasks;
using Near;
using Near.Models.Game;
using Runtime;
using UI.Main_menu.UIPopups;
using UI.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Main_menu
{
    public class MainMenu : UiComponent
    {
        
        [SerializeField] private List<Transform> popups;
        [SerializeField] private Transform loadingPopup; 
        [SerializeField] private Transform signInView;
        
        private RectTransform _instance;
        private Transform _mainMenuParent;
        private Button _playGame;
        private Button _friends;
        private Button _mail;
        private Button _manageTeam;
        private Button _tradeCards;
        private Button _profile;
        private Button _exit;

        protected override void Initialize()
        {
            _playGame = UiUtils.FindChild<Button>(transform, "PlayGame");
            _playGame.onClick.AddListener(() => ShowPrefabPopup("SelectBidPopup"));
            _exit = UiUtils.FindChild<Button>(transform, "Exit");
            _exit.onClick.AddListener(() => SignOut());
            _mainMenuParent = UiUtils.FindChild<Transform>(transform, "MainMenu");
            _friends = UiUtils.FindChild<Button>(transform, "Friends");
            _friends.onClick.AddListener(() => ShowPrefabPopup("Friends"));
            _mail = UiUtils.FindChild<Button>(transform, "Mail");
            _mail.onClick.AddListener(() => ShowPrefabPopup("Mail"));
            _manageTeam = UiUtils.FindChild<Button>(transform, "ManageTeam");
            _manageTeam.onClick.AddListener(() => SceneManager.LoadScene("ManageTeam"));
            _tradeCards = UiUtils.FindChild<Button>(transform, "TradeCards");
            _tradeCards.onClick.AddListener(() => SceneManager.LoadScene("Marketplace"));
            _profile = UiUtils.FindChild<Button>(transform, "Profile");
            _profile.onClick.AddListener(() => SceneManager.LoadScene("Profile"));
        }
        public void SignOut()
        {
            NearPersistentManager.Instance.SignOut();
            gameObject.SetActive(false);
            signInView.gameObject.SetActive(true);
        }
        
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
                    
                }
            }
            
            var isMarketAccountRegistered = await CheckMarketplaceAccount(accountID);
            
            if (!isMarketAccountRegistered)
            {
                //firstEntryPopup.gameObject.SetActive(true);
                //ShowPrefabPopup("FirstEntry");
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
        
        

        public void ShowPrefabPopup(string name)
        {
            string PATH = Configurations.PrefabsFolderPath + $"Popups/{name}";
            
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
            }
            
            GameObject prefab = UiUtils.LoadResource<GameObject>(PATH);
            _instance = Instantiate(prefab, _mainMenuParent).GetComponent<RectTransform>();
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