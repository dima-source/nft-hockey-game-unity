using Near.Models.Tokens;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Marketplace.NftCardsUI.FieldPlayer
{
    public class FieldPlayerCardUI : CardUI
    {
        [SerializeField] private Text position;
        [SerializeField] private Text number;
        [SerializeField] private Text role;
        [SerializeField] private Text stickHandling;
        [SerializeField] private Text skating;
        [SerializeField] private Text shooting;
        [SerializeField] private Text strength;
        [SerializeField] private Text iQ;
        [SerializeField] private Text morale;
        [SerializeField] private Text defence;
        
        public Text Position => position;
        public Text Defence => defence;
        public Text Number => number;
        public Text Role => role;
        public Text StickHandling => stickHandling;
        public Text Skating => skating;
        public Text Shooting => shooting;
        public Text Strength => strength;
        public Text IQ => iQ;
        public Text Morale => morale;


        public override void SetData(Token token)
        {
            CardName.text = token.title;
            StartCoroutine(Utils.Utils.LoadImage(Image, token.media));

            Near.Models.Tokens.Players.FieldPlayer.FieldPlayer fieldPlayer =
                (Near.Models.Tokens.Players.FieldPlayer.FieldPlayer)token;

            Number.text = fieldPlayer.number.ToString();
            /*
            Position.text = Utils.Utils.ConvertPosition(extra.Position);
            Role.text = extra.Role;

            Skating.text = extra.Stats.Skating.ToString();
            Shooting.text = extra.Stats.Shooting.ToString();
            Strength.text = extra.Stats.Strength.ToString();
            IQ.text = extra.Stats.IQ.ToString();
            */
            Morale.text = fieldPlayer.Stats.Morale.ToString();
        }
    }
}