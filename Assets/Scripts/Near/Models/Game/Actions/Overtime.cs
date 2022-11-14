namespace Near.Models.Game.Actions {
    public class Overtime : Action {
        public override string GetMessage(string accountId)
        {
            return $"{DefaultColor} Overtime";
        }
    }
}