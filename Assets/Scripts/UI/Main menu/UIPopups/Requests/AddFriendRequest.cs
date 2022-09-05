using System.Threading.Tasks;

namespace UI.Main_menu.UIPopups.Requests
{
    public class AddFriendRequest : ISendRequest
    {
        private readonly string _accountId;

        public AddFriendRequest(string accountId)
        {
            _accountId = accountId;
        }

        public async Task SendRequest()
        {
            await Near.GameContract.ContractMethods.Actions
                .SendFriendRequest(_accountId, "send_friend_request");
        }
    }
}