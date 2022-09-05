namespace UI.Main_menu.UIPopups.Requests
{
    public class AcceptPlayRequest : ISendRequest
    {
        private string _accountId;
        private string _bid;

        public AcceptPlayRequest(string accountId, string bid)
        {
            _accountId = accountId;
            _bid = bid;
        }
        public void SendRequest()
        {
            Near.GameContract.ContractMethods.Actions.AcceptRequestPlay(_accountId, _bid);
        }
    }
}