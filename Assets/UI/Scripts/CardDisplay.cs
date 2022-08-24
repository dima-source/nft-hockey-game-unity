using System;
using System.Runtime.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class CardDisplay : UiComponent
    {
        public CardView CardView { get; private set; }
        public PolygonDrawer Drawer { get; private set; }
        
        private TextMeshProUGUI _priceText;
        private TextMeshProUGUI _basicInformationText;
        private TextMeshProUGUI _additionalInformationText;

        private Button[] _buttons;

        public int Price { set => _priceText.text = value + " <sprite name=NearLogo>"; }
        

        protected override void Initialize()
        {
            CardView = Utils.FindChild<CardView>(transform, "CardView");
            Drawer = Utils.FindChild<PolygonDrawer>(transform, "GraphContainer");
            _priceText = Utils.FindChild<TextMeshProUGUI>(transform, "Price");
            _basicInformationText = Utils.FindChild<TextMeshProUGUI>(transform, "Basic");
            _additionalInformationText = Utils.FindChild<TextMeshProUGUI>(transform, "Secondary");
            SetBasicInformation("Name Surname", CardView.PlayerRole.Enforcer, CardView.Position.LD, false);
            SetSecondaryInformation("Ukrainian", DateTime.Now, 19);
            
            Transform buttonsContainer = Utils.FindChild<Transform>(transform, "ButtonContainer");
            _buttons = new Button[buttonsContainer.childCount];
            for (int i = 0; i < buttonsContainer.childCount; i++)
            {
                _buttons[i] = buttonsContainer.GetChild(i).GetComponent<Button>();
            }
        }

        public void SetBasicInformation(string fullName, CardView.PlayerRole role, 
            CardView.Position position, bool rightHanded)
        {
            string value = $"<uppercase>{fullName}</uppercase>\nrole: {CardView.RoleToString(role)}\nposition: {position}\n";
            value += "hand: " + (rightHanded ? "R" : "L");
            _basicInformationText.text = value;
        }
        
        public void SetSecondaryInformation(string nation, DateTime birthday, int age)
        {
            string dateFormat = "MM/dd/yyyy";
            string value = $"nation: {nation}\nbirthday: {birthday.ToString(dateFormat)}\nage: {age}\n";
            _additionalInformationText.text = value;
        }


        public void SetButton(int index, string value, UnityAction action = null)
        {
            _buttons[index].gameObject.SetActive(value != String.Empty);
            _buttons[index].GetComponentInChildren<TextMeshProUGUI>().text = value;
            if (_buttons[index].gameObject.activeSelf && action != null)
            {
                _buttons[index].onClick.RemoveAllListeners();
                _buttons[index].onClick.AddListener(() =>
                {
                    AudioController.LoadClip(Configurations.DefaultButtonSoundPath);
                    AudioController.source.Play();
                    action.Invoke();
                });
            }
        }
    }
}
