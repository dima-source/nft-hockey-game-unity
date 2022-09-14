using System;
using UnityEngine;

namespace UI.Main_menu.UIPopups.Requests
{
    public class SendRequestStrategy : MonoBehaviour
    {
        [SerializeField] private Loading loading;
        [SerializeField] private UIPopupError uiPopupError;
        
        public async void SendRequest(ISendRequest request)
        {
            gameObject.SetActive(true);
            loading.gameObject.SetActive(true);
            
            try
            {
                await request.SendRequest();
            }
            catch(Exception e)
            {
                uiPopupError.SetTitle(e.Message.Contains("NotEnoughBalance")
                    ? "Not enough balance"
                    : "Something went wrong");
                loading.gameObject.SetActive(false);
                uiPopupError.Show();
                return;
            }
            
            gameObject.SetActive(false);
        }
    }
}