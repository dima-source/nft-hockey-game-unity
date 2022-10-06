using System;
using System.Collections.Generic;
using UI.Scripts;

namespace Near.Models.Tokens.Filters.ToggleFilters
{
    public class ToggleFilterPosition : IToggleFilter
    {
        public void AddToPlayerFilter(PlayerFilter playerFilter, Toggle[] toggles)
        {
            List<string> playerPositions = new List<string>();
            
            foreach (Toggle toggle in toggles)
            {
                if (!toggle.isOn || toggle.text == "Some toggle")
                {
                    continue;
                }

                string positionSymbol = GetPositionSymbol(toggle.text);
                playerPositions.Add(positionSymbol);
            }

            if (playerPositions.Count != 0)
            {
                playerFilter.native_position_in = playerPositions;
            }
        }

        private string GetPositionSymbol(string position)
        {
            switch (position)
            {
                case "Left winger":
                    return "LW";
                case "Right winger":
                    return "RW";
                case "Center":
                    return "C";
                case "Left defenseman":
                    return "LD";
                case "Right defenseman":
                    return "RD";
                case "Goalie":
                    return "G";
                default:
                    throw new Exception("Position not found");
            }
        }
    }
}