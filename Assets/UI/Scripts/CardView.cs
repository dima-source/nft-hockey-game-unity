using System;
using System.Data;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace UI.Scripts
{
    public class CardView : UiComponent
    {
        public enum PlayerRole
        {
            Playmaker,
            Enforcer,
            Shooter,
            TryHarder,
            DefensiveForward,
            Grinder,
            DefensiveDefenceman,
            OffensiveDefenceman,
            TwoWayDefencemen,
            ToughGuy,
            StandUp,
            Butterfly,
            Hybrid
        }

        public enum Position
        {
            RW,
            LW,
            RD,
            LD,
            C
        }
        
        public enum Rareness 
        {
            Usual,
            Rare,
            Epic,
            Legendary
        }

        [Header("Main")]
        public string avatarImagePath;
        public int year;
        public Position position;

        [Header("Personal")] public string playerName = "";
        public string playerSurname = "";
        public int playerNumber;
        public PlayerRole playerRole;
        public Rareness rareness;
        
        public int[] statistics;

        private Transform _statisticsContainer;
        private TextMeshProUGUI[] _statisticViews;
        private TextMeshProUGUI _year;
        private TextMeshProUGUI _position;
        private TextMeshProUGUI _nameText;
        private TextMeshProUGUI _surnameText;
        private TextMeshProUGUI _numberText;
        private Image _avatar;
        private TextMeshProUGUI _playerRoleText;
        private TextInformation _background;


        protected override void Initialize()
        {
            _year = Utils.FindChild<TextMeshProUGUI>(transform, "Year");
            _position = Utils.FindChild<TextMeshProUGUI>(transform, "Position");
            _nameText = Utils.FindChild<TextMeshProUGUI>(transform, "NameText");
            _surnameText = Utils.FindChild<TextMeshProUGUI>(transform, "SurnameText");
            _numberText = Utils.FindChild<TextMeshProUGUI>(transform, "NumberText");
            _avatar = Utils.FindChild<Image>(transform, "Icon");
            _playerRoleText = Utils.FindChild<TextMeshProUGUI>(transform, "RoleText");
            _background = Utils.FindChild<TextInformation>(transform, "Background");
            
            _statisticsContainer = Utils.FindChild<Transform>(transform, "BottomRow");
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
            _avatar.sprite = Utils.LoadSprite(Configurations.SpritesFolderPath + avatarImagePath);
            _playerRoleText.text = RoleToString(playerRole);
            _background.material = RarenessToMaterial(rareness);
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

        private static string RoleToString(PlayerRole role)
        {
            return role switch
            {
                PlayerRole.Playmaker => "Playmaker",
                PlayerRole.Enforcer => "Enforcer",
                PlayerRole.Shooter => "Shooter",
                PlayerRole.TryHarder => "Try-harder",
                PlayerRole.DefensiveForward => "Defensive forward",
                PlayerRole.Grinder => "Grinder",
                PlayerRole.DefensiveDefenceman => "Defensive defenceman",
                PlayerRole.OffensiveDefenceman => "Offensive defenceman",
                PlayerRole.TwoWayDefencemen => "Two-way defencemen",
                PlayerRole.ToughGuy => "Tough guy",
                PlayerRole.StandUp => "Standup",
                PlayerRole.Butterfly => "Butterfly",
                PlayerRole.Hybrid => "Hybrid",
                _ => throw new ApplicationException("Unsupported role")
            };
        }
        
        // TODO: change colors
        private static TextInformation.BackgroundMaterial RarenessToMaterial(Rareness rareness)
        {
            return rareness switch
            {
                Rareness.Usual => TextInformation.BackgroundMaterial.AccentBackgroundCold,
                Rareness.Rare => TextInformation.BackgroundMaterial.AccentBackgroundHot,
                Rareness.Epic => TextInformation.BackgroundMaterial.AccentBackground1,
                Rareness.Legendary => TextInformation.BackgroundMaterial.AccentBackground2,
                _ => throw new ApplicationException("Unsupported rareness")
            };
        }
        
    }
}
