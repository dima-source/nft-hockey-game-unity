using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Profile
{
    public class CreateTeamLogoView : MonoBehaviour
    {
        [SerializeField] private Image firstLayerImg;
        [SerializeField] private Image secondLayerImg;
        [SerializeField] private Color firstLayerColor;
        private readonly string _pathForm = "\\Assets\\Sprites\\Profile\\Form\\";
        private readonly string _pathPattern = "\\Assets\\Sprites\\Profile";
        private string _formName = "Square"; 
        private string _patternName = "1";  

        private void Awake()
        {
            ChangeForm("Star");
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
            string newPath = directoryPath + _pathForm + "\\"+ _formName + ".png";
            
            Texture2D texture = Utils.ImageLoader.LoadTexture2D(newPath);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            firstLayerImg.sprite = sprite;
            
            ChangeLayerPattern();
        }
        
        private void ChangeLayerPattern()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            string newPath = directoryPath + _pathPattern + "\\" + _formName + "\\" + _patternName + ".png";
            
            Texture2D texture = Utils.ImageLoader.LoadTexture2D(newPath);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            secondLayerImg.sprite = sprite;
            //secondLayerImg.color = firstLayerColor;
        }

        public void ChangeFirstLayerColor(string number)
        {
            switch (number) 
            {
                case "1":
                    firstLayerImg.color = Color.yellow;
                    break;
                case "2":
                    firstLayerImg.color= Color.blue;
                    break;
                case "3":
                    firstLayerImg.color= Color.green;
                    break;
                case "4":
                    firstLayerImg.color = Color.black;
                    break;
                case "5":
                    firstLayerImg.color= Color.cyan;
                    break;
                case "6":
                    firstLayerImg.color= Color.grey;
                    break;
                case "7":
                    firstLayerImg.color= Color.magenta;
                    break;

            }
        }
        
        public void ChangeSecondLayerColor(string number)
        {
            switch (number) 
            {
                case "1":
                    secondLayerImg.color = Color.yellow;
                    break;
                case "2":
                    secondLayerImg.color= Color.blue;
                    break;
                case "3":
                    secondLayerImg.color= Color.green;
                    break;
                case "4":
                    secondLayerImg.color = Color.black;
                    break;
                case "5":
                    secondLayerImg.color= Color.cyan;
                    break;
                case "6":
                    secondLayerImg.color= Color.grey;
                    break;
                case "7":
                    secondLayerImg.color= Color.magenta;
                    break;

            
               // case "2":
                  //  Color color = new Color()
                  //  {
                  //      r = 300,
                  //      g = 200,
                  //      b = 200,
                  //      a = 0 // Прозрачность
                  //  };

                   
            }
        }
        
    }
}