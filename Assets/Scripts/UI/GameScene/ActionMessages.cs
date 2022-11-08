using System;
using System.Collections.Generic;
using Runtime;
using UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScene
{
    public class ActionMessages : UiComponent
    {
        private Transform _content;
        private Scrollbar _scrollbar;
        private List<ActionMessage> _messages;
        private string _accountId;

        protected override void Initialize()
        {
            _scrollbar = Scripts.Utils.FindChild<Scrollbar>(transform, "Scrollbar");
            _content = Scripts.Utils.FindChild<Transform>(transform, "Content");
        }

        private void Awake()
        {
            _messages = new List<ActionMessage>();
            _accountId = Near.NearPersistentManager.Instance.GetAccountId();
        }

        public void RenderMessages(List<Near.Models.Game.Event> events)
        {
            for (int i = _messages.Count; i < events.Count; i++)
            {
                foreach (var action in events[i].Actions)
                {
                    ActionMessage message = Instantiate(Game.AssetRoot.gameAsset.actionMessage, _content);
                    message.SetData(action, _accountId);
                
                    _messages.Add(message);
                }
            }

            _scrollbar.value = 0;
        }
    }
}