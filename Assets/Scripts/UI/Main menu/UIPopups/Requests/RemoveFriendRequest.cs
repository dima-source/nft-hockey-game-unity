namespace UI.Main_menu.UIPopups.Requests
{
    public class RemoveFriendRequest : ISendRequest
    {
        private readonly string _accountId;

        public RemoveFriendRequest(string accountId)
        {
            _accountId = accountId;
        }
        
        public void SendRequest()
        {
            Near.GameContract.ContractMethods.Actions.SendFriendRequest(_accountId, "remove_friend");
        }
    }
}