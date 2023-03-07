using Near;
using UI.Scripts;
using UnityEngine;

namespace UI.Main_menu
{
    public class Enter : MonoBehaviour
    {
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
                path += "SignInMenu";
            }
            
            var currentPage = UiUtils.LoadResource<GameObject>(path);
            Instantiate(currentPage, transform);
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