using System.Threading.Tasks;

namespace UI.Main_menu.UIPopups.Requests
{
    public class DeclineFriendRequest : ISendRequest
    {
        private readonly string _accountId;

        public DeclineFriendRequest(string accountId)
        {
            _accountId = accountId;
        }
        
        public async Task SendRequest()
        {
            await Near.GameContract.ContractMethods.Actions.SendFriendRequest(_accountId, "decline_friend_request");
        }
    }
}