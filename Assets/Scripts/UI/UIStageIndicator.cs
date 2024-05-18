using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStageIndicator : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private List<GameObject> _separators;
    [SerializeField] private TextMeshProUGUI _stageNum;

    public void LoadStageData(Stage data)
    {
        _slider.maxValue = data.tasks.Count;

        int i = 0;
        foreach(var sep in _separators)
        {
            sep.SetActive( i < _slider.maxValue);
            i++;
        }

        _slider.value = 0;
        _stageNum.text = data.stageValue;
    }

    public void UpdateTaskID(int id)
    {
        _slider.value = id;
    }
}
