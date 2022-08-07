using UnityEngine;
using UnityEngine.UI;

namespace UI.Marketplace.NftCardsUI.Goalie
{
    public class GoalieDescriptionUI : NftCardDescriptionUI
    {
        [SerializeField] private Text number;
        [SerializeField] private Text position;
        [SerializeField] private Text role;
        [SerializeField] private Text gloveAndBlocker;
        [SerializeField] private Text pads;
        [SerializeField] private Text stand;
        [SerializeField] private Text stretch;
        [SerializeField] private Text morale;
        [SerializeField] private Text reflexesAvg;
        [SerializeField] private Text angles;
        [SerializeField] private Text brakeaway;
        [SerializeField] private Text fiveHole;
        [SerializeField] private Text glovesideHigh;
        [SerializeField] private Text glovesideLow;
        [SerializeField] private Text sticksideHigh;
        [SerializeField] private Text sticksideLow;
        [SerializeField] private Text puckControleAvg;
        [SerializeField] private Text passing;
        [SerializeField] private Text poise;
        [SerializeField] private Text pokeCheck;
        [SerializeField] private Text puckPlayingFrequency;
        [SerializeField] private Text reboundControl;
        [SerializeField] private Text recover;
        [SerializeField] private Text strengthAvg;
        [SerializeField] private Text aggressiveness;
        [SerializeField] private Text agility;
        [SerializeField] private Text durability;
        [SerializeField] private Text endurance;
        [SerializeField] private Text speed;
        [SerializeField] private Text vision;
        

        public Text SticksideLow => sticksideLow;
        public Text PuckControleAvg => puckControleAvg;
        public Text Passing => passing;
        public Text ReflexesAvg => reflexesAvg;
        public Text Angles => angles;
        public Text Brakeaway => brakeaway;
        public Text FiveHole => fiveHole;
        public Text GlovesideHigh => glovesideHigh;
        public Text GlovesideLow => glovesideLow;
        public Text SticksideHigh => sticksideHigh;
        public Text Vision => vision;
        public Text Speed => speed;
        public Text Endurance => endurance;
        public Text Durability => durability;
        public Text Agility => agility;
        public Text Aggressiveness => aggressiveness;
        public Text StrengthAvg => strengthAvg;
        public Text Recover => recover;
        public Text ReboundControl => reboundControl;
        public Text PuckPlayingFrequency => puckPlayingFrequency;
        public Text PokeCheck => pokeCheck;
        public Text Poise => poise;
        public Text Number => number;
        public Text Position => position;
        public Text Role => role;
        public Text GloveAndBlocker => gloveAndBlocker;
        public Text Pads => pads;
        public Text Stand => stand;
        public Text Stretch => stretch;
        public Text Morale => morale;
    }
}