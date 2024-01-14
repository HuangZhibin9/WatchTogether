using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutputPath : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private static string _text;

    private void Update()
    {
        text.text = _text;
    }

    public static void SetText(string str)
    {
        _text = str;
    }
}