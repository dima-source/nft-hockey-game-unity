using System.Collections.Generic;
using UI.Scripts;

namespace Near.Models.Tokens.Filters.ToggleFilters
{
    public class ToggleFilterRarity : IToggleFilter
    {
        public void AddToPlayerFilter(PlayerFilter playerFilter, Toggle[] toggles)
        {
            List<string> rarities = new List<string>();

            foreach (Toggle toggle in toggles)
            {
                if (!toggle.isOn || toggle.text == "Some toggle")
                {
                    continue;
                }
                
                rarities.Add(toggle.text);
            }

            if (rarities.Count != 0)
            {
                playerFilter.rarity_in = rarities;
            }
        }
    }
}