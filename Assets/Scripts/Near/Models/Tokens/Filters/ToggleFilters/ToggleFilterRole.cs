using System;
using System.Collections.Generic;
using UI.Scripts;

namespace Near.Models.Tokens.Filters.ToggleFilters
{
    public class ToggleFilterRole : IToggleFilter
    {
        public void AddToPlayerFilter(PlayerFilter playerFilter, Toggle[] toggles)
        {
            List<string> playerRoles = new List<string>();

            foreach (Toggle toggle in toggles)
            {
                if (!toggle.isOn || toggle.text == "Some toggle")
                {
                    continue;
                }

                string playerRole = TransformPlayerRole(toggle.text);
                playerRoles.Add(playerRole);
            }

            if (playerRoles.Count != 0)
            {
                playerFilter.player_role_in = playerRoles;
            }
        }

        private string TransformPlayerRole(string playerRole)
        {
            switch (playerRole)
            {
                case "Thy-harder":
                    return "TryHarder";
                case "Defensive forward":
                    return "DefensiveForward";
                case "Defensive defenseman":
                    return "DefensiveDefenseman";
                case "Offensive defenseman":
                    return "OffensiveDefenseman";
                case "Two-way":
                    return "TwoWay";
                case "Tough guy":
                    return "ToughGuy";
                case "Stand up":
                    return "StandUp";
                case "Playmaker":
                    return "Playmaker";
                case "Enforcer":
                    return "Enforcer";
                case "Shooter":
                    return "Shooter";
                case "Grinder":
                    return "Grinder";
                case "Butterfly":
                    return "Butterfly";
                case "Hybrid":
                    return "Hybrid";
                default:
                    throw new Exception("Player role not found");
            }
        }
    }
}