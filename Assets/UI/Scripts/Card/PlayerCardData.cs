using System;
using Near.Models.Tokens;
using UI.Scripts.Card.CardStatistics;
using UnityEngine;
using Utils;

namespace UI.Scripts.Card
{
    public class PlayerCardData
    {
        
        public bool isOnAuction;
        public Sprite background;
        public Sprite avatar;
        public string name;
        public CardPositionCharacteristic position;
        public CardNumberCharacteristic number;
        public CardRoleCharacteristic role;
        public CardRarenessCharacteristic rareness;
        public CardStatistic[] statistics;


        public static PlayerCardData FromDefaultValues()
        {
            PlayerCardData playerCardData = new PlayerCardData();
            playerCardData.InitializeDefaultValues();
            return playerCardData;
        }
        
        private void InitializeDefaultValues()
        {
            isOnAuction = false;
            background = Utils.LoadSprite(Configurations.SpritesFolderPath + "background");
            avatar = Utils.LoadSprite(Configurations.SpritesFolderPath + "TestPlayer");
            name = "Player Name";
            position = new("RW");
            number = new(99);
            role = new("Offensive Defenceman");
            rareness = new("Unique");
            InitializeDefaultStatistics();
        }

        private void InitializeDefaultStatistics()
        {
            statistics = new CardStatistic[]
            {
                new SkatingStatistic(44),
                new StickHandlingStatistic(11),
                new ShootingStatistic(65),
                new HockeyIqStatistic(23),
                new StrengthStatistic(54),
                new DefenceStatistic(99)
            };
        }


    }
}