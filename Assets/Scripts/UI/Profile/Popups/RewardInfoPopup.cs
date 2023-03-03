using System;
using TMPro;
using UI.Main_menu.UIPopups;
using UI.Scripts;
using UnityEngine.UI;

namespace UI.Profile.Popups
{
    public class RewardInfoPopup: UiComponent
    {
        private static readonly string SPRITES_PATH = Configurations.SpritesFolderPath + "SpriteSheets/";
        private string _spriteName;
        private string _title;
        private string _description;
        private bool _obtained;
        [NonSerialized] public Image Image;
        [NonSerialized] public TMP_Text Title;
        [NonSerialized] public TMP_Text Description;
        [NonSerialized] public Button ClosePopup;
        [NonSerialized] public Button MainBackground;

        protected override void Initialize()
        {
            Image = Scripts.UiUtils.FindChild<Image>(transform, "IMGArea");
            Title = Scripts.UiUtils.FindChild<TMP_Text>(transform, "name");
            Description = Scripts.UiUtils.FindChild<TMP_Text>(transform, "desc");
            ClosePopup = Scripts.UiUtils.FindChild<Button>(transform, "ClosePopup");
            MainBackground = Scripts.UiUtils.FindChild<Button>(transform, "MainBackground");
            
            ClosePopup.onClick.AddListener(Close);
            MainBackground.onClick.AddListener(Close);
        }

        public void Show(string spriteName, string title, string description, bool obtained)
        {
            _spriteName = spriteName;
            _title = title;
            _description = description;
            _obtained = obtained;
            gameObject.SetActive(true);
        }
        
        protected override void OnUpdate()
        {
            if (_obtained)
            {
                Image.sprite = Scripts.UiUtils.LoadSprite(SPRITES_PATH + _spriteName);
            }
            else
            {
                Image.sprite = Scripts.UiUtils.LoadSprite(SPRITES_PATH + "Square");
            }
            Title.text = _title;
            Description.text = _description;
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
        }
        
        
    }
}