﻿using System;
using TMPro;
using UI.Main_menu.UIPopups.Requests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Main_menu.UIPopups
{
    public class FriendItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _friendId;

        public enum ButtonAction
        {
            SendPlayRequest,
            AddFriend,
            AcceptFriendRequest,
            AcceptPlayRequest,
            Default
        }

        public enum CancelButtonAction
        {
            DeclinePlayRequest,
            DeclineFriendRequest,
            RemoveFriend,
            Default
        }

        public Button button;
        [SerializeField] private TextMeshProUGUI buttonText;

        public Button cancelButton;

        public TMP_InputField BidInput;
        public TextMeshProUGUI bidText;
        
        private SendRequestStrategy _sendRequestsStrategy;

        public void Init(ButtonAction buttonAction,
            CancelButtonAction cancelButtonAction,
            SendRequestStrategy sendRequestStrategy,
            string friendId,
            UnityAction call)
        {
            _sendRequestsStrategy = sendRequestStrategy;
            _friendId.text = friendId;

            SetAcceptButtonAction(buttonAction);
            SetCancelButtonAction(cancelButtonAction);
            
            cancelButton.onClick.AddListener(call);
            button.onClick.AddListener(call);
        }

        private void SetAcceptButtonAction(ButtonAction buttonAction)
        {
            switch (buttonAction)
            {
                case ButtonAction.SendPlayRequest:
                    buttonText.text = "Play";
                    BidInput.gameObject.SetActive(true);
                    button.onClick.AddListener(SendPlayRequest);
                    break;
                case ButtonAction.AddFriend:
                    buttonText.text = "Add";
                    button.onClick.AddListener(AddFriend);
                    break;
                case ButtonAction.AcceptPlayRequest:
                    buttonText.text = "Play";
                    bidText.gameObject.SetActive(true);
                    button.onClick.AddListener(AcceptPlayRequest);
                    break;
                case ButtonAction.AcceptFriendRequest:
                    buttonText.text = "Accept";
                    button.onClick.AddListener(AcceptFriendRequest);
                    break;
                case ButtonAction.Default:
                    button.gameObject.SetActive(false);
                    break;
                default:
                    throw new Exception("Accept button action not found");
            }
        }

        private void SendPlayRequest()
        {
            if (BidInput.text == "")
            {
                BidInput.placeholder.color = Color.red;
                return;
            }

            var sendPlayRequest = new PlayRequest(_friendId.text, BidInput.text);
            _sendRequestsStrategy.SendRequest(sendPlayRequest);
        }

        private void AddFriend()
        {
            var sendAddFriendRequest = new AddFriendRequest(_friendId.text);
            _sendRequestsStrategy.SendRequest(sendAddFriendRequest);
        }

        private void AcceptPlayRequest()
        {
            var sendAcceptPlayRequest = new AcceptPlayRequest(_friendId.text, bidText.text);
            _sendRequestsStrategy.SendRequest(sendAcceptPlayRequest);

            // TODO: start game
        }

        private void AcceptFriendRequest()
        {
            var sendAcceptFriendRequest = new AcceptFriendRequest(_friendId.text);
            _sendRequestsStrategy.SendRequest(sendAcceptFriendRequest);
        }

        private void SetCancelButtonAction(CancelButtonAction cancelButtonAction)
        {
            switch (cancelButtonAction)
            {
                case CancelButtonAction.RemoveFriend:
                    cancelButton.onClick.AddListener(RemoveFriend);
                    break;
                case CancelButtonAction.DeclineFriendRequest:
                    cancelButton.onClick.AddListener(DeclineFriendRequest);
                    break;
                case CancelButtonAction.DeclinePlayRequest:
                    bidText.gameObject.SetActive(true);
                    cancelButton.onClick.AddListener(DeclinePlayRequest);
                    break;
                case CancelButtonAction.Default:
                    cancelButton.gameObject.SetActive(false);
                    break;
                default:
                    throw new Exception("Cancel button action not found");
            }
        }

        private void RemoveFriend()
        {
            var removeFriendRequest = new RemoveFriendRequest(_friendId.text);
            _sendRequestsStrategy.SendRequest(removeFriendRequest);
        }

        private void DeclineFriendRequest()
        {
            var declineFriendRequest = new DeclineFriendRequest(_friendId.text);
            _sendRequestsStrategy.SendRequest(declineFriendRequest);
        }

        private void DeclinePlayRequest()
        {
            var declinePlayRequest = new DeclinePlayRequest(_friendId.text, bidText.text);
            _sendRequestsStrategy.SendRequest(declinePlayRequest);
        }
    }
}