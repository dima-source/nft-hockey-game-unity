using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class GoatAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "GOAT award";
        protected override string Description => "10 victories in a row";

        public override bool IsObtained(RewardsUser user)
        {
            return user.max_wins_in_line >= 10;
        }
    }
}