using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Near.GameContract.ContractMethods;
using Near.Models;
using Near.Models.Game.Team;
using Near.Models.Tokens;

namespace UI.ManageTeam
{
    public class ManageTeamController
    {
        public async Task<Team> LoadUserTeam()
        {
            return await Views.LoadUserTeam();
        }

        public async Task<List<Token>> LoadUserNFTs()
        {
            var tokes = await Near.MarketplaceContract.ContractMethods.Views.GetUserNFTs();
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