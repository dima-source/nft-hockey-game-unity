using System.Collections;
using System.Collections.Generic;
using Runtime;
using TMPro;
using UI.Profile;
using UI.Profile.Models;
using UI.Scripts;
using UnityEngine;
using UnityEngine.UI;


public class EnterTeamName : MonoBehaviour
{
    private readonly string _pathForm = "/Assets/Resources/Sprites/TeamLogo/Form/";
    private readonly string _pathPattern = "/Assets/Resources/Sprites/TeamLogo/";
    public Transform _popup;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _test;
    private Button _confirm;
    private Button _cancel;
    private Button _goBack;
    private string Title;
    private TMP_InputField inputName;
    private ILogoSaver _logoSaver = new ContractLogoSaver();
    private ILogoLoader _logoLoader = new IndexerLogoLoader();
    [SerializeField] private LogoPrefab _logoPrefab;
    
    
    private void Awake()
    {
        inputName = UiUtils.FindChild<TMP_InputField>(transform, "inputName");
        _test = UiUtils.FindChild<TextMeshProUGUI>(transform, "Test");
        _goBack = UiUtils.FindChild<Button>(transform, "GoBack");
        _goBack.onClick.AddListener(() => Close());
        _cancel = UiUtils.FindChild<Button>(transform, "Cancel");
        _cancel.onClick.AddListener(() => Close());
        _confirm = UiUtils.FindChild<Button>(transform, "Confirm");
        _confirm.onClick.AddListener(() => Save(inputName.text));
        _logoPrefab = UiUtils.FindChild<LogoPrefab>(transform, "LogoTest");
        Load();
    }
    
    public async void ChangeTeamName(string teamName)
    {
        _test.text = teamName;
        _logoPrefab.ChangeTeamName(teamName);
        await _logoSaver.SaveLogo(_logoPrefab.GetTeamLogo());
        gameObject.SetActive(false);
    }

    public void ChangeUserName(string userName)
    {
        _test.text = userName;
        _logoPrefab.ChangeUsername(userName);
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
        
    public async void Save(string name)
    {
        _test.text = name;
        _logoPrefab.ChangeTeamName(name);
        await _logoSaver.SaveLogo(_logoPrefab.GetTeamLogo());
        gameObject.SetActive(false);
    }

    public void Close()
    {
        _popup.gameObject.SetActive(false);
    }
}

