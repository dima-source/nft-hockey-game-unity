namespace Near.Models.Game.Actions {
    public class PuckOut : Action {
        public override string GetMessage(string accountId)
        {
            return $"{DefaultColor} puck out";
        }
    }
}