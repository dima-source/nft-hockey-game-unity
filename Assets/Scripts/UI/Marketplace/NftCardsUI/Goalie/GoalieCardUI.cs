using Near.Models;
using Near.Models.Tokens;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Marketplace.NftCardsUI.Goalie
{
    public class GoalieCardUI : CardUI
    {
        [SerializeField] private Text position;
        [SerializeField] private Text number;
        [SerializeField] private Text role;
        [SerializeField] private Text gloveAndBlocker;
        [SerializeField] private Text pads;
        [SerializeField] private Text stand;
        [SerializeField] private Text stretch;
        [SerializeField] private Text morale;
        [SerializeField] private Text reflexes;
        [SerializeField] private Text puckControl;
        [SerializeField] private Text strength;

        public Text Reflexes => reflexes;
        public Text Strength => strength;
        public Text PuckControl => puckControl;
        public Text Position => position;
        public Text Number => number;
        public Text Role => role;
        public Text GloveAndBlocker => gloveAndBlocker;
        public Text Pads => pads;
        public Text Stand => stand;
        public Text Stretch => stretch;
        public Text Morale => morale;
        
        public override void SetData(Token token)
        {
            CardName.text = token.title;
            StartCoroutine(Utils.Utils.LoadImage(Image, token.media));

            Near.Models.Tokens.Players.Goalie.Goalie goalie = (Near.Models.Tokens.Players.Goalie.Goalie)token;

            Number.text = goalie.number.ToString();
            /*
            Position.text = Utils.Utils.ConvertPosition(extra.Position);
            Role.text = extra.Role;

            GloveAndBlocker.text = extra.Stats.GloveAndBlocker.ToString();
            Pads.text = extra.Stats.Pads.ToString();
            Stand.text = extra.Stats.Stand.ToString();
            Stretch.text = extra.Stats.Stretch.ToString();
            Morale.text = extra.Stats.Morale.ToString();
            */
        }
    }
}