using UI.Profile.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Profile
{
    public class CreateTeamLogoView : MonoBehaviour
    {
        private string firstLayerColorNumber;
        private string secondLayerColorNumber;
        private string inputLayerColorNumber;
        private string inputGroundColorNumber;
        private LogoPrefab _logoPrefab;
        private readonly string _pathForm = "/Assets/Sprites/Profile/Form/";
        private readonly string _pathPattern = "/Assets/Sprites/Profile";
        private ILogoSaver _logoSaver = new ContractLogoSaver();
        private ILogoLoader _logoLoader = new IndexerLogoLoader();
        private Button _saveButton;
        private Button _resetButton;
        private Button _background;
        private Button _closePopupButton;

       
        
        private void Awake()
        {
            _logoPrefab = Scripts.UiUtils.FindChild<LogoPrefab>(transform, "Logo");
            _saveButton = Scripts.UiUtils.FindChild<Button>(transform, "SaveButton");
            _resetButton = Scripts.UiUtils.FindChild<Button>(transform, "ResetButton");
            _background = Scripts.UiUtils.FindChild<Button>(transform, "MainBackground");
            _closePopupButton = Scripts.UiUtils.FindChild<Button>(transform, "ClosePopup");
            _saveButton.onClick.AddListener(Save);
            _resetButton.onClick.AddListener(Load);
            _background.onClick.AddListener(Close);
            _closePopupButton.onClick.AddListener(Close);
            Load();
        }

        private void OnEnable()
        {
            Load();
        }

        public async void Load() 
        {
            TeamLogo logoData = await _logoLoader.LoadLogo();
            Debug.Log(logoData.form_name);
            Load(logoData);
        }

        private void Load(TeamLogo teamLogo) 
        {
            _logoPrefab.SetData(teamLogo, _pathForm, _pathPattern);
        }
        
        public async void Save()
        {
            await _logoSaver.SaveLogo(_logoPrefab.GetTeamLogo());
            gameObject.SetActive(false);
            
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
        }
        
        public void ChangeForm(string form)
        {
            _logoPrefab.ChangeLayerForm(form);
        }

        public void ChangePattern(string pattern)
        {
            _logoPrefab.ChangeLayerPattern(pattern);
        }

        public void ChangeFirstLayerColor(string colorNumber)
        {
            _logoPrefab.ChangeFirstLayerColor(colorNumber);
        }
        
        public void ChangeSecondLayerColor(string colorNumber)
        {
            _logoPrefab.ChangeSecondLayerColor(colorNumber);
        }
    }
}