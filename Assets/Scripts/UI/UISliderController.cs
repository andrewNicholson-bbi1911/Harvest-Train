using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISliderController : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private string _postFix;

    public void UpdateSliderValue(int value)
    {
        _slider.value = value;
        _valueText.text = $"{value}{_postFix}";
    }

    public void UpdateMaxValue(int max)
    {
        _slider.maxValue = max;
    }
}
