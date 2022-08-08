using Near.Models.Extras;
using Near.Models.Game.Team;
using Near.Models.Team.Team;
using UnityEngine;
using UnityEngine.UI;
using Event = Near.Models.Game.Event;

namespace UI.ManageTeam.DragAndDrop
{
    public class FieldPlayerCard : UIPlayer
    {
        [SerializeField] private Text skating;
        [SerializeField] private Text shooting;
        [SerializeField] private Text strength;
        [SerializeField] private Text iq;
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
            
            FieldPlayerExtra goalieExtra =  (FieldPlayerExtra)nftMetadata.Metadata.extra.GetExtra();
            /*
            number.text = goalieExtra.Number.ToString();
            role.text = goalieExtra.Role;
            position.text = Utils.Utils.ConvertPosition(goalieExtra.Position);

            skating.text = goalieExtra.Stats.Skating.ToString();
            shooting.text = goalieExtra.Stats.Shooting.ToString();
            strength.text = goalieExtra.Stats.Strength.ToString();
            iq.text = goalieExtra.Stats.IQ.ToString();
            morale.text = goalieExtra.Stats.Morale.ToString();
            */
        }

        public void SetData(FieldPlayer data)
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
            position.text = Utils.Utils.ConvertPosition(data.position);

            skating.text = data.stats.Skating.ToString();
            shooting.text = data.stats.Shooting.ToString();
            strength.text = data.stats.Strength.ToString();
            //iq.text = data.stats.IQ.ToString();
            morale.text = data.stats.Morale.ToString();
        }
    }
}