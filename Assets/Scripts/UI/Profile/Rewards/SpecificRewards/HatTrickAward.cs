using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class HatTrickAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Hat trick award";
        protected override string Description => "3 victories in a row";

        public override bool IsObtained(RewardsUser user)
        {
            return user.max_wins_in_line >= 3;
        }
    }
}