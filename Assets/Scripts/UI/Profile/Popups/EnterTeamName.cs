using System.Collections;
using System.Collections.Generic;
using Runtime;
using TMPro;
using UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class EnterTeamName : UiComponent
{
    public Transform _popup;
    [SerializeField] private TextMeshProUGUI _title;
    private Button _confirm;
    private Button _cancel;
    private Button _goBack;
    private string Title;
    private TMP_InputField inputName;
    
    
    protected override void Initialize()
    {
        inputName = UiUtils.FindChild<TMP_InputField>(transform, "inputName");
        _goBack = UiUtils.FindChild<Button>(transform, "GoBack");
        _goBack.onClick.AddListener(() => Close());
        _cancel = UiUtils.FindChild<Button>(transform, "Cancel");
        _cancel.onClick.AddListener(() => Close());
        _confirm = UiUtils.FindChild<Button>(transform, "Confirm");
        _confirm.onClick.AddListener(() => SetName(inputName.text));
    }
    
    protected override void OnUpdate()
    {
        SetData(Title);
    }

    public void SetData(string Title)
    {
        _title.text = Title;
    }

    public void SetName(string Name)
    {
        
    }

    public void Close()
    {
        _popup.gameObject.SetActive(false);
    }
}
