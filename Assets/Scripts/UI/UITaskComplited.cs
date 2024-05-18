using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ResourceSystem;

public class UITaskComplited : MonoBehaviour
{
    [SerializeField] private IntResourceContainer _rewardContainer;
    [Space]
    [Header("Task")]
    [SerializeField] private Image _resourceIcon;
    [SerializeField] private TextMeshProUGUI _resAmountText;
    [SerializeField] private TextMeshProUGUI _taskTypeText;
    [Space]
    [SerializeField] private GameObject _timeLimitTextObj;
    [SerializeField] private TextMeshProUGUI _timeLimitText;
    [Space]
    [Header("Reward")]
    [SerializeField] private ResourceSO _money;
    [SerializeField] private ResourceSO _gems;
    [SerializeField] private TextMeshProUGUI _moneyAmoText;
    [SerializeField] private TextMeshProUGUI _gemsAmoText;
    private TaskReward _reward;

    public void LoadData(GameTaskSO taskData)
    {
        _resAmountText.text = ValueToStringConverter.GetBeautifulIntText(taskData.taskResource.CurrentValue);

        switch (taskData.type)
        {
            case TaskType.Harvest:
                _taskTypeText.text = "Harvest";
                _timeLimitTextObj.SetActive(false);
                break;
            case TaskType.HarvestWithTime:
                _taskTypeText.text = "Harvest";
                _timeLimitTextObj.SetActive(true);
                _timeLimitText.text = $"{taskData.taskTime} sec";
                break;
            case TaskType.Earn:
                _taskTypeText.text = "Earn";
                _timeLimitTextObj.SetActive(false);
                break;

            case TaskType.EarnWithTime:
                _taskTypeText.text = "Earn";
                _timeLimitTextObj.SetActive(true);
                _timeLimitText.text = $"{taskData.taskTime} sec";
                break;
            default:
                _taskTypeText.text = "Collect";
                break;
        }

        _resourceIcon.sprite = taskData.taskResource.ResourceIcon;
    }

    public void LoadReward(TaskReward reward)
    {
        _reward = reward;
        LoadRewardData();
        gameObject.SetActive(true);
    }


    public void ClaimExtraReward()
    {
        int x = 3;
        foreach (var reward in _reward.rewards)
        {
            var rew = reward.CloneWithValue(reward.CurrentValue * 3) as IntResourceData;
            _rewardContainer.AddResourceOld(rew);
        }
        _reward = default;
    }


    public void ClaimReward()
    {
        foreach(var reward in _reward.rewards)
        {
            _rewardContainer.AddResourceOld(reward);
        }
        _reward = default;
    }


    private void LoadRewardData()
    {
        _moneyAmoText.text = ValueToStringConverter.GetBeautifulIntText(_reward.rewards.Find(x => x.ResourceID == _money.ID).CurrentValue);
        _gemsAmoText.text = ValueToStringConverter.GetBeautifulIntText(_reward.rewards.Find(x => x.ResourceID == _gems.ID).CurrentValue);
    }

    
}

public class ValueToStringConverter
{
    public static string GetBeautifulIntText(int value)
    {
        string baseStr = "";
        if (value > 1000000000)
        {
            baseStr = $"{value / 1000000000}.{(value % 1000000000) / 10000000}";
            if(baseStr.Length > 4)
            {
                baseStr = baseStr.Substring(0, 4).TrimEnd('.');
            }
            return $"{baseStr} B";
        }
        else if (value > 1000000)
        {
            baseStr = $"{value / 1000000}.{(value % 1000000) / 10000}";
            if (baseStr.Length > 4)
            {
                baseStr = baseStr.Substring(0, 4).TrimEnd('.');
            }
            return $"{baseStr} M";
        }
        else if (value > 1000)
        {
            baseStr = $"{value / 1000}.{(value % 1000) / 10}";
            if (baseStr.Length > 4)
            {
                baseStr = baseStr.Substring(0, 4).TrimEnd('.');
            }
            return $"{baseStr} K";
        }

        return value.ToString();
    }
}

