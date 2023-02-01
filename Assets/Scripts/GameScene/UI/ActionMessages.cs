using System.Collections.Generic;
using Runtime;
using UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.UI
{
    public class ActionMessages : UiComponent
    {
        private Transform _content;
        private Scrollbar _scrollbar;
        private List<ActionMessage> _messages;
        private string _accountId;

        protected override void Initialize()
        {
            _scrollbar = global::UI.Scripts.Utils.FindChild<Scrollbar>(transform, "Scrollbar");
            _content = global::UI.Scripts.Utils.FindChild<Transform>(transform, "Content");
        }

        protected override void OnAwake()
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
                    string path = Configurations.PrefabsFolderPath + "Game/MessageText";
                    ActionMessage prefab = global::UI.Scripts.Utils.LoadResource<ActionMessage>(path);
                    ActionMessage message = Instantiate(prefab, _content);
                    message.SetData(action, _accountId);
                
                    _messages.Add(message);
                }
            }

            _scrollbar.value = 0;
        }
    }
}