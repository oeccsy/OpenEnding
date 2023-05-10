using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugText : Singleton<DebugText>
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        text.text = "";
    }
    
    public void AddText(string str)
    {
        text.text = text.text + "\n" + str;
    }

    public void SetText(string str)
    {
        text.text = str;
    }
}
