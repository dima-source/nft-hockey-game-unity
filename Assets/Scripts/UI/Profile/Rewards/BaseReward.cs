using UI.Profile.Models;

namespace UI.Profile.Rewards
{
    public abstract class BaseReward
    {
        protected abstract string SpriteName { get; }
        protected abstract string Title { get; }
        protected abstract string Description { get; }
        
        public abstract bool IsObtained(RewardsUser user);

        public void SetForView(RewardView view, RewardsUser user)
        {
            view.SetData(SpriteName, Title, Description, IsObtained(user));
        }
    }
}