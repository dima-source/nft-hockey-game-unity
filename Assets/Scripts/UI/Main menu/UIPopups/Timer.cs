using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_menu.UIPopups
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;

        private float _seconds;
        private int _minutes;

        private void OnEnable()
        {
            _minutes = 0;
            _seconds = 0;
        }
        
        private void Update()
        {
            _seconds += Time.deltaTime;
            
            if (_seconds >= 60)
            {
                _seconds -= 60;
                _minutes += 1;
            }

            string minutesText = _minutes.ToString();
            if (_minutes < 10)
            {
                minutesText = "0" + minutesText;
            }

            string secondsText = Math.Round(_seconds).ToString();
            if (_seconds < 10)
            {
                secondsText = "0" + secondsText;
            }
            
            timeText.text = minutesText + " : " + secondsText;
        }
    }
}