using System.Collections.Generic;
using UI.Scripts;

namespace Near.Models.Tokens.Filters.ToggleFilters
{
    public interface IToggleFilter
    {
        void AddToPlayerFilter(PlayerFilter playerFilter, Toggle[] toggles);
    }
}