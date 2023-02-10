using System.IO;
using TMPro;
using UI.Profile.Models;
using UI.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Profile
{
    public class LogoPrefab : UiComponent
    {
         private Image _firstLayerImg;
         private Image _secondLayerImg;
         private Image _textArea;
         private Image _textGround;
         private TextMeshProUGUI _textTeamName;
         private string _teamName;
         private string _username;
         private string _pathForm;
         private string _pathPattern;
         private string _formName = "Square"; 
         private string _patternName = "1";
         private string firstLayerColorNumber;
         private string secondLayerColorNumber;
         private string inputLayerColorNumber;
         private string inputGroundColorNumber;
         
        protected override void Initialize()
        {
            _firstLayerImg = Scripts.UiUtils.FindChild<Image>(transform, "FirstLayer");
            _secondLayerImg = Scripts.UiUtils.FindChild<Image>(transform, "SecondLayer");
            _textArea = Scripts.UiUtils.FindChild<Image>(transform, "TextArea");
            _textGround = Scripts.UiUtils.FindChild<Image>(transform, "TextGround");
            _textTeamName = Scripts.UiUtils.FindChild<TextMeshProUGUI>(transform, "TeamName"); 
        }
        
        public TeamLogo GetTeamLogo()
        {
            TeamLogo logoData = new TeamLogo()
            {
                username = _username ?? "",
                team_name = _teamName ?? "",
                form_name = _formName,
                pattern_name = _patternName,
                first_layer_color_number = firstLayerColorNumber,
                second_layer_color_number = secondLayerColorNumber
            };
            return logoData;
        }
        
        public void SetData(TeamLogo teamLogo, string pathForm, string pathPattern)
        {
            _pathForm = pathForm;
            _pathPattern = pathPattern;
            _firstLayerImg.gameObject.SetActive(false);
            _secondLayerImg.gameObject.SetActive(false);
            ChangeLayerForm(teamLogo.form_name);
            ChangeLayerPattern(teamLogo.pattern_name);
            ChangeFirstLayerColor(teamLogo.first_layer_color_number); 
            ChangeSecondLayerColor(teamLogo.second_layer_color_number); 
            ChangeTeamName(teamLogo.team_name);
            _firstLayerImg.gameObject.SetActive(true);
            _secondLayerImg.gameObject.SetActive(true);
        }
        
        public void ChangeLayerForm(string formName)
        {
            _formName = formName;
            string directoryPath = Directory.GetCurrentDirectory();
            string newPath = directoryPath + _pathForm + "/"+ _formName + ".png";
            
            Texture2D texture = Utils.ImageLoader.LoadTexture2D(newPath);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            _firstLayerImg.sprite = sprite;
            
            ChangeLayerPattern(_patternName);
        }
        
        public void ChangeLayerPattern(string patternName)
        {
            _patternName = patternName;
            string directoryPath = Directory.GetCurrentDirectory();
            string newPath = directoryPath + _pathPattern + "/" + _formName + "/" + _patternName + ".png";
            
            Texture2D texture = Utils.ImageLoader.LoadTexture2D(newPath);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            _secondLayerImg.sprite = sprite;
        }

        public void ChangeTeamName(string teamName)
        {
            _textTeamName.SetText(teamName);
            _teamName = teamName;
        }

        public void ChangeUsername(string userName)
        {
            // TODO: change name on the scene
            _username = userName;
        }
        
        public void ChangeFirstLayerColor(string number)
        {
            firstLayerColorNumber = number;
            inputLayerColorNumber = number;
            switch (number)
            {
                case "1":
                    Color color1 = new Color()
                    {
                        r = (float)223 / 255,
                        g = (float)178 / 255,
                        b = (float)125 / 255,
                        a = 1
                    };
                    _firstLayerImg.color = color1;
                    _textArea.color = color1;
                    break;
                case "2":
                    Color color2 = new Color()
                    {
                        r = (float)220 / 255,
                        g = (float)143 / 255,
                        b = (float)133 / 255,
                        a = 1
                    };
                    _firstLayerImg.color = color2;
                    _textArea.color = color2;
                    break;
                case "3":
                    Color color3 = new Color()
                    {
                        r = (float)217 / 255,
                        g = (float)217 / 255,
                        b = (float)217 / 255,
                        a = 1
                    };
                    _firstLayerImg.color = color3;
                    _textArea.color = color3;
                    break;
                case "4":
                    Color color4 = new Color()
                    {
                        r = (float)182 / 255,
                        g = (float)220 / 255,
                        b = (float)133 / 255,
                        a = 1
                    };
                    _firstLayerImg.color = color4;
                    _textArea.color = color4;
                    break;
                case "5":
                    Color color5 = new Color()
                    {
                        r = (float)141 / 255,
                        g = (float)207 / 255,
                        b = (float)246 / 255,
                        a = 1
                    };
                    _firstLayerImg.color = color5;
                    _textArea.color = color5;
                    break;
                case "6":
                    Color color6 = new Color()
                    {
                        r = (float)162 / 255,
                        g = (float)141 / 255,
                        b = (float)246 / 255,
                        a = 1
                    };
                    _firstLayerImg.color = color6;
                    _textArea.color = color6;
                    break;
                case "7":
                    Color color7 = new Color()
                    {
                        r = (float)255 / 255,
                        g = (float)255 / 255,
                        b = (float)255 / 255,
                        a = 1
                    };
                    _firstLayerImg.color = color7;
                    _textArea.color = color7;
                    break;

            }
            
            
        }
        public void ChangeSecondLayerColor(string number)
        {
           secondLayerColorNumber = number;
            inputGroundColorNumber = number;
            switch (number) 
            {
                case "1":
                    Color color1 = new Color()
                    {
                        r = (float)223 / 255,
                        g = (float)178 / 255,
                        b = (float)125 / 255,
                        a = 1
                    };
                    _secondLayerImg.color = color1;
                    _textGround.color = color1;
                    break;
                case "2":
                    Color color2 = new Color()
                    {
                        r = (float)220 / 255,
                        g = (float)143 / 255,
                        b = (float)133 / 255,
                        a = 1
                    };
                    _secondLayerImg.color = color2;
                    _textGround.color = color2;
                    break;
                case "3":
                    Color color3 = new Color()
                    {
                        r = (float)217 / 255,
                        g = (float)217 / 255,
                        b = (float)217 / 255,
                        a = 1
                    };
                    _secondLayerImg.color = color3;
                    _textGround.color = color3;
                    break;
                case "4":
                    Color color4 = new Color()
                    {
                        r = (float)182 / 255,
                        g = (float)220 / 255,
                        b = (float)133 / 255,
                        a = 1
                    };
                    _secondLayerImg.color = color4;
                    _textGround.color = color4;
                    break;
                case "5":
                    Color color5 = new Color()
                    {
                        r = (float)141 / 255,
                        g = (float)207 / 255,
                        b = (float)246 / 255,
                        a = 1
                    };
                    _secondLayerImg.color = color5;
                    _textGround.color = color5;
                    break;
                case "6":
                    Color color6 = new Color()
                    {
                        r = (float)162 / 255,
                        g = (float)141 / 255,
                        b = (float)246 / 255,
                        a = 1
                    };
                    _secondLayerImg.color = color6;
                    _textGround.color = color6;
                    break;
                case "7":
                    Color color7 = new Color()
                    {
                        r = (float)255 / 255,
                        g = (float)255 / 255,
                        b = (float)255 / 255,
                        a = 1
                    };
                    _secondLayerImg.color = color7;
                    _textGround.color = color7;
                    break;

            }
        }
    }
}