using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class PentaTrickAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Penta trick award";
        protected override string Description => "5 victories in a row";

        public override bool IsObtained(RewardsUser user)
        {
            return user.max_wins_in_line >= 5;
        }
    }
}