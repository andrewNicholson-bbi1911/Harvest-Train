using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResourceSystem;
using TMPro;

public class UICurMaxResText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _icon = null;
    [SerializeField] private string _separator = "/";

    private int _maxValue = 100;
    private int _curValue = 0;

    public void UpdateTargetResource(IntResourceData resource)
    {
        _maxValue = resource.CurrentValue;

        if(_icon != null)
        {
            _icon.sprite = resource.ResourceIcon;
        }

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
        return ValueToStringConverter.GetBeautifulIntText(value);
    }
}
