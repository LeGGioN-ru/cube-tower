using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    
    public void Notify(string key)
    {
        _textMeshPro.text = Utility.TakeLocalizationString(key);
    }
}
