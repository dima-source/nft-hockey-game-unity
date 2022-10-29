using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class VeteranAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Veteran award";
        protected override string Description => "Fifty victories";

        public override bool IsObtained(RewardsUser user)
        {
            return user.wins >= 50;
        } 
    }
}