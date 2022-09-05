namespace UI.Main_menu.UIPopups.Requests
{
    public class AddFriendRequest : ISendRequest
    {
        private readonly string _accountId;

        public AddFriendRequest(string accountId)
        {
            _accountId = accountId;
        }
        
        public void SendRequest()
        {
            Near.GameContract.ContractMethods.Actions
                .SendFriendRequest(_accountId, "send_friend_request");
        }
    }
}