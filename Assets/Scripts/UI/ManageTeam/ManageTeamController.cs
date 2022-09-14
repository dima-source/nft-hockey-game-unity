using System.Collections.Generic;
using System.Threading.Tasks;
using Near;
using Near.GameContract.ContractMethods;
using Near.Models.Game.Team;
using Near.Models.Game.TeamIds;
using Near.Models.Tokens;
using Near.Models.Tokens.Filters;

namespace UI.ManageTeam
{
    public class ManageTeamController
    {
        public async Task<TeamIds> LoadUserTeam()
        {
            return await Near.MarketplaceContract.ContractMethods.Views.GetTeam(NearPersistentManager.Instance.GetAccountId());
        }

        public async Task<List<Token>> LoadUserNFTs(PlayerFilter filter, Pagination pagination)
        {
            var tokens = await Near.MarketplaceContract.ContractMethods.Views.GetTokens(filter, pagination);
            return tokens;
        }

        public void ChangeLineups(Team team)
        {
            Actions.ChangeLineups(team);
        }
    }
}