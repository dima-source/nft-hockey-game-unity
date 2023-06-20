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
    public string first = "";
    string second = "";
    public Transform Mid;
    public Transform BluePossesion;
    public Transform RedPossesion;
    public Transform RedAttack;
    public Transform BlueAttack;
    public TextMeshProUGUI FinalCount;
    public TextMeshProUGUI FinalShoots;
    public TextMeshProUGUI FinalPossesing;
    public TextMeshProUGUI FinalMessege;
    
    public void ShowPrefabPopup(string name, string text)
    { 
        first = text;
        if (first != second)
        {
            string PATH = Configurations.PrefabsFolderPath + $"Popups/Profile/{name}";

            GameObject prefab = UiUtils.LoadResource<GameObject>(PATH);
            _messege = Instantiate(prefab, profileMassegeParentArea).GetComponent<Messege>();
            _messege.SetData(text);
        }

        second = text;
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
        int GoalPossible = 0;
        int RedOwn = 0;
        int BlueOwn = 0;
        int RedShoots = 0;
        int BlueShoots = 0;
        PeridScore.text = "1";
        for (int i = 0; i < 30; i++)
        {
            if (GoalPossible == 0)
            {
                ShowPrefabPopup("Messege", "Mid game");
                ChangeZone("mid");
            }
            else if(GoalPossible > 0)
            {
                ShowPrefabPopup("Messege", "Red team in possession");
                ChangeZone("RP");
            }
            else if(GoalPossible < 0)
            {
                ShowPrefabPopup("Messege", "Blue team in possession");
                ChangeZone("BP");
            }

            int Count = Random.Range(0, 100);
            if(Count > 50)
            {
                if (Count > 80)
                {
                    GoalPossible += 1;
                    RedOwn += 1;
                }
                else if (Count > 90)
                {
                    GoalPossible += 2;
                    RedOwn += 2;
                }
                if (Count > 95)
                {
                    GoalPossible += 1;
                    RedOwn += 1;
                }
                
                if(GoalPossible >= 3)
                {
                    ShowPrefabPopup("Messege", "Red team in attack");
                    ChangeZone("RA");
                    RedShoots += 1;
                    int GoalFlag = Random.Range(0, 100);
                    if (GoalFlag > 33)
                    {
                        RedScore += 1;
                        ShowPrefabPopup("Messege", "Red team goal!");
                        GoalPossible = 0;
                    }
                }
            }
            else
            {
                if (Count < 20)
                {
                    GoalPossible -= 1;
                    BlueOwn += 1;
                }
                else if (Count < 10)
                {
                    GoalPossible -= 2;
                    BlueOwn += 2;
                }

                if (Count < 5)
                {
                    GoalPossible -= 1;
                    BlueOwn += 1;
                }
                
                if(GoalPossible <= -3)
                {
                    ShowPrefabPopup("Messege", "Blue team in attack");
                    BlueShoots += 1;
                    ChangeZone("BA");
                    int GoalFlag = Random.Range(0, 100);
                    if (GoalFlag > 33)
                    {
                        BlueScore += 1;
                        ShowPrefabPopup("Messege", "Blue team goal!");
                        GoalPossible = 0;
                    }
                }
            }
            yield return new WaitForSeconds(1f); 
            ScoreCount.text = $"{RedScore}-{BlueScore}";
            TimeScore.text = $"{30-i}";
        }
        
        PeridScore.text = "2";
        
       for (int i = 0; i < 30; i++)
        {
            if (GoalPossible == 0)
            {
                ShowPrefabPopup("Messege", "Mid game");
                ChangeZone("mid");
            }
            else if(GoalPossible > 0)
            {
                ShowPrefabPopup("Messege", "Red team in possession");
                ChangeZone("RP");
            }
            else if(GoalPossible < 0)
            {
                ShowPrefabPopup("Messege", "Blue team in possession");
                ChangeZone("BP");
            }

            int Count = Random.Range(0, 100);
            if(Count > 50)
            {
                if (Count > 80)
                {
                    GoalPossible += 1;
                    RedOwn += 1;
                }
                else if (Count > 90)
                {
                    RedOwn += 2;
                    GoalPossible += 2;
                }
                if (Count > 95)
                {
                    RedOwn += 1;
                    GoalPossible += 1;
                }
                
                if(GoalPossible >= 3)
                {
                    RedShoots += 1;
                    ShowPrefabPopup("Messege", "Red team in attack");
                    ChangeZone("RA");
                    int GoalFlag = Random.Range(0, 100);
                    if (GoalFlag > 33)
                    {
                        RedScore += 1;
                        ShowPrefabPopup("Messege", "Red team goal!");
                        GoalPossible = 0;
                    }
                }
            }
            else
            {
                if (Count < 20)
                {
                    BlueOwn += 1;
                    GoalPossible -= 1;
                }
                else if (Count < 10)
                {
                    BlueOwn += 2;
                    GoalPossible -= 2;
                }

                if (Count < 5)
                {
                    BlueOwn += 1;
                    GoalPossible -= 1;
                }
                
                if(GoalPossible <= -3)
                {
                    ShowPrefabPopup("Messege", "Blue team in attack");
                    BlueShoots += 1;
                    ChangeZone("BA");
                    int GoalFlag = Random.Range(0, 100);
                    if (GoalFlag > 33)
                    {
                        BlueScore += 1;
                        ShowPrefabPopup("Messege", "Blue team goal!");
                        GoalPossible = 0;
                    }
                }
            }
            yield return new WaitForSeconds(1f); 
            ScoreCount.text = $"{RedScore}-{BlueScore}";
            TimeScore.text = $"{30-i}";
        }
        
        PeridScore.text = "3";
        
        for (int i = 0; i < 30; i++)
        {
            if (GoalPossible == 0)
            {
                ShowPrefabPopup("Messege", "Mid game");
                ChangeZone("mid");
            }
            else if(GoalPossible > 0)
            {
                ShowPrefabPopup("Messege", "Red team in possession");
                ChangeZone("RP");
            }
            else if(GoalPossible < 0)
            {
                ShowPrefabPopup("Messege", "Blue team in possession");
                ChangeZone("BP");
            }

            int Count = Random.Range(0, 100);
            if(Count > 50)
            {
                if (Count > 80)
                {
                    RedOwn += 1;
                    GoalPossible += 1;
                }
                else if (Count > 90)
                {
                    RedOwn += 2;
                    GoalPossible += 2;
                }
                if (Count > 95)
                {
                    RedOwn += 1;
                    GoalPossible += 1;
                }
                
                if(GoalPossible >= 3)
                {
                    ShowPrefabPopup("Messege", "Red team in attack");
                    ChangeZone("RA");
                    RedShoots += 1;
                    int GoalFlag = Random.Range(0, 100);
                    if (GoalFlag > 33)
                    {
                        RedScore += 1;
                        ShowPrefabPopup("Messege", "Red team goal!");
                        GoalPossible = 0;
                    }
                }
            }
            else
            {
                if (Count < 20)
                {
                    BlueOwn += 1;
                    GoalPossible -= 1;
                }
                else if (Count < 10)
                {
                    BlueOwn += 2;
                    GoalPossible -= 2;
                }

                if (Count < 5)
                {
                    BlueOwn += 1;
                    GoalPossible -= 1;
                }
                
                if(GoalPossible <= -3)
                {
                    ShowPrefabPopup("Messege", "Blue team in attack");
                    ChangeZone("BA");
                    BlueShoots += 1;
                    int GoalFlag = Random.Range(0, 100);
                    if (GoalFlag > 33)
                    {
                        BlueScore += 1;
                        ShowPrefabPopup("Messege", "Blue team goal!");
                        GoalPossible = 0;
                    }
                }
            }
            yield return new WaitForSeconds(1f); 
            ScoreCount.text = $"{RedScore}-{BlueScore}";
            TimeScore.text = $"{30-i}";
        }

        #region EndGame
        ChangeZone(null);
        
            if (RedScore > BlueScore)
            {
                ShowPrefabPopup("Messege","Red team win !");
                FinalMessege.text = "Victory !";
            }
            else if(BlueScore > RedScore)
            {
                ShowPrefabPopup("Messege","Blue team win !");
                FinalMessege.text = "Lose !";
            }
            else
            {
                ShowPrefabPopup("Messege","Draw !");
                FinalMessege.text = "Draw !";
            }
            
            FinalPossesing.text = $"{RedOwn}-{BlueOwn}";
            FinalCount.text = $"{RedScore}-{BlueScore}";
            FinalShoots.text = $"{RedShoots}-{BlueShoots}";
            WinPopup.gameObject.SetActive(true);
        
        
        #endregion
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CloseStartPopup()
    {
        StartPopup.gameObject.SetActive(false);
    }

    public void ChangeZone(string name)
    {
        Mid.gameObject.SetActive(false);
        RedPossesion.gameObject.SetActive(false);
        RedAttack.gameObject.SetActive(false);
        BlueAttack.gameObject.SetActive(false);
        BluePossesion.gameObject.SetActive(false);
        if (name == "mid")
        {
            Mid.gameObject.SetActive(true);
        }
        else if (name == "BP")
        {
            BluePossesion.gameObject.SetActive(true);
        }
        else if (name == "RP")
        {
            RedPossesion.gameObject.SetActive(true);
        }
        else if (name == "RA")
        {
            RedAttack.gameObject.SetActive(true);
        }
        else if (name == "BA")
        {
            BlueAttack.gameObject.SetActive(true);
        }
    }
}