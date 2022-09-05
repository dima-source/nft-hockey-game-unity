namespace UI.Main_menu.UIPopups.Requests
{
    public class PlayRequest : ISendRequest
    {
        private readonly string _accountId;
        private readonly string _bid;

        public PlayRequest(string accountId, string bid)
        {
            _accountId = accountId;
            _bid = bid;
        } 
        
        public void SendRequest()
        {
            Near.GameContract.ContractMethods.Actions.SendRequestPlay(_accountId, _bid);
        }
    }
}