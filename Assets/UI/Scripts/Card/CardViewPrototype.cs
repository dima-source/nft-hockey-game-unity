using System;
using TMPro;
using UI.Scripts.Card.CardStatistics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.Card
{
    public class CardViewPrototype
    { 
        
        private readonly Transform transform;
        private readonly PlayerCardData playerCardData;
        
        private readonly Graphic graphic;
        private readonly Text text;
        private CardStatisticView[] cardStatisticViews;

        public CardViewPrototype(Transform transform, PlayerCardData playerCardData)
        {
            this.transform = transform;
            this.playerCardData = playerCardData;
            
            graphic = new Graphic(transform);
            text = new Text(transform);
            FindCardStatisticViews();
        }
        
        private class Graphic
        {
            public readonly Image background;
            public readonly Image avatar;
            public RectRenderer[] rarenessIndicators;

            public Graphic(Transform transform)
            {
                background = Utils.FindChild<Image>(transform, "BackgroundImage");
                avatar = Utils.FindChild<Image>(transform, "AvatarImage");
                InitializeRarenessIndicators(transform);
            }

            private void InitializeRarenessIndicators(Transform transform)
            {
                rarenessIndicators = new RectRenderer[3];
                rarenessIndicators[0] = Utils.FindChild<StraightTrapezoidRenderer>(transform, "LeftTrapezoid");
                rarenessIndicators[1] = Utils.FindChild<ParallelogramRenderer>(transform, "Parallelogram");
                rarenessIndicators[2] = Utils.FindChild<StraightTrapezoidRenderer>(transform, "UpTrapezoid");
            }
        }
            
        private class Text
        {
            public readonly TextMeshProUGUI name;
            public readonly TextMeshProUGUI position;
            public readonly TextMeshProUGUI playerNumber;
            public readonly TextMeshProUGUI playerRole;  
                
            public Text(Transform transform)
            {
                name = Utils.FindChild<TextMeshProUGUI>(transform, "Name");
                position = Utils.FindChild<TextMeshProUGUI>(transform, "Position");
                playerNumber = Utils.FindChild<TextMeshProUGUI>(transform, "Number");
                playerRole = Utils.FindChild<TextMeshProUGUI>(transform, "Role");
            }
        }

        private void FindCardStatisticViews()
        {
            Transform statisticsContainer = Utils.FindChild<Transform>(transform, "StatisticsContainer");
            cardStatisticViews = new CardStatisticView[statisticsContainer.childCount];
            for (int i = 0; i < cardStatisticViews.Length; i++)
            {
                Transform statistic = statisticsContainer.GetChild(i);
                cardStatisticViews[i] = statistic.GetComponent<CardStatisticView>();
            }
        }

        public void UpdateView()
        {
            UpdateGraphic();
            UpdateText();
            UpdateStatisticViews();
        }

        private void UpdateGraphic()
        {
            graphic.background.sprite = playerCardData.background;
            graphic.avatar.sprite = playerCardData.avatar;
            foreach (var indicator in graphic.rarenessIndicators)
            {
                indicator.color = playerCardData.rareness.ToColor();
            }
        }
        
        
        private void UpdateText()
        {
            text.name.text = playerCardData.name;
            text.position.text = playerCardData.position.ToString();
            text.playerNumber.text = playerCardData.number.ToString();
            text.playerRole.text = playerCardData.role.ToString();
        }

        private void UpdateStatisticViews()
        {
            for (int i = 0; i < cardStatisticViews.Length; i++)
            {
                CardStatistic statistic = playerCardData.statistics[i];
                CardStatisticView view = cardStatisticViews[i];
                statistic.DisplayTo(view);
            }
        }
            
    }
}