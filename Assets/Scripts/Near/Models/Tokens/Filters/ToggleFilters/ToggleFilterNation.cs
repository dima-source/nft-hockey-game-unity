using System;
using System.Collections.Generic;
using UI.Scripts;

namespace Near.Models.Tokens.Filters.ToggleFilters
{
    public class ToggleFilterNation : IToggleFilter
    {
        public void AddToPlayerFilter(PlayerFilter playerFilter, Toggle[] toggles)
        {
            List<string> nationalities = new List<string>();
            
            foreach (Toggle toggle in toggles)
            {
                if (!toggle.isOn || toggle.text == "Some toggle")
                {
                    continue;
                }

                string countrySymbol = GetCountrySymbol(toggle.text);
                nationalities.Add(countrySymbol);    
            }

            if (nationalities.Count != 0)
            {
                playerFilter.nationality_in = nationalities;
            }
        }

        private string GetCountrySymbol(string countryName)
        {
            switch (countryName)
            {
                case "Canada":
                    return "CA";
                case "America":
                    return "US";
                case "Finland":
                    return "FI";
                case "Czech":
                    return "CZ";
                case "Sweden":
                    return "SE";
                case "Germany":
                    return "DE";
                case "Ukraine":
                    return "UA";
                default:
                    throw new Exception("Country not found");
            }
        }
    }
}