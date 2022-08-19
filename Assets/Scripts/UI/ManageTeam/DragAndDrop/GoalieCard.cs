using Near.Models.Game.Team;
using Near.Models.Tokens;
using Near.Models.Tokens.Players.Goalie;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ManageTeam.DragAndDrop
{
    public class GoalieCard : UIPlayer
    {
        // [SerializeField] private Text gloveAndBlocker;
        // [SerializeField] private Text pads;
        // [SerializeField] private Text stand;
        // [SerializeField] private Text stretch;
        // [SerializeField] private Text morale;

        private bool _isInit = false;
        
        public override void SetData(Token token)
        {
            CardData = token;

            setPlayerName(token.title);

            if (!string.IsNullOrEmpty(token.media))
            {
                StartCoroutine(Utils.Utils.LoadImage(playerImg, token.media));
            }
            // else
            // {
            //     silverStroke.gameObject.SetActive(true);
            // }

            Goalie goalie =  (Goalie)token;
            // number.text = goalie.number.ToString();
            playerNumber = goalie.number;
            //role.text = goalieExtra.Role;
            //position.text = Utils.Utils.ConvertPosition(goalieExtra.Position);

            //gloveAndBlocker.text = goalieExtra.Stats.GloveAndBlocker.ToString();
            /*
            pads.text = goalieExtra.Stats.Pads.ToString();
            stand.text = goalieExtra.Stats.Stand.ToString();
            stretch.text = goalieExtra.Stats.Stretch.ToString();
            morale.text = goalieExtra.Stats.Morale.ToString();
            */
        }

        public void SetData(Goalie data)
        {
            gameObject.SetActive(true);
            
            if (!_isInit && data.media != "")
            {
                StartCoroutine(Utils.Utils.LoadImage(playerImg, data.media));
            }

            _isInit = true;
            
            setPlayerName(data.title);
            
            playerNumber = data.number;
            playerRole = StringToRole(data.player_role);
            position = Position.G;
/*
            gloveAndBlocker.text = data.stats.GloveAndBlocker.ToString();
            pads.text = data.stats.Pads.ToString();
            stand.text = data.stats.Stand.ToString();
            stretch.text = data.stats.Stretch.ToString();
            morale.text = data.stats.Morale.ToString();
            */
        }
    }
}