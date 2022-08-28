using System;
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
            G,
            C
        }
        
        public enum Rareness 
        {
            Usual,
            Rare,
            SuperRare,
            Myth,
            Exclusive
        }

        [Header("Main")] 
        public bool isAuction;
        public string avatarImagePath;
        public int year;
        public Position position;

        [Header("Personal")] 
        [Range(1, 20)]
        public int nameSpacing;
        public string playerName = "";
        public string playerSurname = "";
        public int playerNumber;
        public PlayerRole playerRole;
        public Rareness rareness;

        [Range(1, 20)]
        public int statisticsSpacing;
        public int[] statistics;
        

        private TextMeshProUGUI _statistics;
        private TextMeshProUGUI _year;
        private TextMeshProUGUI _position;
        private TextMeshProUGUI _nameText;
        private TextMeshProUGUI _numberText;
        protected Image _avatar;
        private TextMeshProUGUI _playerRoleText;
        private Image _background;

        private RectTransform _transform;
        public RectTransform RectTransform => _transform;


        protected override void Initialize()
        {
            _year = Utils.FindChild<TextMeshProUGUI>(transform, "Year");
            _position = Utils.FindChild<TextMeshProUGUI>(transform, "Position");
            _nameText = Utils.FindChild<TextMeshProUGUI>(transform, "NameText");
            _numberText = Utils.FindChild<TextMeshProUGUI>(transform, "NumberText");
            _avatar = Utils.FindChild<Image>(transform, "Icon");
            _playerRoleText = Utils.FindChild<TextMeshProUGUI>(transform, "RoleText");
            _background = Utils.FindChild<Image>(transform, "Background");
            
            _statistics = Utils.FindChild<TextMeshProUGUI>(transform, "Statistics");
            _transform = GetComponent<RectTransform>();

            isAuction = true;
        }

        protected override void OnUpdate()
        {
            _year.text = year.ToString();
            _position.text = position.ToString();
            _numberText.text = playerNumber.ToString();
            UpdateName();
            _avatar.sprite = Utils.LoadSprite(Configurations.SpritesFolderPath + avatarImagePath);
            _playerRoleText.text = RoleToString(playerRole);
            _background.GetComponent<Image>().material = RarenessToMaterial(rareness);
            UpdateStatistics();
        }

        private void UpdateName()
        {
            _nameText.text = playerName.ToLower() + new String(' ', nameSpacing) + 
                             "<color=\"red\">" + playerSurname.ToUpper() + "</color>";
        }

        private void UpdateStatistics()
        {
            string space = new String(' ', statisticsSpacing);
            string[] toDisplay = new string[statistics.Length];
            for (int i = 0; i < statistics.Length; i++)
            {
                string value = statistics[i].ToString();
                if (i % 2 == 0)
                {
                    value = "<color=\"red\">" + value + "</color>";
                }

                toDisplay[i] = value;
            }
            _statistics.text = String.Join(space, toDisplay);
        }

        public static string RoleToString(PlayerRole role)
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
        private static Material RarenessToMaterial(Rareness rareness)
        {
            string path = rareness switch
            {
                Rareness.Usual => Configurations.MaterialsFolderPath + "AccentBackgroundCold",
                Rareness.Rare => Configurations.MaterialsFolderPath + "AccentBackgroundHot",
                Rareness.SuperRare => Configurations.MaterialsFolderPath + "AccentBackground1",
                Rareness.Myth => Configurations.MaterialsFolderPath + "AccentBackground2",
                Rareness.Exclusive => Configurations.MaterialsFolderPath + "AccentBackground2",
                _ => throw new ApplicationException("Unsupported rareness")
            };
            return Utils.LoadResource<Material>(path);
        }

        public void Enable(bool value)
        {
            MonoBehaviour[] comps = GetComponentsInChildren<MonoBehaviour>();
            foreach(MonoBehaviour c in comps)
            {
                c.enabled = value;
            }
        }
        
    }
}
