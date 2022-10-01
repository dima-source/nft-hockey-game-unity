using System;

namespace Near.Models.Tokens.Filters.ToggleFilters
{
    public class ToggleFilterFactory
    {
        public IToggleFilter GetToggleFilter(string name)
        {
            switch (name)
            {
                case "Sale option":
                    return new SaleOptionToggleFilter();
                case "Rarity":
                    return new ToggleFilterRarity();
                case "Position":
                    return new ToggleFilterPosition();
                case "Role":
                    return new ToggleFilterRole();
                case "Nation":
                    return new ToggleFilterNation();
                case "Age":
                    return new ToggleFilterAge();
                case "Hand":
                    return new ToggleFilterHand();
            }

            throw new Exception("Filter not found");
        }
    }
}