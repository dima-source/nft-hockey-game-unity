using System.Collections.Generic;
using UI.Scripts;

namespace Near.Models.Tokens.Filters.ToggleFilters
{
    public class SaleOptionToggleFilter : IToggleFilter
    {
        public void AddToPlayerFilter(PlayerFilter playerFilter, Toggle[] toggles)
        {
            MarketplaceTokenFilter marketplaceTokenFilter = new MarketplaceTokenFilter
            {
                isAuction_in = new List<bool>()
            };

            foreach (var toggle in toggles)
            {
                if (!toggle.isOn || toggle.text == "Some toggle")
                {
                    continue;
                }

                switch (toggle.text)
                {
                    case "Straight":
                        marketplaceTokenFilter.isAuction_in.Add(false);
                        break;
                    case "Auction":
                        marketplaceTokenFilter.isAuction_in.Add(true);
                        break;
                }
            }

            if (marketplaceTokenFilter.isAuction_in.Count == 0)
            {
                marketplaceTokenFilter.isAuction_in.AddRange(
                    new List<bool> { true, false }
                );
            }
            
            playerFilter.marketplace_data_ = marketplaceTokenFilter;
        }
    }
}