using Near;
using UI.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Profile.Popups
{
    public class LoghoutPopup : MonoBehaviour
    {
        public Button _confirm;
        public Button _cancel;
        public Button _goBack;
       
        public void Close()
        {
            gameObject.SetActive(false);
        }
        public void SignOut()
        {
            NearPersistentManager.Instance.SignOut();
            gameObject.SetActive(false);
            SceneManager.LoadScene("MainMenu");
        }
    }
}