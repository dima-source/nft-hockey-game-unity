using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Near.Models.Tokens;
using Near.Models.Tokens.Players;
using Near.Models.Tokens.Players.FieldPlayer;
using Near.Models.Tokens.Players.Goalie;
using NearClientUnity.Utilities;
using TMPro;
using UI.Scripts.Card;
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
            SetInformation();

           Transform buttonsContainer = Utils.FindChild<Transform>(transform, "ButtonContainer");
            _buttons = new Button[buttonsContainer.childCount];
            for (int i = 0; i < buttonsContainer.childCount; i++)
            {
                _buttons[i] = buttonsContainer.GetChild(i).GetComponent<Button>();
            }
        }

        private void SetInformation()
        {
            //string value = $"<uppercase>{CardView.playerName}</uppercase>\nrole: {""} \nposition: {CardView.cardPosition}\n";
            //value += "hand: " + (rightHanded ? "R" : "L");
            //_basicInformationText.text = value;
            
            //string dateFormat = "MM/dd/yyyy";
            //string value = $"nation: {nation}\nbirthday: {birthday.ToString(dateFormat)}\nage: {age}\n";
            //_additionalInformationText.text = value;
        }

        public void SetData(Token token)
        {
            CardView.SetData(token);
            
            if (token.marketplace_data == null || token.marketplace_data.price == null)
            {
                _priceText.text = "";
            }
            else
            {
                _priceText.text = Near.NearUtils.FormatNearAmount(UInt128.Parse(token.marketplace_data.price)).ToString();
            }

            Player player = (Player) token;
            _basicInformationText.text = "<uppercase>" + token.title + "</uppercase>\n";
            
            _basicInformationText.text += "role: " + player.player_role + "\n" +
                                          "position: " + "Goalie\n" +
                                          "hand: " + player.hand + "\n";

            DateTime birthday = DateTimeOffset.FromUnixTimeSeconds(long.Parse(player.birthday)).DateTime;
            
            DateTime today = DateTime.Now; 

            int age = today.Year - birthday.Year;
            if (today.Month < birthday.Month)
            {
                age--;
            } else if (today.Month == birthday.Month)
            {
                if (today.Day < birthday.Day)
                {
                    age--;
                }
            }
            
            _additionalInformationText.text = "nation: " + player.nationality + "\n" +
                                              "birthday: " + birthday.Year + "/" + birthday.Month + "/" + birthday.Day + "\n" +
                                              "age: " + age;

            if (token.player_type == "Goalie")
            {
                Goalie goalie = (Goalie)token;
                Drawer.statistics = new List<PolygonDrawer.Statistic>
                {
                    new("Reflexes", (int) goalie.Stats.Reflexes),
                    new("Puck control", (int) goalie.Stats.PuckControl),
                    new("Strength", (int) goalie.Stats.Strength)
                };
            }
            else
            {
                FieldPlayer fieldPlayer = (FieldPlayer)token;
                Drawer.statistics = new List<PolygonDrawer.Statistic>
                {
                    new("Skating", (int) fieldPlayer.Stats.Skating),
                    new("Shooting", (int) fieldPlayer.Stats.Shooting),
                    new("Stick handling", (int) fieldPlayer.Stats.StickHandling),
                    new("Strength", (int) fieldPlayer.Stats.StrengthAvg),
                    new("IQ", (int) fieldPlayer.Stats.Iq),
                    new("Defense", (int) fieldPlayer.Stats.Defense)
                };
            }
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
