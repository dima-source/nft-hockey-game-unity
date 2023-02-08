using System.Collections.Generic;
using System.Linq;
using Near.Models.Game;
using NearClientUnity.Utilities;
using Runtime;
using TMPro;
using UI.Main_menu.UIPopups.Requests;
using UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_menu.UIPopups
{
    public class UIPopupFriends : UIPopup
    {
        [SerializeField] private TextMeshProUGUI accountIdInput;
        [SerializeField] private Transform friendsContent;
        [SerializeField] private SendRequestStrategy sendRequestStrategy;
        
        private User _user;
        private List<FriendItem> _friendItems;
        private FriendItem _friendItemPrefab;
        private Button Exit;
        
        protected override void Initialize()
        {
            Exit = UiUtils.FindChild<Button>(transform, "Exit");
        }

        private void Awake()
        {
            _friendItems = new List<FriendItem>();
            ShowFriends();
            string path = Configurations.PrefabsFolderPath + "MainMenu/Friend";
            _friendItemPrefab = Scripts.UiUtils.LoadResource<FriendItem>(path);
        }

        public async void ShowFriends()
        {
            ClearFriendsScrollView();
            _user = await Near.GameContract.ContractMethods.Views.GetUser();
            
            foreach (var friend in _user.friends)
            {
                FriendItem friendItem = Instantiate(_friendItemPrefab, friendsContent);
                
                friendItem.Init(FriendItem.ButtonAction.SendPlayRequest, 
                    FriendItem.CancelButtonAction.RemoveFriend,
                    sendRequestStrategy,
                    friend.id,
                    ShowFriends);
                
                _friendItems.Add(friendItem);
            }
        }

        public async void ShowRequestFriends()
        {
            ClearFriendsScrollView();
            _user = await Near.GameContract.ContractMethods.Views.GetUser();
            
            foreach (var friend in _user.sent_friend_requests)
            {
                FriendItem friendItem = Instantiate(_friendItemPrefab, friendsContent);
                
                friendItem.Init(FriendItem.ButtonAction.Default, 
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
            
            foreach (var friend in _user.sent_requests_play)
            {
                FriendItem friendItem = Instantiate(_friendItemPrefab, friendsContent);
                
                friendItem.Init(FriendItem.ButtonAction.Default, 
                    FriendItem.CancelButtonAction.DeclinePlayRequest,
                    sendRequestStrategy,
                    friend.to,
                    ShowRequestPlay);
                
                friendItem.bidText.text = Near.NearUtils.FormatNearAmount(UInt128.Parse(friend.deposit)).ToString(); 
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
                FriendItem friendItem = Instantiate(_friendItemPrefab, friendsContent);
                
                if (_user.friends.Count(x => x.id == _user.id) != 0)
                {
                    friendItem.Init(FriendItem.ButtonAction.SendPlayRequest, 
                        FriendItem.CancelButtonAction.RemoveFriend,
                        sendRequestStrategy,
                        user.id,
                        ShowFriends);
                }
                else
                {
                    friendItem.Init(FriendItem.ButtonAction.AddFriend,
                        FriendItem.CancelButtonAction.Default,
                        sendRequestStrategy,
                        user.id,
                        ShowFriends);
                }
                
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