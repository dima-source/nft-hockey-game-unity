using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class TraderAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Trader award";
        protected override string Description => "Ten sales";

        public override bool IsObtained(RewardsUser user)
        {
            return user.players_sold >= 10;
        }
    }
}