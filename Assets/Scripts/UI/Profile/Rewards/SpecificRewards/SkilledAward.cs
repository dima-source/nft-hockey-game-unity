using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class SkilledAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Skilled award";
        protected override string Description => "Five victories";
        
        public override bool IsObtained(RewardsUser user)
        {
            return user.wins >= 5;
        }
    }
}