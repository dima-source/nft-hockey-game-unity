using System.Collections.Generic;
using Runtime;
using UnityEngine;

namespace UI.GameScene
{
    public class ActionMessages : MonoBehaviour
    {
        [SerializeField] private Transform content;

        private List<ActionMessage> _messages;
        private string _accountId;

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
                    ActionMessage message = Instantiate(Game.AssetRoot.gameAsset.actionMessage, content);
                    message.SetData(action, _accountId);
                
                    _messages.Add(message);
                }
            }
        }
    }
}