using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using dotnetstandard_bip39;
using Near;
using TMPro;
using UI.Main_menu.UIPopups;
using UI.Scripts;
using UnityEngine;

namespace UI.Main_menu
{
    public class SignInView : UiComponent
    { 
        private TMP_InputField _inputUri; 
        private TMP_InputField _accountIdInput; 
        private TMP_Text _inputDescription;
        private InputPopup _inputPopup; 
        private Transform _infoPopup; 
        private TMP_Dropdown _accountsDropdown; 
        private SeedPhraseView _seedPhrase;

        private const string MainMenuPath = "Prefabs/MainMenu";
        
        protected override void Initialize()
        {
            _inputUri = UiUtils.FindChild<TMP_InputField>(transform, "InputUri");
            _accountIdInput = UiUtils.FindChild<TMP_InputField>(transform, "AccountIdInput");
            _inputDescription = UiUtils.FindChild<TMP_Text>(transform, "DescriptionText");
            _inputPopup = UiUtils.FindChild<InputPopup>(transform, "InputPopup");
            _infoPopup = UiUtils.FindChild<Transform>(transform, "InfoPopup");
            _accountsDropdown = UiUtils.FindChild<TMP_Dropdown>(transform, "Dropdown");
            _seedPhrase = UiUtils.FindChild<SeedPhraseView>(transform, "SeedPhraseField");
        }
        private void Start()
        {
            _inputPopup.HideSpinner();
        }

        private async void OnEnable()
        {
            var options = new List<TMP_Dropdown.OptionData>();
            foreach (var accountId in await NearPersistentManager.Instance.GetAvailableAccounts())
            {
                options.Add(new TMP_Dropdown.OptionData(accountId));
            }
            _accountsDropdown.options = options;
        }

        public async void CompleteSignIn()
        {
            // Application.deepLinkActivated -= CompleteSignIn;
            
            await NearPersistentManager.Instance.WalletAccount.CompleteSignIn(_inputUri.text);
            if(NearPersistentManager.Instance.WalletAccount.IsSignedIn())
            {
                gameObject.SetActive(false);
                LoadAccount();
            }   
        }

        private void LoadMainMenu()
        {
            var mainMenuPrefab = UiUtils.LoadResource<MainMenu>(MainMenuPath);
            var mainMenu = Instantiate(mainMenuPrefab, transform.parent);
            mainMenu.LoadAccountId();
        }
        
        public async void RequestSignIn()
        {
            await NearPersistentManager.Instance.SignIn();
        }

        private async Task<bool> ValidateAccountId()
        {
            var accountId = _accountIdInput.text.Trim();
            var inputDescriptionParent = _inputDescription.transform.parent;
            if (accountId.Length > 64)
            {
                inputDescriptionParent.gameObject.SetActive(true);
                _inputDescription.text = "Account id must be longer than 2 and less than 64 symbols";
                return false;
            }
            
            var parts = accountId.Split(".");
            if (parts.Length != 2)
            {
                inputDescriptionParent.gameObject.SetActive(true);
                _inputDescription.text = "Incorrect input";
                return false;
            }

            if (parts[1] != "testnet")
            {
                inputDescriptionParent.gameObject.SetActive(true);
                _inputDescription.text = "Account id must end with \"testnet\"";
                return false;
            }

            Regex regex = new(@"^(([a-z\d]+[\-_])*[a-z\d]+\.)*([a-z\d]+[\-_])*[a-z\d]+$");
            if (!regex.IsMatch(accountId))
            {
                inputDescriptionParent.gameObject.SetActive(true);
                _inputDescription.text = "Invalid account id format";
                return false;
            }
            
            if (await Utils.Utils.CheckAccountIdAvailability(accountId))
            {
                inputDescriptionParent.gameObject.SetActive(true);
                _inputDescription.text = "Such account already exists";
                return false;
            }
            return true;
        }

        public async void LoadAccount()
        {
            var accountId = (await NearPersistentManager.Instance.GetAvailableAccounts())[_accountsDropdown.value];
            Debug.Log(accountId);
            await NearPersistentManager.Instance.LoadAccount(accountId);
            LoadMainMenu();
            gameObject.SetActive(false);
        }

        public async void RegisterAccount()
        {
            _inputPopup.ShowSpinner();
            if (!await ValidateAccountId())
            {
                _inputPopup.HideSpinner();
                return;
            }

            var bip = new BIP39();
            _seedPhrase.SeedPhraseText.text = bip.GenerateMnemonic(128, BIP39Wordlist.English).Replace("\r", "");
            string accountId = _accountIdInput.text.Trim();
            await NearPersistentManager.Instance.Register(accountId, _seedPhrase.SeedPhraseText.text);
            LoadMainMenu();
            _inputPopup.HideSpinner();
            
            _inputPopup.gameObject.SetActive(false);
            _infoPopup.gameObject.SetActive(true);
        }
    }
}