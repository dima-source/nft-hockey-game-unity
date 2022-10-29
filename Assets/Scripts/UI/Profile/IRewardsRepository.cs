using System.Collections.Generic;
using System.Threading.Tasks;
using UI.Profile.Models;
using UI.Profile.Rewards;

namespace UI.Profile
{
    public interface IRewardsRepository
    {
        public Task<RewardsUser> GetUser();

        public Task<List<BaseReward>> GetRewards();
    }
}