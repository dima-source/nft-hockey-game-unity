﻿using System.Threading.Tasks;

namespace UI.Main_menu.UIPopups.Requests
{
    public class AcceptFriendRequest : ISendRequest
    {
        private readonly string _accountId;

        public AcceptFriendRequest(string accountId)
        {
            _accountId = accountId;
        }
        public async Task SendRequest()
        {
            await Near.GameContract.ContractMethods.Actions.SendFriendRequest(_accountId, "accept_friend_request");
        }
    }
}