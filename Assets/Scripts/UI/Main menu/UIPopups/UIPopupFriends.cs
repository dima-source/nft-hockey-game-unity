using System.Collections.Generic;
using Near.Models.Game;
using Runtime;
using TMPro;
using UI.Main_menu.UIPopups.Requests;
using UnityEngine;

namespace UI.Main_menu.UIPopups
{
    public class UIPopupFriends : UIPopup
    {
        [SerializeField] private TextMeshProUGUI accountIdInput;
        [SerializeField] private Transform friendsContent;
        [SerializeField] private SendRequestStrategy sendRequestStrategy;
        
        private User _user;
        private List<FriendItem> _friendItems;
        
        private void Awake()
        {
            _friendItems = new List<FriendItem>();
            ShowFriends();
        }

        public async void ShowFriends()
        {
            ClearFriendsScrollView();
            _user = await Near.GameContract.ContractMethods.Views.GetUser();
            
            foreach (var friend in _user.friends)
            {
                FriendItem friendItem = Instantiate(Game.AssetRoot.mainMenuAsset.friendItem, friendsContent);
                
                friendItem.Init(FriendItem.ButtonAction.SendPlayRequest, 
                    FriendItem.CancelButtonAction.RemoveFriend,
                    sendRequestStrategy,
                    friend.id);
                
                _friendItems.Add(friendItem);
            }
        }

        public async void ShowRequestFriends()
        {
            ClearFriendsScrollView();
            _user = await Near.GameContract.ContractMethods.Views.GetUser();
            
            foreach (var friend in _user.sent_friend_requests)
            {
                FriendItem friendItem = Instantiate(Game.AssetRoot.mainMenuAsset.friendItem, friendsContent);
                
                friendItem.Init(FriendItem.ButtonAction.Default, 
                    FriendItem.CancelButtonAction.DeclineFriendRequest,
                    sendRequestStrategy,
                    friend.id);
                
                _friendItems.Add(friendItem);
            }
        }
        
        public async void ShowRequestPlay()
        {
            ClearFriendsScrollView();
            _user = await Near.GameContract.ContractMethods.Views.GetUser();
            
            foreach (var friend in _user.sent_requests_play)
            {
                FriendItem friendItem = Instantiate(Game.AssetRoot.mainMenuAsset.friendItem, friendsContent);
                
                friendItem.Init(FriendItem.ButtonAction.Default, 
                    FriendItem.CancelButtonAction.DeclinePlayRequest,
                    sendRequestStrategy,
                    friend.id);
                
                friendItem.Bid.text = friend.deposit;
                _friendItems.Add(friendItem);
            }
        }
        
        public async void FindFriend()
        {
            ClearFriendsScrollView();
            
            var filter = new UserFilter()
            {
                id = accountIdInput.text.Split('.')[0] + ".testnet"
            };
            var users = await Near.GameContract.ContractMethods.Views.GetUsers(filter); 
            
            foreach (var user in users)
            {
                FriendItem friendItem = Instantiate(Game.AssetRoot.mainMenuAsset.friendItem, friendsContent);
                friendItem.Init(FriendItem.ButtonAction.AddFriend,
                    FriendItem.CancelButtonAction.Default,
                    sendRequestStrategy,
                    user.id);
                
                _friendItems.Add(friendItem);
            } 
        }

        private void ClearFriendsScrollView()
        {
            foreach (var friend in _friendItems)
            {
                Destroy(friend.gameObject);
            }

            _friendItems = new List<FriendItem>();
        }
    }
}