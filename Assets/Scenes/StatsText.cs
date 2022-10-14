using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsText : MonoBehaviour
{
    public static string StatText = string.Empty;
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = StatText;
    }
}
