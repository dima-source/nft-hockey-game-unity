using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Near;
using NearClientUnity;
using NearClientUnity.Utilities;
using TMPro;
using UI.Main_menu;
using UI.Profile.Models;
using UI.Profile.Popups;
using UI.Profile.Rewards;
using UI.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace UI.Profile
{
    public class Profile : UiComponent
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
        
        private TextMeshProUGUI _userWalletName;
        private TextMeshProUGUI _userWalletBalance;
        
        [SerializeField] private TMP_Text LevelNumber;
        [SerializeField] private Slider LevelSlider;
        [SerializeField] private Transform _rewardsParent; 
        private RewardInfoPopup _rewardsInfoPopup;
        [SerializeField] private SignInView signInView;
        [SerializeField] private Transform profilePlaceParentArea;

        private IRewardsRepository _repository = new IndexerRewardsRepository();
        private RewardsUser _rewardsUser;
        private LevelCalculator _levelCalculator;
        private List<BaseReward> _rewardsPrototypes;
        private Button _logoButton;
        private RewardView _rewardViewPrefab;
        private RectTransform _instance;
        //public Button ClosePopup;
        private ILogoLoader _logoLoader = new IndexerLogoLoader();
        private LogoPrefab _logoPrefab;
        private readonly string _pathForm = "/Assets/Resources/Sprites/TeamLogo/Form/";
        private readonly string _pathPattern = "/Assets/Resources/Sprites/TeamLogo/";
        private Button _logout;
        private Button _withdraw;
        
        private void SetInitialValues()
        {
            LevelNumber.text = _levelCalculator.GetLevelString();
            LevelSlider.value = _levelCalculator.GetLevelProgress();
        }
        
        protected override void Initialize()
        {
            LevelNumber = UiUtils.FindChild<TMP_Text>(transform, "LevelNumber");
            LevelSlider = UiUtils.FindChild<Slider>(transform, "Progress");
            _rewardsParent = UiUtils.FindChild<Transform>(transform, "RewardsContent");
            //_rewardsInfoPopup = UiUtils.FindChild<RewardInfoPopup>(transform.parent, "TrophyPopup");
            _logoButton = UiUtils.FindChild<Button>(transform, "LogoContainer");
            _logout = UiUtils.FindChild<Button>(transform, "Logout");
            _withdraw = UiUtils.FindChild<Button>(transform, "Withdraw");
            _logout.onClick.AddListener(()=> ShowPrefabPopup("InfoPopup"));
            _withdraw.onClick.AddListener(()=> ShowPrefabPopup("WithdrawPopup"));
            _levelCalculator = new LevelCalculator(_rewardsUser);
            SetInitialValues();
            _logoButton.onClick.AddListener(() => ShowPrefabPopup("CreateLogoPopup"));
            _userWalletName = UiUtils.FindChild<TextMeshProUGUI>(transform, "Wallet");
            _userWalletBalance = UiUtils.FindChild<TextMeshProUGUI>(transform, "Balance");
            string path = Configurations.PrefabsFolderPath + "Profile/RewardView";
            _rewardViewPrefab = UiUtils.LoadResource<RewardView>(path);
            _logoPrefab = UiUtils.FindChild<LogoPrefab>(transform, "Logo");
        }
        
        protected override async void OnAwake()
        {
            _rewardsUser = await _repository.GetUser();
            _rewardsPrototypes = await _repository.GetRewards();
            StartCoroutine(UpdateProfile());
            InitRewards();
        }

        private IEnumerator UpdateProfile()
        {
            while (gameObject.activeSelf)
            {
                OnUpdate();
                yield return new WaitForSeconds(1);
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
            await Load();
        }
        
        private async Task Load() 
        {
            TeamLogo logoData = await _logoLoader.LoadLogo();
            Debug.Log(logoData.form_name);
            Load(logoData);
        }
        
        private void Load(TeamLogo teamLogo) 
        {
            if (!_logoPrefab.IsDestroyed())
            {
                _logoPrefab.SetData(teamLogo, _pathForm, _pathPattern);
            }
        }
        
        public void GoMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void ShowPopup(Transform popupTransform)
        {
            popupTransform.gameObject.SetActive(true);
        }
        
        public void ClosePopup(Transform popupTransform)
        {
            popupTransform.gameObject.SetActive(false);
        }
        
        public void SignOut()
        {
            NearPersistentManager.Instance.SignOut();

            gameObject.SetActive(false);
            signInView.gameObject.SetActive(true);
        }

        private void InitRewards()
        {
            RemoveAllRewards();
            
            foreach (var reward in _rewardsPrototypes)
                CreateReward(reward);
        }

        private void RemoveAllRewards()
        {
            for (int i = 0; i < _rewardsParent.childCount; i++)
            {
                Destroy(_rewardsParent.GetChild(i));
            }
        }
        
        private RewardView CreateReward(BaseReward reward)
        {
            RewardView rewardView = Instantiate(_rewardViewPrefab, _rewardsParent);
            rewardView.rewardInfoPopup = _rewardsInfoPopup;
            reward.SetForView(rewardView, _rewardsUser);
            return rewardView;
        }
        
        public void ShowPrefabPopup(string name)
        {
            string PATH = Configurations.PrefabsFolderPath + $"Popups/Profile/{name}";

            if (_instance != null)
            {
                Destroy(_instance.gameObject);
            }

            GameObject prefab = UiUtils.LoadResource<GameObject>(PATH);
            _instance = Instantiate(prefab, profilePlaceParentArea).GetComponent<RectTransform>();
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}