using System;
using Near.Models.Game.Team;
using Near.Models.Tokens;
using Near.Models.Tokens.Players.FieldPlayer;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Event = Near.Models.Game.Event;

namespace UI.ManageTeam.DragAndDrop
{
    public class FieldPlayerCard : UIPlayer
    {
        // [SerializeField] private Text skating;
        // [SerializeField] private Text shooting;
        // [SerializeField] private Text strength;
        // [SerializeField] private Text iq;
        // [SerializeField] private Text morale;

        private bool _isInit = false;

        protected override void Initialize()
        {
            base.Initialize();
            updateAvatar = false;
        }

        public override void SetData(Token token)
        {
            CardData = token;
            
            setPlayerName(token.title);

            if (!string.IsNullOrEmpty(token.media))
            {
                try
                {
                    StartCoroutine(ImageLoader.LoadImage(_avatar, token.media));
                } catch (ApplicationException) {}
            }
            // else
            // {
            //     silverStroke.gameObject.SetActive(true);
            // }

            FieldPlayer fieldPlayer = (FieldPlayer)token;
            playerNumber = fieldPlayer.number;
            playerRole = StringToRole(fieldPlayer.player_role);
            position = StringToPosition(fieldPlayer.native_position);
            //role.text = goalieExtra.Role;
            //position.text = Utils.Utils.ConvertPosition(goalieExtra.Position);

            // skating.text = ;
            // shooting.text = ;
            // strength.text = ;
            //iq.text = goalieExtra.Stats.IQ.ToString();
            // morale.text = ;
            statistics = new[]
            {
                int.Parse(fieldPlayer.Stats.Skating.ToString()),
                int.Parse(fieldPlayer.Stats.Shooting.ToString()),
                int.Parse(fieldPlayer.Stats.Strength.ToString()),
                int.Parse(fieldPlayer.Stats.Morale.ToString())
            };
        }

        // public void SetData(FieldPlayer data)
        // {
        //     gameObject.SetActive(true);
        //
        //     if (!_isInit && data.media != "")
        //     {
        //         StartCoroutine(Utils.Utils.LoadImage(playerImg, data.media));
        //     }
        //
        //     _isInit = true;
        //
        //     setPlayerName(data.title);
        //
        //     playerNumber = data.number;
        //     playerRole = StringToRole(data.player_role);
        //     position = StringToPosition(data.native_position);
        //
        //
        //     // skating.text = data.Stats.Skating.ToString();
        //     // shooting.text = data.Stats.Shooting.ToString();
        //     // strength.text = data.Stats.Strength.ToString();
        //     // iq.text = data.stats.IQ.ToString();
        //     // morale.text = data.Stats.Morale.ToString();
        //     statistics = new[]
        //     {
        //         int.Parse(data.Stats.Skating.ToString()),
        //         int.Parse(data.Stats.Shooting.ToString()),
        //         int.Parse(data.Stats.Strength.ToString()),
        //         int.Parse(data.Stats.Morale.ToString())
        //     };
        // }
    }
}