using System.Text.RegularExpressions;
using System.Threading.Tasks;
using dotnetstandard_bip39;
using Near;
using TMPro;
using UI.Main_menu.UIPopups;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_menu
{
    public class SignInView : MonoBehaviour
    {
        [SerializeField] private MainMenuView mainMenuView;
        [SerializeField] private Text inputUri;
        [SerializeField] private TMP_InputField accountIdInput;
        [SerializeField] private TMP_Text inputDescription;
        [SerializeField] private Transform inputPopup;
        [SerializeField] private Transform infoPopup;

        [SerializeField] private SeedPhraseView seedPhrase;
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

        private async Task<bool> ValidateAccountId()
        {
            string accountId = accountIdInput.text.Trim();
            if (accountId.Length > 64)
            {
                inputDescription.text = "Account id must be longer than 2 and less than 64 symbols";
                return false;
            }
            
            string[] parts = accountId.Split(".");
            if (parts.Length != 2)
            {
                inputDescription.text = "Incorrect input";
                return false;
            }

            if (parts[1] != "testnet")
            {
                inputDescription.text = "Account id must end with \"testnet\"";
                return false;
            }

            Regex regex = new(@"^(([a-z\d]+[\-_])*[a-z\d]+\.)*([a-z\d]+[\-_])*[a-z\d]+$");
            if (!regex.IsMatch(accountId))
            {
                inputDescription.text = "Invalid account id format";
                return false;
            }
            
            if (await Utils.Utils.CheckAccountIdAvailability(accountId))
            {
                inputDescription.text = "Such account already exists";
                return false;
            }
            return true;
        }

        public async void RegisterAccount()
        {
            if (!await ValidateAccountId())
            {
                return;
            }

            var bip = new BIP39();
            seedPhrase.SeedPhraseText.text = bip.GenerateMnemonic(128, BIP39Wordlist.English).Replace("\r", "");
            string accountId = accountIdInput.text.Trim();
            await NearPersistentManager.Instance.Register(accountId, seedPhrase.SeedPhraseText.text);
            mainMenuView.LoadAccountId();
            
            inputPopup.gameObject.SetActive(false);
            infoPopup.gameObject.SetActive(true);
        }
    }
}