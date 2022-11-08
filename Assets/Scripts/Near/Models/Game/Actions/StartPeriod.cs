namespace Near.Models.Game.Actions
{
    public class StartPeriod : Action
    {
        public string number { get; set; }
        
        public override string GetMessage(string accountId)
        {
            return $"{DefaultColor}{number} period";
        }
    }
}