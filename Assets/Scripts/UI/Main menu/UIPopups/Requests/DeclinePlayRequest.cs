namespace UI.Main_menu.UIPopups.Requests
{
    public class DeclinePlayRequest : ISendRequest
    {
        private readonly string _accountId;
        private readonly string _bid;

        public DeclinePlayRequest(string accountId, string bid)
        {
            _accountId = accountId;
            _bid = bid;
        }

        public void SendRequest()
        {
            Near.GameContract.ContractMethods.Actions.DeclineRequestPlay(_accountId, _bid);
        }
    }
}