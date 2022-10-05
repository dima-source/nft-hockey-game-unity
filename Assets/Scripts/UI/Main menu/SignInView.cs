using Near;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_menu
{
    public class SignInView : MonoBehaviour
    {
        [SerializeField] private MainMenuView mainMenuView;
        [SerializeField] private Text inputUri;

        private void Start()
        {
            // Application.deepLinkActivated += url => CompleteSignIn(url);
        }
        
        public async void CompleteSignIn()
        {
            // Application.deepLinkActivated -= CompleteSignIn;
            
            await NearPersistentManager.Instance.WalletAccount.CompleteSignIn(inputUri.text);
            if(NearPersistentManager.Instance.WalletAccount.IsSignedIn())
            {
                gameObject.SetActive(false);
                mainMenuView.gameObject.SetActive(true);
                mainMenuView.LoadAccountId();
            }   
        }
        
        public async void RequestSignIn()
        {
            await NearPersistentManager.Instance.SignIn();
        }

        public async void RequestRegister()
        {
            
        }
    }
}