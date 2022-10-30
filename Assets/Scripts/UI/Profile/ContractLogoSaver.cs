using System.Threading.Tasks;
using Near.GameContract.ContractMethods;
using UI.Profile.Models;

namespace UI.Profile
{
    public class ContractLogoSaver: ILogoSaver
    {
        public Task<bool> SaveLogo(string logoString)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> SaveLogo(TeamLogo logo)
        {
            return await Actions.SetTeamLogo(logo);
        }
    }
}