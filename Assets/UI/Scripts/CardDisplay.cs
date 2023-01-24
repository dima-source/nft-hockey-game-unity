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

            DateTime birthday = DateTimeOffset.FromUnixTimeSeconds(player.birthday).DateTime;
            
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
                    new("Reflexes", (int) goalie.Stats.Reflexes,
                        new PolygonDrawer.Statistic.SubStatistic[]
                        {
                            new()
                            {
                                Label = "Angles",
                                Value = (int)goalie.Stats.Angles
                            },
                            new()
                            {
                                Label = "Breakaway",
                                Value = (int)goalie.Stats.Breakaway
                            },
                            new ()
                            {
                                Label = "Five Hole",
                                Value = (int)goalie.Stats.FiveHole
                            }
                        }),
                    new("Puck control", (int) goalie.Stats.PuckControl,
                        new PolygonDrawer.Statistic.SubStatistic[]
                        {
                            new()
                            {
                                Label = "Passing",
                                Value = (int)goalie.Stats.Passing
                            },
                            new()
                            {
                                Label = "Poise",
                                Value = (int)goalie.Stats.Poise
                            },
                            new()
                            {
                                Label = "PuckPlaying",
                                Value = (int)goalie.Stats.PuckPlaying
                            },
                            new()
                            {
                                Label = "Recover",
                                Value = (int)goalie.Stats.Recover
                            },
                            new()
                            {
                                Label = "ReboundControl",
                                Value = (int)goalie.Stats.ReboundControl
                            },
                            new ()
                            {
                                Label = "PokeCheck",
                                Value = (int)goalie.Stats.PokeCheck
                            }
                        }),
                    new("Strength", (int) goalie.Stats.Strength,
                        new PolygonDrawer.Statistic.SubStatistic[]
                        {
                            new()
                            {
                                Label = "Aggressiveness",
                                Value = (int)goalie.Stats.Aggressiveness
                            },
                            new()
                            {
                                Label = "Agility",
                                Value = (int)goalie.Stats.Agility
                            },
                            new()
                            {
                                Label = "Durability",
                                Value = (int)goalie.Stats.Durability
                            },
                            new()
                            {
                                Label = "Endurance",
                                Value = (int)goalie.Stats.Endurance
                            },
                            new()
                            {
                                Label = "Speed",
                                Value = (int)goalie.Stats.Speed
                            },
                            new()
                            {
                                Label = "Morale",
                                Value = (int)goalie.Stats.Morale
                            },
                            new ()
                            {
                                Label = "Vision",
                                Value = (int)goalie.Stats.Vision
                            }
                        }),
                        
                };
            }
            else
            {
                FieldPlayer fieldPlayer = (FieldPlayer)token;
                Drawer.statistics = new List<PolygonDrawer.Statistic>
                {
                    new("Skating", (int) fieldPlayer.Stats.Skating,
                        new PolygonDrawer.Statistic.SubStatistic[]
                        {
                            new()
                            {
                                Label = "Acceleration",
                                Value = (int)fieldPlayer.Stats.Acceleration
                            },
                            new()
                            {
                                Label = "Agility",
                                Value = (int)fieldPlayer.Stats.Agility
                            },
                            new()
                            {
                                Label = "Balance",
                                Value = (int)fieldPlayer.Stats.Balance
                            },
                            new()
                            {
                                Label = "Endurance",
                                Value = (int)fieldPlayer.Stats.Endurance
                            },
                            new()
                            {
                                Label = "Speed",
                                Value = (int)fieldPlayer.Stats.Speed
                            }
                        }),
                    new("Shooting", (int) fieldPlayer.Stats.Shooting,
                        new PolygonDrawer.Statistic.SubStatistic[]
                        {
                            new()
                            {
                                Label = "SlapShotAccuracy",
                                Value = (int)fieldPlayer.Stats.SlapShotAccuracy
                            },
                            new()
                            {
                                Label = "SlapShotPower",
                                Value = (int)fieldPlayer.Stats.SlapShotPower
                            },
                            new()
                            {
                                Label = "WristShotAccuracy",
                                Value = (int)fieldPlayer.Stats.WristShotAccuracy
                            },
                            new()
                            {
                                Label = "WristShotPower",
                                Value = (int)fieldPlayer.Stats.WristShotPower
                            }
                        }),
                    new("Stick handling", (int) fieldPlayer.Stats.StickHandling,
                        new PolygonDrawer.Statistic.SubStatistic[]
                        {
                            new()
                            {
                                Label = "Deking",
                                Value = (int)fieldPlayer.Stats.Deking
                            },
                            new()
                            {
                                Label = "HandEye",
                                Value = (int)fieldPlayer.Stats.HandEye
                            },
                            new()
                            {
                                Label = "Passing",
                                Value = (int)fieldPlayer.Stats.Passing
                            },
                            new()
                            {
                                Label = "PuckControl",
                                Value = (int)fieldPlayer.Stats.PuckControl
                            }
                        }),
                    new("Strength", (int) fieldPlayer.Stats.StrengthAvg,
                        new PolygonDrawer.Statistic.SubStatistic[]
                        {
                            new()
                            {
                                Label = "Aggressiveness",
                                Value = (int)fieldPlayer.Stats.Aggressiveness
                            },
                            new()
                            {
                                Label = "BodyChecking",
                                Value = (int)fieldPlayer.Stats.BodyChecking
                            },
                            new()
                            {
                                Label = "Durability",
                                Value = (int)fieldPlayer.Stats.Durability
                            },
                            new()
                            {
                                Label = "FightingSkill",
                                Value = (int)fieldPlayer.Stats.FightingSkill
                            },
                            new()
                            {
                                Label = "Strength",
                                Value = (int)fieldPlayer.Stats.Strength
                            }
                        }),
                    new("IQ", (int) fieldPlayer.Stats.Iq,
                        new PolygonDrawer.Statistic.SubStatistic[]
                        {
                            new()
                            {
                                Label = "Discipline",
                                Value = (int)fieldPlayer.Stats.Discipline
                            },
                            new()
                            {
                                Label = "Poise",
                                Value = (int)fieldPlayer.Stats.Poise
                            },
                            new()
                            {
                                Label = "Morale",
                                Value = (int)fieldPlayer.Stats.Morale
                            },
                            new()
                            {
                                Label = "Offensive",
                                Value = (int)fieldPlayer.Stats.Offensive
                            }
                        }),
                    new("Defense", (int) fieldPlayer.Stats.Defense,
                        new PolygonDrawer.Statistic.SubStatistic[]
                        {
                            new()
                            {
                                Label = "DefensiveAwareness",
                                Value = (int)fieldPlayer.Stats.DefensiveAwareness
                            },
                            new()
                            {
                                Label = "FaceOffs",
                                Value = (int)fieldPlayer.Stats.FaceOffs
                            },
                            new()
                            {
                                Label = "ShotBlocking",
                                Value = (int)fieldPlayer.Stats.ShotBlocking
                            },
                            new()
                            {
                                Label = "StickChecking",
                                Value = (int)fieldPlayer.Stats.StickChecking
                            }
                        })
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
