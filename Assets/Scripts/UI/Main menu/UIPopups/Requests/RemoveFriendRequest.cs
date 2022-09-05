using System.Threading.Tasks;

namespace UI.Main_menu.UIPopups.Requests
{
    public class RemoveFriendRequest : ISendRequest
    {
        private readonly string _accountId;

        public RemoveFriendRequest(string accountId)
        {
            _accountId = accountId;
        }
        
        public async Task SendRequest()
        {
            await Near.GameContract.ContractMethods.Actions.SendFriendRequest(_accountId, "remove_friend");
        }
    }
}