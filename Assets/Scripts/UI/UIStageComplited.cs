using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ResourceSystem;

public class UIStageComplited : MonoBehaviour
{
    [SerializeField] private IntResourceContainer _rewardContainer;
    [Header("Task")]
    [SerializeField] private TextMeshProUGUI _complitedLevel;
    [Header("Reward")]
    [SerializeField] private ResourceSO _money;
    [SerializeField] private ResourceSO _gems;
    [SerializeField] private TextMeshProUGUI _moneyAmoText;
    [SerializeField] private TextMeshProUGUI _gemsAmoText;
    private StageReward _reward;


    public void LoadData(Stage stageData)
    {
        _complitedLevel.text = stageData.stageValue;
    }


    public void LoadReward(StageReward reward)
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
        foreach (var reward in _reward.rewards)
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
