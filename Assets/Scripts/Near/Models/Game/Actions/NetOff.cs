namespace Near.Models.Game.Actions {
    public class NetOff : Action {
        public override string GetMessage(string accountId)
        {
            return $"{DefaultColor} net off";
        }
    }
}