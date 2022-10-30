namespace UI.Profile.Rewards
{
    public interface IRewardDataReceiver
    {
        public void SetData(string spriteName, string title, string description, bool obtained);
    }
}