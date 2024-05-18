using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICurMaxIntText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] private string _separator = "/";

    private int _maxValue = 100;
    private int _curValue = 0;

    public void UpdateMaxValue(int maxValue)
    {
        _maxValue = maxValue;
        UpdateText();
    }

    public void UpdateCurValue(int curValue)
    {
        _curValue = curValue;
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = $"{GetBeautifulStr(_curValue)}{_separator}{GetBeautifulStr(_maxValue)}";
    }

    private static string GetBeautifulStr(int value)
    {
        return value.ToString();
    }
}
