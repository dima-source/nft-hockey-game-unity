using System.IO;
using UI.Profile.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Profile
{
    public class CreateTeamLogoView : MonoBehaviour
    {
        [SerializeField] private Image firstLayerImg;
        [SerializeField] private Image secondLayerImg;
        private string firstLayerColorNumber;
        private string secondLayerColorNumber;
        private readonly string _pathForm = "/Assets/Sprites/Profile/Form/";
        private readonly string _pathPattern = "/Assets/Sprites/Profile";
        private string _formName = "Square"; 
        private string _patternName = "1";
        private ILogoSaver _logoSaver = new ConsoleLogoSaver();
        private ILogoLoader _logoLoader = new MockLogoLoader();
        private Button _saveButton;
        private Button _resetButton;
        private Button _background;
        private Button _closePopupButton;

        public TeamLogo GetTeamLogo()
        {
            TeamLogo logoData = new TeamLogo()
            {
                form_name = _formName,
                pattern_name = _patternName,
                first_layer_color_number = firstLayerColorNumber,
                second_layer_color_number = secondLayerColorNumber
            };
            return logoData;
        }
        
        private void Awake()
        {
            ChangeForm("Star");
            _saveButton = Scripts.Utils.FindChild<Button>(transform, "SaveButton");
            _resetButton = Scripts.Utils.FindChild<Button>(transform, "ResetButton");
            _background = Scripts.Utils.FindChild<Button>(transform, "MainBackground");
            _closePopupButton = Scripts.Utils.FindChild<Button>(transform, "ClosePopup");
            _saveButton.onClick.AddListener(Save);
            _resetButton.onClick.AddListener(Load);
            _background.onClick.AddListener(Close);
            _closePopupButton.onClick.AddListener(Close);
            Load();
        }

        private void OnEnable()
        {
            Load();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public async void Load()
        {
            TeamLogo logoData = await _logoLoader.LoadLogo();
            Load(logoData);
        }

        private void Load(TeamLogo teamLogo)
        {
            ChangeForm(teamLogo.form_name);
            ChangePattern(teamLogo.pattern_name);
            ChangeFirstLayerColor(teamLogo.first_layer_color_number);
            ChangeSecondLayerColor(teamLogo.second_layer_color_number);
        }
        
        public async void Save()
        {
            await _logoSaver.SaveLogo(GetTeamLogo());
            gameObject.SetActive(false);
        }

        public void ChangeForm(string form)
        {
            _formName = form;
            ChangeLayerForm();
        }

        public void ChangePattern(string pattern)
        {
            _patternName = pattern;
            ChangeLayerPattern();
        }
        
        private void ChangeLayerForm()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            string newPath = directoryPath + _pathForm + "/"+ _formName + ".png";
            
            Texture2D texture = Utils.ImageLoader.LoadTexture2D(newPath);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            firstLayerImg.sprite = sprite;
            
            ChangeLayerPattern();
        }
        
        private void ChangeLayerPattern()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            string newPath = directoryPath + _pathPattern + "/" + _formName + "/" + _patternName + ".png";
            
            Texture2D texture = Utils.ImageLoader.LoadTexture2D(newPath);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            secondLayerImg.sprite = sprite;
        }

        public void ChangeFirstLayerColor(string number)
        {
            firstLayerColorNumber = number;
            switch (number)
            {
                case "1":
                    firstLayerImg.color = Color.yellow;
                    break;
                case "2":
                    firstLayerImg.color = Color.blue;
                    break;
                case "3":
                    firstLayerImg.color = Color.green;
                    break;
                case "4":
                    firstLayerImg.color = Color.black;
                    break;
                case "5":
                    firstLayerImg.color = Color.cyan;
                    break;
                case "6":
                    firstLayerImg.color = Color.grey;
                    break;
                case "7":
                    firstLayerImg.color = Color.magenta;
                    break;

            }
        }
        
        public void ChangeSecondLayerColor(string number)
        {
            secondLayerColorNumber = number;
            switch (number) 
            {
                case "1":
                    secondLayerImg.color = Color.yellow;
                    break;
                case "2":
                    secondLayerImg.color = Color.blue;
                    break;
                case "3":
                    secondLayerImg.color = Color.green;
                    break;
                case "4":
                    secondLayerImg.color = Color.black;
                    break;
                case "5":
                    secondLayerImg.color = Color.cyan;
                    break;
                case "6":
                    secondLayerImg.color = Color.grey;
                    break;
                case "7":
                    secondLayerImg.color = Color.magenta;
                    break;
            }
        }
        
    }
}