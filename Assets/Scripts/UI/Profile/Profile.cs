using System.Collections.Generic;
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
        [SerializeField] private TMP_Text LevelNumber;
        [SerializeField] private Slider LevelSlider;
        [SerializeField] private Transform _rewardsParent;
        [SerializeField] private RewardInfoPopup _rewardsInfoPopup;

        private IRewardsRepository _repository = new IndexerRewardsRepository();
        private RewardsUser _rewardsUser;
        private LevelCalculator _levelCalculator;
        private List<BaseReward> _rewardsPrototypes;

        private void SetInitialValues()
        {
            LevelNumber.text = _levelCalculator.GetLevelString();
            LevelSlider.value = _levelCalculator.GetLevelProgress();
        }
        
        protected override async void Initialize()
        {
            LevelNumber = Scripts.Utils.FindChild<TMP_Text>(transform, "LevelNumber");
            LevelSlider = Scripts.Utils.FindChild<Slider>(transform, "Progress");
            _rewardsParent = Scripts.Utils.FindChild<Transform>(transform, "RewardsContent");
            _rewardsInfoPopup = Scripts.Utils.FindChild<RewardInfoPopup>(transform.parent, "TrophyPopup");
            _rewardsUser = await _repository.GetUser();
            _rewardsPrototypes = await _repository.GetRewards();
            _levelCalculator = new LevelCalculator(_rewardsUser);
            SetInitialValues();
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
            reward.SetForView(rewardView, _rewardsUser);
            return rewardView;
        }
    }
}