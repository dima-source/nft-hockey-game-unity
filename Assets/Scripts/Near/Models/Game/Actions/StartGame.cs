namespace Near.Models.Game.Actions {
    public class StartGame : Action {
        public override string GetMessage(string accountId)
        {
            return $"{DefaultColor} Start game";
        }
    }
}