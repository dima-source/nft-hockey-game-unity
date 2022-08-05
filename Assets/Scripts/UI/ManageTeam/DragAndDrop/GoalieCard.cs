using Near.Models.Extras;
using Near.Models.Game.Team;
using Near.Models.Team.Team;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ManageTeam.DragAndDrop
{
    public class GoalieCard : UIPlayer
    {
        [SerializeField] private Text gloveAndBlocker;
        [SerializeField] private Text pads;
        [SerializeField] private Text stand;
        [SerializeField] private Text stretch;
        [SerializeField] private Text morale;

        private bool _isInit = false;
        
        public override void SetData(NFTMetadata nftMetadata)
        {
            CardData = nftMetadata;
            
            playerName.text = nftMetadata.Metadata.title;

            if (!string.IsNullOrEmpty(nftMetadata.Metadata.media))
            {
                StartCoroutine(Utils.Utils.LoadImage(playerImg, nftMetadata.Metadata.media));
            }
            else
            {
                silverStroke.gameObject.SetActive(true);
            }

            GoalieExtra goalieExtra =  (GoalieExtra)nftMetadata.Metadata.extra.GetExtra();
            number.text = goalieExtra.Number.ToString();
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
            
            if (!_isInit && data.img != "")
            {
                StartCoroutine(Utils.Utils.LoadImage(playerImg, data.img));
            }

            _isInit = true;
            
            playerName.text = data.name; 
            
            number.text = data.number.ToString();
            role.text = data.role;
            position.text = Utils.Utils.ConvertPosition("G");
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