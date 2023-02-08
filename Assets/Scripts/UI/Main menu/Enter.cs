using Near;
using NearClientUnity.Utilities;
using UI.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Main_menu
{
    public class Enter : UiComponent
    { 
        private MainMenu mainMenu; 
        private SignInView signInView;

        private void Start()
        {
            if (NearPersistentManager.Instance.WalletAccount.IsSignedIn())
            {
                mainMenu.gameObject.SetActive(true);
                mainMenu.LoadAccountId();
                signInView.gameObject.SetActive(false);
            }
            else
            {
                mainMenu.gameObject.SetActive(false);
                signInView.gameObject.SetActive(true);
            }
        }

        protected override void Initialize()
        {
            mainMenu = UiUtils.FindChild<MainMenu>(transform, "MainMenu");
            signInView = UiUtils.FindChild<SignInView>(transform, "SignInMenu");
        }
    }
}