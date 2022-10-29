using UI.Profile.Models;

namespace UI.Profile.Rewards.SpecificRewards
{
    public class AgentAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Agent award";
        protected override string Description => "Five sales";

        public override bool IsObtained(RewardsUser user)
        {
            return user.players_sold >= 5;
        }
    }
}