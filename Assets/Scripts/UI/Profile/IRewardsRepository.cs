using System.Threading.Tasks;
using UI.Profile.Models;

namespace UI.Profile
{
    public interface IRewardsRepository
    {
        public Task<RewardsUser> GetUser();
    }
}