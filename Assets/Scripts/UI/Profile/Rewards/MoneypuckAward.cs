using UI.Profile.Models;

namespace UI.Profile.Rewards
{
    public class MoneypuckAward: BaseReward
    {
        protected override string SpriteName => "Shield";
        protected override string Title => "Moneypuck award";
        protected override string Description => "Twenty sales";

        public override bool IsObtained(RewardsUser user)
        {
            return user.players_sold >= 20;
        }
    }
}