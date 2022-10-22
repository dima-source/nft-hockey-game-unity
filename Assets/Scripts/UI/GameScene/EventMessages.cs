using System.Collections.Generic;
using Runtime;
using UnityEngine;

namespace UI.GameScene
{
    public class EventMessages : MonoBehaviour
    {
        [SerializeField] private Transform content;

        private List<EventMessage> _messages;
        private string _accountId;
        private bool flag;

        private void Awake()
        {
            _messages = new List<EventMessage>();
            _accountId = Near.NearPersistentManager.Instance.GetAccountId();
        }

        public void RenderMessages(List<Near.Models.Game.Event> events)
        {
            for (int i = _messages.Count; i < events.Count; i++)
            {
                if (flag)
                {
                    Debug.Log("Number of events: " + i);
                }
                
                EventMessage message = Instantiate(Game.AssetRoot.gameAsset.eventMessage, content);
                message.SetData(events[i], _accountId);
                
                _messages.Add(message);
            }

            flag = true;
        }
    }
}