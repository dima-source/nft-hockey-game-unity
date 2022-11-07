using Near.Models.Game.Actions;
using TMPro;
using UnityEngine;

namespace UI.GameScene
{
    public class ActionMessage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;

        public void SetData(Action action, string accountId)
        {
            messageText.text = action.GetMessage(accountId);
        }
    }
}