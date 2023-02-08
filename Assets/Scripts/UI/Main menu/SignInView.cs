using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using dotnetstandard_bip39;
using Near;
using TMPro;
using UI.Main_menu.UIPopups;
using UI.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Main_menu
{
    public class SignInView : UiComponent
    { 
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private Text inputUri; 
        [SerializeField] private TMP_InputField accountIdInput; 
        [SerializeField] private TMP_Text inputDescription;
        [SerializeField] private InputPopup inputPopup; 
        [SerializeField] private Transform infoPopup; 
        [SerializeField] private TMP_Dropdown accountsDropdown; 
        [SerializeField] private SeedPhraseView seedPhrase;
        
        protected override void Initialize()
        {
            inputUri = UiUtils.FindChild<Text>(transform, "TextUri");
            accountIdInput = UiUtils.FindChild<TMP_InputField>(transform, "AccountIdInput");
            inputDescription = UiUtils.FindChild<TMP_Text>(transform, "DescriptionText");
            inputPopup = UiUtils.FindChild<InputPopup>(transform, "InputPopup");
            infoPopup = UiUtils.FindChild<Transform>(transform, "InfoPopup");
            accountsDropdown = UiUtils.FindChild<TMP_Dropdown>(transform, "Dropdown");
            seedPhrase = UiUtils.FindChild<SeedPhraseView>(transform, "SeedPhraseField");
           //mainMenu = UiUtils.FindParent<MainMenu>(transform,"MainMenu");
        }
        private void Start()
        {
            inputPopup.HideSpinner();
        }

        private async void OnEnable()
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            foreach (var accountId in await NearPersistentManager.Instance.GetAvailableAccounts())
                options.Add(new TMP_Dropdown.OptionData(accountId));
            accountsDropdown.options = options;
        }

        public async void CompleteSignIn()
        {
            // Application.deepLinkActivated -= CompleteSignIn;
            
            await NearPersistentManager.Instance.WalletAccount.CompleteSignIn(inputUri.text);
            if(NearPersistentManager.Instance.WalletAccount.IsSignedIn())
            {
                gameObject.SetActive(false);
                mainMenu.gameObject.SetActive(true);
                mainMenu.LoadAccountId();
            }   
        }
        
        public async void RequestSignIn()
        {
            await NearPersistentManager.Instance.SignIn();
        }

        private async Task<bool> ValidateAccountId()
        {
            string accountId = accountIdInput.text.Trim();
            var inputDescriptionParent = inputDescription.transform.parent;
            if (accountId.Length > 64)
            {
                inputDescriptionParent.gameObject.SetActive(true);
                inputDescription.text = "Account id must be longer than 2 and less than 64 symbols";
                return false;
            }
            
            string[] parts = accountId.Split(".");
            if (parts.Length != 2)
            {
                inputDescriptionParent.gameObject.SetActive(true);
                inputDescription.text = "Incorrect input";
                return false;
            }

            if (parts[1] != "testnet")
            {
                inputDescriptionParent.gameObject.SetActive(true);
                inputDescription.text = "Account id must end with \"testnet\"";
                return false;
            }

            Regex regex = new(@"^(([a-z\d]+[\-_])*[a-z\d]+\.)*([a-z\d]+[\-_])*[a-z\d]+$");
            if (!regex.IsMatch(accountId))
            {
                inputDescriptionParent.gameObject.SetActive(true);
                inputDescription.text = "Invalid account id format";
                return false;
            }
            
            if (await Utils.Utils.CheckAccountIdAvailability(accountId))
            {
                inputDescriptionParent.gameObject.SetActive(true);
                inputDescription.text = "Such account already exists";
                return false;
            }
            return true;
        }

        public async void LoadAccount()
        {
            var accountId = (await NearPersistentManager.Instance.GetAvailableAccounts())[accountsDropdown.value];
            Debug.Log(accountId);
            await NearPersistentManager.Instance.LoadAccount(accountId);
            mainMenu.LoadAccountId();
            gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }

        public async void RegisterAccount()
        {
            inputPopup.ShowSpinner();
            if (!await ValidateAccountId())
            {
                inputPopup.HideSpinner();
                return;
            }

            var bip = new BIP39();
            seedPhrase.SeedPhraseText.text = bip.GenerateMnemonic(128, BIP39Wordlist.English).Replace("\r", "");
            string accountId = accountIdInput.text.Trim();
            await NearPersistentManager.Instance.Register(accountId, seedPhrase.SeedPhraseText.text);
            mainMenu.LoadAccountId();
            inputPopup.HideSpinner();
            
            inputPopup.gameObject.SetActive(false);
            infoPopup.gameObject.SetActive(true);
        }
    }
}