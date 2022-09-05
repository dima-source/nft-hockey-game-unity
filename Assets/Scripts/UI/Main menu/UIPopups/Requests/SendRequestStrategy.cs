using System;
using UnityEngine;

namespace UI.Main_menu.UIPopups.Requests
{
    public class SendRequestStrategy : MonoBehaviour
    {
        [SerializeField] private Loading loading;
        [SerializeField] private UIPopupError uiPopupError;
        
        public void SendRequest(ISendRequest request)
        {
            loading.gameObject.SetActive(true);
            
            try
            {
                request.SendRequest();
            }
            catch(Exception e)
            {
                uiPopupError.SetTitle(e.Message.Contains("NotEnoughBalance")
                    ? "Not enough balance"
                    : "Something went wrong");
                uiPopupError.Show();
            }
            
            loading.gameObject.SetActive(false);
        }
    }
}