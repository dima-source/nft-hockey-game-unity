using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UI.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Simulation : MonoBehaviour
{
    public Transform StartButton;
    public TextMeshProUGUI ScoreCount;
    public TextMeshProUGUI PeridScore;
    public TextMeshProUGUI TimeScore;
    public Messege _messege;
    public Transform EndPopup;
    [SerializeField] private Transform profileMassegeParentArea;
    public Transform UserTeam;
    public Transform EnemyTeam;
    public Transform LosePopup;
    public Transform WinPopup;
    public Transform DrawPopup;
    public Transform StartPopup;
    
    public void ShowPrefabPopup(string name, string text)
    {
        string PATH = Configurations.PrefabsFolderPath + $"Popups/Profile/{name}";

        GameObject prefab = UiUtils.LoadResource<GameObject>(PATH);
        _messege = Instantiate(prefab, profileMassegeParentArea).GetComponent<Messege>();
        _messege.SetData(text);
        
    }
    
    void Start()
    {
        StartPopup.gameObject.SetActive(true);
    }

    public void ClickEnemy()
    {
        UserTeam.gameObject.SetActive(false);
        EnemyTeam.gameObject.SetActive(true);
    }
    
    public void ClickUser()
    {
        UserTeam.gameObject.SetActive(true);
        EnemyTeam.gameObject.SetActive(false);
    }


    public void StartSimulation()
    {
        StartButton.gameObject.SetActive(false);
        StartCoroutine(UpdateScoreText());
    }

    private IEnumerator UpdateScoreText()
    {
        int RedScore = 0;
        int BlueScore = 0;
        PeridScore.text = "1";
        for (int i = 0; i < 30; i++)
        {
            int Count = Random.Range(0, 100);
            if(Count > 50)
            {
                if(Count > 90)
                {
                    RedScore += 1;
                    ShowPrefabPopup("Messege", "Red team goal!");
                }
            }
            else
            {
                if (Count < 10)
                {
                    BlueScore += 1;
                    ShowPrefabPopup("Messege","Blue team goal!");
                }
            }
            yield return new WaitForSeconds(0.5f); 
            ScoreCount.text = $"{RedScore}-{BlueScore}";
            TimeScore.text = $"{30-i}";
        }
        
        PeridScore.text = "2";
        
        for (int i = 0; i < 30; i++)
        {
            int Count = Random.Range(0, 100);
            if(Count > 50)
            {
                if(Count > 90)
                {
                    RedScore += 1;
                    ShowPrefabPopup("Messege", "Red team goal!");
                }
            }
            else
            {
                if (Count < 10)
                {
                    BlueScore += 1;
                    ShowPrefabPopup("Messege","Blue team goal!");
                }
            }
            yield return new WaitForSeconds(0.5f); 
            ScoreCount.text = $"{RedScore}-{BlueScore}";
            TimeScore.text = $"{30-i}";
        }
        
        PeridScore.text = "3";
        
        for (int i = 0; i < 30; i++)
        {
            int Count = Random.Range(0, 100);
            if(Count > 50)
            {
                if(Count > 90)
                {
                    RedScore += 1;
                    ShowPrefabPopup("Messege", "Red team goal!");
                }
            }
            else
            {
                if (Count < 10)
                {
                    BlueScore += 1;
                    ShowPrefabPopup("Messege","Blue team goal!");
                }
            }
            yield return new WaitForSeconds(0.5f); 
            ScoreCount.text = $"{RedScore}-{BlueScore}";
            TimeScore.text = $"{30-i}";
        }
        
        if (RedScore < BlueScore)
        {
            ShowPrefabPopup("Messege","Blue team win !");
            LosePopup.gameObject.SetActive(true);
        }
        else if(RedScore == BlueScore)
        {
            ShowPrefabPopup("Messege","Draw");
            DrawPopup.gameObject.SetActive(true);
        }
        else
        {
            ShowPrefabPopup("Messege","Red team win !");
            WinPopup.gameObject.SetActive(true);
        }
        
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CloseStartPopup()
    {
        StartPopup.gameObject.SetActive(false);
    }
}