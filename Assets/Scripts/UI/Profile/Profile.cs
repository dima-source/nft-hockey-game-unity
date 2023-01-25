using System;
using System.Collections.Generic;
using Near;
using NearClientUnity;
using NearClientUnity.Utilities;
using Runtime;
using TMPro;
using UI.Profile.Models;
using UI.Profile.Popups;
using UI.Profile.Rewards;
using UI.Scripts;
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
        [SerializeField] private RewardInfoPopup _rewardsInfoPopup;
        [SerializeField] private Transform _createLogoPopup;

        private IRewardsRepository _repository = new IndexerRewardsRepository();
        private RewardsUser _rewardsUser;
        private LevelCalculator _levelCalculator;
        private List<BaseReward> _rewardsPrototypes;
        private Button _logoButton;
        public Button ClosePopup;
        private void SetInitialValues()
        {
            LevelNumber.text = _levelCalculator.GetLevelString();
            LevelSlider.value = _levelCalculator.GetLevelProgress();
        }
        
        protected override void Initialize()
        {
            LevelNumber = Scripts.Utils.FindChild<TMP_Text>(transform, "LevelNumber");
            LevelSlider = Scripts.Utils.FindChild<Slider>(transform, "Progress");
            _rewardsParent = Scripts.Utils.FindChild<Transform>(transform, "RewardsContent");
            _rewardsInfoPopup = Scripts.Utils.FindChild<RewardInfoPopup>(transform.parent, "TrophyPopup");
            _createLogoPopup = Scripts.Utils.FindChild<Transform>(transform.parent, "CreateLogoPopup");
            _logoButton = Scripts.Utils.FindChild<Button>(transform, "LogoContainer");
            _levelCalculator = new LevelCalculator(_rewardsUser);
            SetInitialValues();
            _logoButton.onClick.AddListener(() => ShowPopup(_createLogoPopup));
            _userWalletName = Scripts.Utils.FindChild<TextMeshProUGUI>(transform, "Wallet");
            _userWalletBalance = Scripts.Utils.FindChild<TextMeshProUGUI>(transform, "Balance");
        }
        protected override async void OnAwake()
        {
            _rewardsUser = await _repository.GetUser();
            _rewardsPrototypes = await _repository.GetRewards();
            OnUpdate();
            InitRewards();
        }

        public void GoMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void ShowPopup(Transform popupTransform)
        {
            popupTransform.gameObject.SetActive(true);
        }

        private void InitRewards()
        {
            foreach (var reward in _rewardsPrototypes)
                CreateReward(reward);
        }

        private RewardView CreateReward(BaseReward reward)
        {
            RewardView rewardView = Instantiate(Game.AssetRoot.profileAsset.rewardView, _rewardsParent);
            rewardView.rewardInfoPopup = _rewardsInfoPopup;
            reward.SetForView(rewardView, _rewardsUser);
            return rewardView;
        }
        public void Close()
        {
            gameObject.SetActive(false);
        }

        protected override async void OnUpdate()
        {
            userWallet.name = NearPersistentManager.Instance.GetAccountId();
            AccountState accountState = await NearPersistentManager.Instance.GetAccountState();
            userWallet.balance = NearUtils.FormatNearAmount(UInt128.Parse(accountState.Amount));
            _userWalletName.text = userWallet.name;
            string pattern = "{0:0." + new String('0', balanceFractionalDisplay) + "}";
            _userWalletBalance.text = String.Format(pattern, userWallet.balance) + " <sprite name=NearLogo>";
        }

       
    }
}