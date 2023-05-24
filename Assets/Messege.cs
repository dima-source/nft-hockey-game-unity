using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Messege : MonoBehaviour
{
    public TextMeshProUGUI _messageText;
    
    public void SetData(string messageText)
    {
        _messageText.text = messageText;
    }
}
