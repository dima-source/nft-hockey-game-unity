using System;
using UI.Scripts.Constraints;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.Card.CardStatistics
{
    public abstract class CardStatistic : CharacteristicImpl<int>, IFormattable
    {

        private static readonly string SPRITES_PATH = Configurations.SpritesFolderPath + "SpriteSheets/PlayerCardSpriteSheet/";

        private static readonly int LOW_NUMBER_BOUND = 0;
        private static readonly int UPPER_NUMBER_BOUND = 100;
        
        protected abstract string SpriteName { get; }

        public CardStatistic(int characteristic) : base(characteristic, 
            RangeConstraint<int>.FromBounds(LOW_NUMBER_BOUND, UPPER_NUMBER_BOUND))
        {
            
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return characteristic.ToString(format, formatProvider);
        }

        public void DisplayTo(CardStatisticView view)
        {
            view.statisticSprite = GetStatisticSprite();
            view.FillAmount = GetNormalized();
        }
        
        private float GetNormalized()
        {
            int valueInterval = UPPER_NUMBER_BOUND - LOW_NUMBER_BOUND;
            return characteristic / (float) valueInterval;
        }

        private Sprite GetStatisticSprite()
        {
            string spritePath = SPRITES_PATH + SpriteName;
            return Utils.LoadSprite(spritePath);
        }
        
    }
}