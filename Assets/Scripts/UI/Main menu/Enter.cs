using Near;
using UI.Scripts;
using UnityEngine;

namespace UI.Main_menu
{
    public class Enter : MonoBehaviour
    {
        public Transform IntroPopup;
       
        private void Start()
        {
            RemoveChildren();
            var path = "Prefabs/";
            
            if (NearPersistentManager.Instance.WalletAccount.IsSignedIn())
            {
                path += "MainMenu";
            }
            else
            {
                path += "MainMenu";
            }
            
            var currentPage = UiUtils.LoadResource<GameObject>(path);
            Instantiate(currentPage, transform);
            
        }

        public void StartIntro()
        {
            IntroPopup.gameObject.SetActive(true);
        }

        public void Close()
        {
            IntroPopup.gameObject.SetActive(false);
        }

        private void RemoveChildren()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i));
            }
        }
    }
}