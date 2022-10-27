using System;
using TMPro;
using UI.Profile.Models;
using UI.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Profile
{
    public class Profile : UiComponent
    {
        [SerializeField] private TMP_Text LevelNumber;
        [SerializeField] private Slider LevelSlider;

        private IRewardsRepository _repository = new IndexerRewardsRepository();
        private RewardsUser _rewardsUser;
        
        protected override async void Initialize()
        {
            LevelNumber = Scripts.Utils.FindChild<TMP_Text>(transform, "LevelNumber");
            LevelSlider = Scripts.Utils.FindChild<Slider>(transform, "Progress");
            _rewardsUser = await _repository.GetUser();
        }

        public void GoMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void ShowPopup(Transform popupTransform)
        {
            popupTransform.gameObject.SetActive(true);
        }
    }
}