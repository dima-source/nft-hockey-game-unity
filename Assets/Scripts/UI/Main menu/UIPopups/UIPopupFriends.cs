using System.Collections.Generic;
using Near.Models.Game;
using Runtime;
using TMPro;
using UnityEngine;

namespace UI.Main_menu.UIPopups
{
    public class UIPopupFriends : UIPopup
    {
        [SerializeField] private TextMeshProUGUI accountIdInput;
        [SerializeField] private Transform friendsContent;
        
        private User _user;
        private List<FriendItem> _friendItems;
        
        private async void Awake()
        {
            _friendItems = new List<FriendItem>();
            _user = await Near.GameContract.ContractMethods.Views.GetUser();
            ShowFriends();
        }

        private void ShowFriends()
        {
            ClearFriendsScrollView();
            
            foreach (var friend in _user.friends)
            {
                FriendItem friendItem = Instantiate(Game.AssetRoot.mainMenuAsset.friendItem, friendsContent);
                friendItem.AccountId.text = friend.id;
                
                _friendItems.Add(friendItem);
            }
        }

        private async void FindFriend()
        {
            ClearFriendsScrollView();
            
            var filter = new UserFilter()
            {
                id = accountIdInput.text
            };
            
            var users = await Near.GameContract.ContractMethods.Views.GetUsers(filter); 
            
            foreach (var user in users)
            {
                FriendItem friendItem = Instantiate(Game.AssetRoot.mainMenuAsset.friendItem, friendsContent);
                friendItem.AccountId.text = user.id;
                
                _friendItems.Add(friendItem);
            } 
        }

        private void ClearFriendsScrollView()
        {
            foreach (var friend in _friendItems)
            {
                Destroy(friend.gameObject);
            } 
        }
    }
}