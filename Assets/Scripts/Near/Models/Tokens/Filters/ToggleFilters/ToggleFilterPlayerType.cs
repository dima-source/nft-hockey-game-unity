using System;
using System.Collections.Generic;
using UI.Scripts;

namespace Near.Models.Tokens.Filters.ToggleFilters
{
    public class ToggleFilterPlayerType : IToggleFilter
    {
        public void AddToPlayerFilter(PlayerFilter playerFilter, Toggle[] toggles)
        {
            List<string> types= new List<string>();

            foreach (Toggle toggle in toggles)
            {
                if (!toggle.isOn || toggle.text == "Some toggle")
                {
                    continue;
                }

                string playerType = TransformPlayerType(toggle.text);
                
                types.Add(playerType);
            }

            if (types.Count != 0)
            {
                playerFilter.player_type_in = types;
            }
        }

        private string TransformPlayerType(string type)
        {
            switch (type)
            {
                case "Field player":
                    return "FieldPlayer";
                case "Goalie":
                    return "Goalie";
                default:
                    throw new Exception("Player type not found");
            }
        }
    }
}