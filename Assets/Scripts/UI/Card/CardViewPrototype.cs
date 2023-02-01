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
        private readonly int FIELD_PLAYER_STATS_COUNT = 6;
        private readonly int GOALIE_STATS_COUNT = 3;
        private readonly Transform transform;
        private readonly PlayerCardData playerCardData;
        
        private readonly Graphic graphic;
        private readonly Text text;
        private CardStatisticView[] cardStatisticViews;

        private bool _foundCardStatisticsViews;

        public CardViewPrototype(Transform transform, PlayerCardData playerCardData)
        {
            this.transform = transform;
            this.playerCardData = playerCardData;
            
            graphic = new Graphic(transform);
            text = new Text(transform);
            // FindCardStatisticViews();
        }
        
        private class Graphic
        {
            public readonly Image background;
            public readonly Image avatar;
            public RectRenderer[] rarenessIndicators;

            public Graphic(Transform transform)
            {
                background = UiUtils.FindChild<Image>(transform, "BackgroundImage");
                avatar = UiUtils.FindChild<Image>(transform, "AvatarImage");
                InitializeRarenessIndicators(transform);
            }

            private void InitializeRarenessIndicators(Transform transform)
            {
                rarenessIndicators = new RectRenderer[3];
                rarenessIndicators[0] = UiUtils.FindChild<StraightTrapezoidRenderer>(transform, "LeftTrapezoid");
                rarenessIndicators[1] = UiUtils.FindChild<ParallelogramRenderer>(transform, "Parallelogram");
                rarenessIndicators[2] = UiUtils.FindChild<StraightTrapezoidRenderer>(transform, "UpTrapezoid");
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
                name = UiUtils.FindChild<TextMeshProUGUI>(transform, "Name");
                position = UiUtils.FindChild<TextMeshProUGUI>(transform, "Position");
                playerNumber = UiUtils.FindChild<TextMeshProUGUI>(transform, "Number");
                playerRole = UiUtils.FindChild<TextMeshProUGUI>(transform, "Role");
            }
        }

        private void FindCardStatisticViews()
        {
            
            Transform statisticsContainer = UiUtils.FindChild<Transform>(transform, "StatisticsContainer");
            if (playerCardData.position.ToString() != "G")
            {
                int indexCounter = 0;
                cardStatisticViews = new CardStatisticView[FIELD_PLAYER_STATS_COUNT];
                for (int i = 0; i < FIELD_PLAYER_STATS_COUNT; i++)
                {
                    Transform statistic = statisticsContainer.GetChild(i);
                    statistic.gameObject.SetActive(true);
                    cardStatisticViews[indexCounter] = statistic.GetComponent<CardStatisticView>();
                    indexCounter++;
                }

                if(Application.isPlaying){
                    for (int i = FIELD_PLAYER_STATS_COUNT; i < FIELD_PLAYER_STATS_COUNT + GOALIE_STATS_COUNT; i++)
                    {
                        statisticsContainer.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                cardStatisticViews = new CardStatisticView[GOALIE_STATS_COUNT];
                int indexCounter = 0;
                for (int i = FIELD_PLAYER_STATS_COUNT; i < FIELD_PLAYER_STATS_COUNT + GOALIE_STATS_COUNT; i++)
                {
                    Transform statistic = statisticsContainer.GetChild(i);
                    statistic.gameObject.SetActive(true);
                    cardStatisticViews[indexCounter] = statistic.GetComponent<CardStatisticView>();
                    indexCounter++;
                }

                for (int i = 0; i < 6; i++)
                {
                    statisticsContainer.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        public void UpdateView()
        {
            if (!_foundCardStatisticsViews)
            {
                FindCardStatisticViews();
                _foundCardStatisticsViews = true;
            }
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
            if (playerCardData.rareness.ToString() == "Common")
                text.playerRole.color = Color.black;
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