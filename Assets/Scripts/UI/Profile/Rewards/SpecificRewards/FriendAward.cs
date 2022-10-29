using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class FriendAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Friend award";
        protected override string Description => "Got fifty friends";

        public override bool IsObtained(RewardsUser user)
        {
            return user.friends_count >= 50;
        }
    }
}