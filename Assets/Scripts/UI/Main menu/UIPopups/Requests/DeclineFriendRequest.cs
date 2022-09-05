namespace UI.Main_menu.UIPopups.Requests
{
    public class DeclineFriendRequest : ISendRequest
    {
        private readonly string _accountId;

        public DeclineFriendRequest(string accountId)
        {
            _accountId = accountId;
        }
        
        public void SendRequest()
        {
            Near.GameContract.ContractMethods.Actions.SendFriendRequest(_accountId, "decline_friend_request");
        }
    }
}