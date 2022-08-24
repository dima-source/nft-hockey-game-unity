using System;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UI.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.ManageTeam
{
    public class CardView : UiComponent
    {
        protected bool updateAvatar = true;


        [Header("Main")]
        public string avatarImagePath;
        public int year;
        public Scripts.CardView.Position position;

        [Header("Personal")] public string playerName = "";
        public string playerSurname = "";
        public int playerNumber;
        public Scripts.CardView.PlayerRole playerRole;
        public Scripts.CardView.Rareness rareness;
        
        public int[] statistics;

        private Transform _statisticsContainer;
        private TextMeshProUGUI[] _statisticViews;
        private TextMeshProUGUI _year;
        private TextMeshProUGUI _position;
        private TextMeshProUGUI _nameText;
        private TextMeshProUGUI _surnameText;
        private TextMeshProUGUI _numberText;
        protected Image _avatar;
        private TextMeshProUGUI _playerRoleText;
        private TextInformation _background;


        protected override void Initialize()
        {
            _year = UI.Scripts.Utils.FindChild<TextMeshProUGUI>(transform, "Year");
            _position = UI.Scripts.Utils.FindChild<TextMeshProUGUI>(transform, "Position");
            _nameText = UI.Scripts.Utils.FindChild<TextMeshProUGUI>(transform, "NameText");
            _surnameText = UI.Scripts.Utils.FindChild<TextMeshProUGUI>(transform, "SurnameText");
            _numberText = UI.Scripts.Utils.FindChild<TextMeshProUGUI>(transform, "NumberText");
            _avatar = UI.Scripts.Utils.FindChild<Image>(transform, "Icon");
            _playerRoleText = UI.Scripts.Utils.FindChild<TextMeshProUGUI>(transform, "RoleText");
            _background = UI.Scripts.Utils.FindChild<TextInformation>(transform, "Background");
            
            _statisticsContainer = UI.Scripts.Utils.FindChild<Transform>(transform, "BottomRow");
            _statisticViews = new TextMeshProUGUI[_statisticsContainer.childCount];
            for (int i = 0; i < _statisticsContainer.childCount; i++)
            {
                _statisticViews[i] = _statisticsContainer.GetChild(i).GetComponent<TextMeshProUGUI>();
            }
        }

        protected override void OnUpdate()
        {
            _year.text = year.ToString();
            _position.text = position.ToString();
            _nameText.text = playerName;
            _surnameText.text = playerSurname;
            _numberText.text = playerNumber.ToString();
            if (updateAvatar) 
                _avatar.sprite = UI.Scripts.Utils.LoadSprite(Configurations.SpritesFolderPath + avatarImagePath);
            else
            {
                if (_avatar.sprite.IsUnityNull())
                {
                    _avatar.sprite = UI.Scripts.Utils.LoadSprite(Configurations.SpritesFolderPath + avatarImagePath);
                }
            }
            _playerRoleText.text = RoleToString(playerRole);
            // TODO: 
            //_background.material = RarenessToMaterial(rareness);
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            if (statistics.Length > _statisticsContainer.childCount)
            {
                statistics = statistics.Take(_statisticsContainer.childCount).ToArray();   
            }
            
            for (int i = 0; i < _statisticViews.Length; i++)
            {
                GameObject statisticsObject = _statisticViews[i].gameObject;
                statisticsObject.SetActive(i < statistics.Length);
                if (i < statistics.Length)
                {
                    _statisticViews[i].text = statistics[i].ToString();   
                }
            }
        }

        private static string RoleToString(Scripts.CardView.PlayerRole role)
        {
            return role switch
            {
                Scripts.CardView.PlayerRole.Playmaker => "Playmaker",
                Scripts.CardView.PlayerRole.Enforcer => "Enforcer",
                Scripts.CardView.PlayerRole.Shooter => "Shooter",
                Scripts.CardView.PlayerRole.TryHarder => "Try-harder",
                Scripts.CardView.PlayerRole.DefensiveForward => "Defensive forward",
                Scripts.CardView.PlayerRole.Grinder => "Grinder",
                Scripts.CardView.PlayerRole.DefensiveDefenceman => "Defensive defenceman",
                Scripts.CardView.PlayerRole.OffensiveDefenceman => "Offensive defenceman",
                Scripts.CardView.PlayerRole.TwoWayDefencemen => "Two-way defencemen",
                Scripts.CardView.PlayerRole.ToughGuy => "Tough guy",
                Scripts.CardView.PlayerRole.StandUp => "Standup",
                Scripts.CardView.PlayerRole.Butterfly => "Butterfly",
                Scripts.CardView.PlayerRole.Hybrid => "Hybrid",
                _ => throw new ApplicationException("Unsupported role")
            };
        }
        
        // TODO: change colors
        /*
        private static TextInformation.BackgroundMaterial RarenessToMaterial(Scripts.CardView.Rareness rareness)
        {
            return rareness switch
            {
                Scripts.CardView.Rareness.Usual => TextInformation.BackgroundMaterial.AccentBackgroundCold,
                Scripts.CardView.Rareness.Rare => TextInformation.BackgroundMaterial.AccentBackgroundHot,
                Scripts.CardView.Rareness.=> TextInformation.BackgroundMaterial.AccentBackground1,
                Scripts.CardView.Rareness.Legendary => TextInformation.BackgroundMaterial.AccentBackground2,
                _ => throw new ApplicationException("Unsupported rareness")
            };
        }
        */
    }
}
