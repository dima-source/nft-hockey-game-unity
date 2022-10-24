using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Profile
{
    public class Profile : MonoBehaviour
    {
        public void GoMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void ShowPopup(Transform popupTransform)
        {
            popupTransform.gameObject.SetActive(true);
        }
    }
}