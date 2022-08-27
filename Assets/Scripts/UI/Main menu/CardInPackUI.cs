using System;
using Near.Models.Tokens;
using Near.Models.Tokens.Players.FieldPlayer;
using Near.Models.Tokens.Players.Goalie;
using UI.Scripts;
using UnityEngine;
using Utils;

namespace UI.Main_menu
{
    public class CardInPackUI : CardView
    {
        public void SetData(Token token)
        {
            SetPlayerName(token.title);

            if (!string.IsNullOrEmpty(token.media))
            {
                try
                {
                    StartCoroutine(ImageLoader.LoadImage(_avatar, token.media));
                } catch (ApplicationException) {}
            }
            
            if (token.player_type == "Goalie")
            {
                Goalie goalie = (Goalie)token;
                playerNumber = goalie.number;
                playerRole = StringToRole(goalie.player_role);
                position = StringToPosition(goalie.native_position);
                statistics = new[]
                {
                    int.Parse(goalie.Stats.Reflexes.ToString()),
                    int.Parse(goalie.Stats.PuckControl.ToString()),
                    int.Parse(goalie.Stats.Strength.ToString()),
                };
                
            }
            else
            {
                FieldPlayer fieldPlayer = (FieldPlayer)token;
                playerNumber = fieldPlayer.number;
                playerRole = StringToRole(fieldPlayer.player_role);
                position = StringToPosition(fieldPlayer.native_position);
                statistics = new[]
                {
                    int.Parse(fieldPlayer.Stats.Skating.ToString()),
                    int.Parse(fieldPlayer.Stats.Shooting.ToString()),
                    int.Parse(fieldPlayer.Stats.Strength.ToString()),
                    int.Parse(fieldPlayer.Stats.Morale.ToString())
                };
            }
        }
        
        private void SetPlayerName(string name)
        {
            string[] splittedName = name.Split(" ", 2);
            if (splittedName.Length != 2)
            {
                throw new ApplicationException("Name is incorrect. Must be \"Name Surname\", got \"" + name + "\"");
            }
            playerName = splittedName[0];
            playerSurname = splittedName[1];
        }
        
        private static Position StringToPosition(string position)
        {
            Position.TryParse(position, out Position parsedPosition);
            return parsedPosition;
        }

        private static PlayerRole StringToRole(string roleString)
        {
            PlayerRole.TryParse(roleString, out PlayerRole parsedRole);
            return parsedRole;
        }
    }
}