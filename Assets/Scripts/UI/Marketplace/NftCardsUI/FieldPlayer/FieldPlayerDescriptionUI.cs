using UnityEngine;
using UnityEngine.UI;

namespace UI.Marketplace.NftCardsUI.FieldPlayer
{
    public class FieldPlayerDescriptionUI : NftCardDescriptionUI
    {
        [SerializeField] private Text number;
        [SerializeField] private Text position;
        [SerializeField] private Text role;

        [SerializeField] private Text skatingAvg;
        [SerializeField] private Text shootingAvg;
        [SerializeField] private Text strengthAvg;
        [SerializeField] private Text defenceAvg;
        [SerializeField] private Text iq;
        [SerializeField] private Text morale;
        [SerializeField] private Text stickHandling;
        [SerializeField] private Text acceleration;
        [SerializeField] private Text agility;
        [SerializeField] private Text balance;
        [SerializeField] private Text endurance;
        [SerializeField] private Text speed;
        [SerializeField] private Text deking;
        [SerializeField] private Text hand_eye;
        [SerializeField] private Text passing;
        [SerializeField] private Text puck_controle;
        [SerializeField] private Text aggressiveness;
        [SerializeField] private Text bodyChecking;
        [SerializeField] private Text durability;
        [SerializeField] private Text fightingSkill;
        [SerializeField] private Text strength;
        [SerializeField] private Text discipline;
        [SerializeField] private Text offensiveAwareness;
        [SerializeField] private Text poise;
        [SerializeField] private Text slapShotAccuracy;
        [SerializeField] private Text slapShotPower;
        [SerializeField] private Text wristShotAccuracy;
        [SerializeField] private Text wristShotPower;
        [SerializeField] private Text defensiveAwareness;
        [SerializeField] private Text faceoffs;
        [SerializeField] private Text shotBlocking;
        [SerializeField] private Text stickChecking;

        public Text ShotBlocking => shotBlocking;
        public Text StickChecking => stickChecking;
        public Text OffensiveAwareness => offensiveAwareness;
        public Text Poise => poise;
        public Text SlapShotAccuracy => slapShotAccuracy;
        public Text SlapShotPower => slapShotPower;
        public Text WristShotAccuracy => wristShotAccuracy;
        public Text WristShotPower => wristShotPower;
        public Text DefensiveAwareness => defensiveAwareness;
        public Text Faceoffs => faceoffs;
        public Text BodyChecking => bodyChecking;
        public Text Durability => durability;
        public Text FightingSkill => fightingSkill;
        public Text Strength => strength;
        public Text Discipline => discipline;
        public Text Acceleration => acceleration;
        public Text Agility => agility;
        public Text Balance => balance;
        public Text Endurance => endurance;
        public Text Speed => speed;
        public Text Deking => deking;
        public Text Hand_eye => hand_eye;
        public Text Passing => passing;
        public Text Puck_controle => puck_controle;
        public Text Aggressiveness => aggressiveness;
        public Text Number => number;
        public Text StickHandling => stickHandling;
        public Text Position => position;
        public Text Role => role;
        public Text DefenceAvg => defenceAvg;
        public Text SkatingAvg => skatingAvg;
        public Text ShootingAvg => shootingAvg;
        public Text StrengthAvg => strengthAvg;
        public Text Iq => iq;
        public Text Morale => morale;
    }
}