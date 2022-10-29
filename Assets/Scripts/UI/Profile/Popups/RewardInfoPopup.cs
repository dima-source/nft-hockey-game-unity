using TMPro;
using UI.Main_menu.UIPopups;
using UI.Scripts;
using UnityEngine.UI;

namespace UI.Profile.Popups
{
    public class RewardInfoPopup: UiComponent
    {
        private static readonly string SPRITES_PATH = Configurations.SpritesFolderPath + "SpriteSheet/";
        private string _spriteName;
        private string _title;
        private string _description;
        private bool _obtained;
        public Image Image;
        public TMP_Text Title;
        public TMP_Text Description;
        public Button ClosePopup;
        public Button MainBackground;

        protected override void Initialize()
        {
            Image = Scripts.Utils.FindChild<Image>(transform, "IMGArea");
            Title = Scripts.Utils.FindChild<TMP_Text>(transform, "name");
            Description = Scripts.Utils.FindChild<TMP_Text>(transform, "desc");
            ClosePopup = Scripts.Utils.FindChild<Button>(transform, "ClosePopup");
            MainBackground = Scripts.Utils.FindChild<Button>(transform, "MainBackground");
            
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
                Image.sprite = Scripts.Utils.LoadSprite(SPRITES_PATH + _spriteName);
            }
            else
            {
                Image.sprite = Scripts.Utils.LoadSprite(SPRITES_PATH + "Square");
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