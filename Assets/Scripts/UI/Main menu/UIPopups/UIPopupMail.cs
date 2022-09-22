using System.Collections.Generic;
using Near.Models.Game;
using NearClientUnity.Utilities;
using Runtime;
using TMPro;
using UI.Main_menu.UIPopups.Requests;
using UnityEngine;

namespace UI.Main_menu.UIPopups
{
    public class UIPopupMail : UIPopup
    {
        [SerializeField] private TextMeshProUGUI accountIdInput;
        [SerializeField] private Transform friendsContent;
        [SerializeField] private SendRequestStrategy sendRequestStrategy;
        
        private User _user;
        private List<FriendItem> _friendItems;
        
        private void Awake()
        {
            _friendItems = new List<FriendItem>();
            ShowRequestFriends();
        }
        
        public async void ShowRequestFriends()
        {
            ClearFriendsScrollView();
            _user = await Near.GameContract.ContractMethods.Views.GetUser();
            
            foreach (var friend in _user.friend_requests_received)
            {
                FriendItem friendItem = Instantiate(Game.AssetRoot.mainMenuAsset.friendItem, friendsContent);
                
                friendItem.Init(FriendItem.ButtonAction.AcceptFriendRequest, 
                    FriendItem.CancelButtonAction.DeclineFriendRequest,
                    sendRequestStrategy,
                    friend.id,
                    ShowRequestFriends);
                
                _friendItems.Add(friendItem);
            }
        }
       
        public async void ShowRequestPlay()
        {
            ClearFriendsScrollView();
            _user = await Near.GameContract.ContractMethods.Views.GetUser();
            
            foreach (var friend in _user.requests_play_received)
            {
                FriendItem friendItem = Instantiate(Game.AssetRoot.mainMenuAsset.friendItem, friendsContent);
                
                friendItem.Init(FriendItem.ButtonAction.AcceptPlayRequest, 
                    FriendItem.CancelButtonAction.DeclinePlayRequest,
                    sendRequestStrategy,
                    friend.from,
                    ShowRequestPlay);
                
                friendItem.bidText.text = Near.NearUtils.FormatNearAmount(UInt128.Parse(friend.deposit)).ToString(); 
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