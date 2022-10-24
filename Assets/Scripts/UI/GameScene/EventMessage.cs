using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.GameScene
{
    public class EventMessage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;

        [SerializeField] private Color opponentMessageColor;
        [SerializeField] private Color userMessageColor;
        [SerializeField] private Color defaultMessageColor;
        public void SetData(Near.Models.Game.Event eventData, string accountId)
        {
            messageText.text = eventData.action;
            
        }
    }
}