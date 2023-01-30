using System.Collections;
using System.Collections.Generic;
using Runtime;
using UnityEngine;

public class EnterTeamName : MonoBehaviour
{
    public Transform _popup;
    
    // Here will be called Actions.SetTeamLogo() method. TeamLogo will be retrieved using IndexerLogoLoader.LoadLogo()
    // method and only team_name will be changed in this structure. Team name will be provided by user

    public void SignOut()
    {
        Near.NearPersistentManager.Instance.SignOut();
        Game.LoadMainMenu();
    }
    
    public void Close()
    {
        _popup.gameObject.SetActive(false);
    }
}
