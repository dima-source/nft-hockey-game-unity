using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Near.GameContract.ContractMethods;
using Near.Models;
using Near.Models.Game.Team;
using Near.Models.Tokens;
using Near.Models.Tokens.Filters;

namespace UI.ManageTeam
{
    public class ManageTeamController
    {
        public async Task<Team> LoadUserTeam()
        {
            return await Views.LoadUserTeam();
        }

        public async Task<List<Token>> LoadUserNFTs(PlayerFilter filter, Pagination pagination)
        {
            var tokes = await Near.MarketplaceContract.ContractMethods.Views.GetTokens(filter, pagination);
            if (tokes == null)
            {
                return new List<Token>();
            }

            return tokes;
        }

        public void ChangeLineups(Team team)
        {
            Actions.ChangeLineups(team);
        }
    }
}