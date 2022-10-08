using UnityEngine;
using UnityEngine.UI;
using Directory = System.IO.Directory;

namespace UI.Profile
{
    public class CreateTeamLogoView : MonoBehaviour
    {
        [SerializeField] private Image firstLayerImg;
        [SerializeField] private Image secondLayerImg;
        [SerializeField] private Color firstLayerColor;
        private readonly string _pathForm = "\\Assets\\Sprites\\Profile\\Form\\";
        private readonly string _pathPattern = "\\Assets\\Sprites\\Profile\\";
        private string _formName = "Square";  // Shield
        private string _patternName = "1";  

        private void Awake()
        {
            ChangeForm("Square");
        }

        public void ChangeForm(string form)
        {
            _formName = form;
            ChangeLayerForm();
        }
        
        private void ChangeLayerForm()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            string newPath = directoryPath + _pathForm + _formName + ".png";
            
            Texture2D texture = Utils.ImageLoader.LoadTexture2D(newPath);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            firstLayerImg.sprite = sprite;
            
            ChangeLayerPattern();
        }
        
        private void ChangeLayerPattern()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            string newPath = directoryPath + _pathPattern + "\\" + _formName + _patternName + ".png";
            
            Texture2D texture = Utils.ImageLoader.LoadTexture2D(newPath);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            secondLayerImg.sprite = sprite;
            secondLayerImg.color = firstLayerColor;
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
                    Color color = new Color()
                    {
                        r = 300,
                        g = 200,
                        b = 200,
                        a = 0 // Прозрачность
                    };

                    secondLayerImg.color = color;
                    break;
            }
        }
    }
}