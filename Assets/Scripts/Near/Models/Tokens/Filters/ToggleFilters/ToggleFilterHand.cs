using System.Collections.Generic;
using UI.Scripts;

namespace Near.Models.Tokens.Filters.ToggleFilters
{
    public class ToggleFilterHand : IToggleFilter
    {
        public void AddToPlayerFilter(PlayerFilter playerFilter, Toggle[] toggles)
        {
            List<string> hands = new List<string>();
            
            foreach (Toggle toggle in toggles)
            {
                if (!toggle.isOn || toggle.text == "Some toggle")
                {
                    continue;
                }

                switch (toggle.text)
                {
                    case "Left":
                        hands.Add("L");
                        break;
                    case "Right":
                        hands.Add("R");
                        break;
                }
            }

            if (hands.Count != 0)
            {
                playerFilter.hand_in = hands;
            }
        }
    }
}