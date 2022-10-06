using System;
using Near.Models.Tokens;
using Near.Models.Tokens.Players.FieldPlayer;
using Near.Models.Tokens.Players.Goalie;
using UI.Scripts.Card.CardStatistics;
using UnityEngine;
using Utils;

namespace UI.Scripts.Card
{
    public class CardView : UiComponent
    {

        public PlayerCardData playerCardData;

        private CardViewPrototype viewPrototype;
        public RectTransform rectTransform => GetComponent<RectTransform>();


        protected override void Initialize()
        {
            playerCardData = PlayerCardData.FromDefaultValues();
            viewPrototype = new CardViewPrototype(transform, playerCardData);
        }

        protected override void OnUpdate()
        {
            viewPrototype.UpdateView();
        }


        public void Enable(bool value)
        {
            MonoBehaviour[] comps = GetComponentsInChildren<MonoBehaviour>();
            foreach(MonoBehaviour c in comps)
            {
                c.enabled = value;
            }
        }
        
        public void SetData(Token token)
        {
            if (playerCardData == null)
            {
                return;
            }
            
            if (token.marketplace_data != null)
            {
                if (token.marketplace_data.offers != null)
                {
                    playerCardData.isOnAuction = true;
                }
                else
                {
                    playerCardData.isOnAuction = false;
                }
            }

            if (!string.IsNullOrEmpty(token.media))
            {
                try
                {
                    StartCoroutine(ImageLoader.LoadImage(token.media, (sprite) =>
                    {
                        playerCardData.avatar = sprite;
                    }));
                } catch (ApplicationException) {}
            }
            
            playerCardData.name = token.title;
            playerCardData.tokenData = token;
            
            if (token.player_type == "Goalie")
            {
                Goalie goalie = (Goalie)token;
                playerCardData.number = new CardNumberCharacteristic(goalie.number);
                playerCardData.role = new CardRoleCharacteristic(goalie.player_role);
                playerCardData.position = new CardPositionCharacteristic(goalie.native_position);
                playerCardData.statistics = new CardStatistic[]
                {
                    new ReflexesStatistic((int)goalie.Stats.Reflexes),
                    new PuckControlStatistic((int)goalie.Stats.PuckControl),
                    new StrengthStatistic((int)goalie.Stats.Strength),
                };
                playerCardData.rareness =
                    new CardRarenessCharacteristic(
                        global::Utils.Utils.GetRarityWithinAverageStats((int)goalie.Stats.GetAverageStats()));
            }
            else
            {
                FieldPlayer fieldPlayer = (FieldPlayer)token;
                playerCardData.number = new CardNumberCharacteristic(fieldPlayer.number);
                playerCardData.role = new CardRoleCharacteristic(fieldPlayer.player_role);
                playerCardData.position = new CardPositionCharacteristic(fieldPlayer.native_position);
                playerCardData.statistics = new CardStatistic[]
                {
                    new SkatingStatistic((int)fieldPlayer.Stats.Skating),
                    new ShootingStatistic((int)fieldPlayer.Stats.Shooting),
                    new StickHandlingStatistic((int)fieldPlayer.Stats.StickHandling),
                    new StrengthStatistic((int)fieldPlayer.Stats.StrengthAvg),
                    new HockeyIqStatistic((int)fieldPlayer.Stats.Iq),
                    new DefenceStatistic((int)fieldPlayer.Stats.Defense)
                };
                playerCardData.rareness =
                    new CardRarenessCharacteristic(
                        global::Utils.Utils.GetRarityWithinAverageStats((int)fieldPlayer.Stats.GetAverageStats()));
            }
            
            viewPrototype = new CardViewPrototype(transform, playerCardData);
        }
    }
}
