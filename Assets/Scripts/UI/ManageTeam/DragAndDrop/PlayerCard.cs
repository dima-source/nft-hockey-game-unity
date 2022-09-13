using System;
using Near.Models.Game.Team;
using Near.Models.Tokens;
using Near.Models.Tokens.Players.FieldPlayer;
using Near.Models.Tokens.Players.Goalie;
using UI.Scripts.Card;
using UI.Scripts.Card.CardStatistics;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Event = Near.Models.Game.Event;

namespace UI.ManageTeam.DragAndDrop
{
    public class PlayerCard : DraggableCard
    {
        
        public override void SetData(Token token)
        {
            CardData = token;
            
            playerCardData.name = token.title;

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

            
            if (token.player_type == "Goalie")
            {
                Goalie goalie = (Goalie)token;
                playerCardData.number = new CardNumberCharacteristic(goalie.number);
                playerCardData.role = new CardRoleCharacteristic(goalie.player_role);
                playerCardData.position = new CardPositionCharacteristic(goalie.native_position);
                playerCardData.statistics = new CardStatistic[]
                {
                    new HockeyIqStatistic((int)goalie.Stats.Reflexes),
                    new StickHandlingStatistic((int)goalie.Stats.PuckControl),
                    new StrengthStatistic((int)goalie.Stats.Strength),
                };
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
                    new StrengthStatistic((int)fieldPlayer.Stats.Strength),
                    new HockeyIqStatistic((int)fieldPlayer.Stats.Morale)
                };
            }
        }
    }
}