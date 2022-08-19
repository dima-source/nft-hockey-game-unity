using System.Collections.Generic;
using System.Threading.Tasks;
using Near.GameContract.ContractMethods;
using Near.Models.Game.Team;
using Near.Models.Tokens;
using Near.Models.Tokens.Filters;

namespace UI.ManageTeam
{
    public class ManageTeamController
    {
        public async Task<Team> LoadUserTeam()
        {
            // return await Views.LoadUserTeam();
            return new Team();
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