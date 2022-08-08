using Near.Models.Game.Team;
using Near.Models.Tokens;
using Near.Models.Tokens.Players.FieldPlayer;
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

        public override void SetData(NFT nft)
        {
            CardData = nft;

            playerName.text = nft.Name;

            if (!string.IsNullOrEmpty(nft.Media))
            {
                StartCoroutine(Utils.Utils.LoadImage(playerImg, nft.Media));
            }
            else
            {
                silverStroke.gameObject.SetActive(true);
            }

            FieldPlayer fieldPlayer = (FieldPlayer)nft;
            number.text = fieldPlayer.Number.ToString();
            //role.text = goalieExtra.Role;
            //position.text = Utils.Utils.ConvertPosition(goalieExtra.Position);

            skating.text = fieldPlayer.Stats.Skating.ToString();
            shooting.text = fieldPlayer.Stats.Shooting.ToString();
            strength.text = fieldPlayer.Stats.Strength.ToString();
            //iq.text = goalieExtra.Stats.IQ.ToString();
            morale.text = fieldPlayer.Stats.Morale.ToString();
        }

        public void SetData(FieldPlayer data)
        {
            gameObject.SetActive(true);

            if (!_isInit && data.Media != "")
            {
                StartCoroutine(Utils.Utils.LoadImage(playerImg, data.Media));
            }

            _isInit = true;

            playerName.text = data.Name;

            number.text = data.Number.ToString();
            role.text = data.PlayerRole;
            position.text = Utils.Utils.ConvertPosition(data.NativePosition);

            skating.text = data.Stats.Skating.ToString();
            shooting.text = data.Stats.Shooting.ToString();
            strength.text = data.Stats.Strength.ToString();
            // iq.text = data.stats.IQ.ToString();
            morale.text = data.Stats.Morale.ToString();
        }
    }
}