using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UITaskFailed : MonoBehaviour
{
    [Header("Task")]
    [SerializeField] private Image _resourceIcon;
    [SerializeField] private TextMeshProUGUI _resAmountText;
    [SerializeField] private TextMeshProUGUI _taskTypeText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [Space]
    [Header("Tips")]
    [SerializeField] private List<GameObject> _harvestTips;
    [SerializeField] private List<GameObject> _earnTips;

    public void LoadData(GameTaskSO taskData)
    {
        _resAmountText.text = ValueToStringConverter.GetBeautifulIntText(taskData.taskResource.CurrentValue);

        switch (taskData.type)
        {
            case TaskType.HarvestWithTime:
                _taskTypeText.text = "Harvest";
                foreach(var tip in _earnTips)
                {
                    tip.SetActive(false);
                }
                foreach (var tip in _harvestTips)
                {
                    tip.SetActive(true);
                }
                break;
            case TaskType.EarnWithTime:
                _taskTypeText.text = "Earn";
                foreach (var tip in _harvestTips)
                {
                    tip.SetActive(false);
                }
                foreach (var tip in _earnTips)
                {
                    tip.SetActive(true);
                }
                break;
            default:
                _taskTypeText.text = "Collect";
                break;
        }

        _timeText.text = $"{taskData.taskTime} sec";

        _resourceIcon.sprite = taskData.taskResource.ResourceIcon;
    }
}

