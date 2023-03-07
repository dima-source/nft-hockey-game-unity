using System;
using UI.Profile.Popups;
using UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Profile.Rewards
{
    public class RewardView: UiComponent, IRewardDataReceiver
    {
        private static readonly string SpritesPath = Configurations.SpritesFolderPath + "SpriteSheets/PlayerCardSpriteSheet/";
        public string SpriteName;
        public string RewardTitle;
        public string Description;
        public bool Obtained;
        private Image _rewardImage;
        private Image _notObtainedForeground; 
        [NonSerialized]public RewardInfoPopup rewardInfoPopup;
        private Button _showPopupButton; 
        private RectTransform _profilePlaceParentArea;

        public void SetData(string spriteName, string title, string description, bool obtained)
        {
            SpriteName = spriteName;
            RewardTitle = title;
            Description = description;
            Obtained = obtained;
        }

        protected override void Initialize()
        {
            _rewardImage = UiUtils.FindChild<Image>(transform, "RewardImage");
            _notObtainedForeground = UiUtils.FindChild<Image>(transform, "NotObtainedForeground");
            _showPopupButton = GetComponent<Button>();
            _showPopupButton.onClick.AddListener(ShowPopup);
            _profilePlaceParentArea = UiUtils.FindParent<RectTransform>(transform.parent.transform, "Canvas");
        }

        private void ShowPopup()
        {
            if (rewardInfoPopup == null)
            {
                var path = Configurations.PrefabsFolderPath + "Popups/Profile/TrophyPopup";
                var prefab = UiUtils.LoadResource<GameObject>(path);
                if (rewardInfoPopup != null)
                {
                    Destroy(rewardInfoPopup.gameObject);
                }
                rewardInfoPopup = Instantiate(prefab, _profilePlaceParentArea).GetComponent<RewardInfoPopup>();
                rewardInfoPopup.Show(SpriteName, RewardTitle, Description, Obtained);
            }
            else
            {
                rewardInfoPopup.Show(SpriteName, RewardTitle, Description, Obtained);
            }
        }

        protected override void OnUpdate()
        {
            var color = _notObtainedForeground.color;
            if (Obtained) color.a = 0f;
            else color.a = .75f;
            _notObtainedForeground.color = color;
            
            _rewardImage.sprite = GetRewardSprite();
        }
        
        private Sprite GetRewardSprite()
        {
            string spritePath = SpritesPath + SpriteName;
            return UiUtils.LoadSprite(spritePath);
        }

    }
}