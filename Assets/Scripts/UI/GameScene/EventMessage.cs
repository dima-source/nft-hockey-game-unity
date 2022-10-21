using TMPro;
using UnityEngine;

namespace UI.GameScene
{
    public class EventMessage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;
        
        [SerializeField] private Color opponentMessageColor;
        [SerializeField] private Color messageColor;

        public void SetData(Near.Models.Game.Event eventData, string accountId)
        {
            if (eventData.user1.user.id == accountId)
            {
                
            }
        }
    }
}