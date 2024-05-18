using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceSystem;

public class ResourceSaler : MonoBehaviour, IStationEnabler
{
    public string ID { get => _salerID; }
    [SerializeField] private string _salerID = ""; 
    [SerializeField] private float maxTime = 3f;
    [SerializeField] private float stepTime = 0.1f;

    [SerializeField] private IntResourceData _moneyData;
    [SerializeField] private IntResourceContainer _moneyContainer;
    [SerializeField] private List<PriceData> _costData;
    [Space]
    [SerializeField] private ResourceDataContainer _statsContainer;
    [SerializeField] private ResourceSO _incomeResource;
    [SerializeField] private ResourceSO _totalIncomeResource;
    private float _currentCostPlexer;
    private float _normalizedAmount = 1f;

    private float _lastCash = 0f;

    private bool _isTrainEmpty = true;

    private void Start()
    {
        if(_statsContainer!=null)
            _statsContainer.onStatsChanged += (int val, string id) => { UpdateCurrentCostPlexer(); };

        _normalizedAmount = 1f / (maxTime / stepTime);
        UpdateCurrentCostPlexer();
    }


    public async void TakeResourcesFromTrain(MergingTrain train)
    {
        _isTrainEmpty = false;
        CargoContainer cargo;
        int totalTakeMoney = 0;
        int totres = 0;
        if(train.TryGetComponent(out cargo))
        {
            int takeAmount = Mathf.Max(1, Mathf.CeilToInt(_normalizedAmount * cargo.MaxCargo));

            foreach (var costData in _costData)
            {
                takeAmount = Mathf.Min(Mathf.Max(1, Mathf.CeilToInt(_normalizedAmount * cargo.MaxCargo)), cargo.GetResourceCurrentValue(costData.ResID));
                var realCost = costData.BaseCost;
                var taken = await cargo.TryTakeResource(costData.ResID, takeAmount);

                if (taken)
                {
                    totalTakeMoney += realCost * takeAmount;
                }

                totres += takeAmount;
            }
        }

        Debug.Log($"STATION>>>gold - {totalTakeMoney} and res - {totres}");

        if(totalTakeMoney > 0)
        {
            var earndMoney = totalTakeMoney * _currentCostPlexer;
            int realMoney = Mathf.FloorToInt(earndMoney);
            _lastCash += earndMoney - realMoney;
            if (_lastCash >= 1f)
            {
                realMoney += Mathf.FloorToInt(_lastCash);
                _lastCash %= 1;
            }

            _moneyContainer.AddResource(_moneyData.ResourceID, realMoney);
            _isTrainEmpty = false;
            //Debug.Log($"STATION>>>sent - {totalTakeMoney} && {earndMoney} && {realMoney}");
        }
        else
        {
            //train.EnableMovment(true);
            _isTrainEmpty = true;
        }
    }


    private void UpdateCurrentCostPlexer()
    {
        _currentCostPlexer = _statsContainer.GetResourceValue(_incomeResource) / 100f * _statsContainer.GetResourceValue(_totalIncomeResource) / 100f;
    }

    public bool CanTrainMove() => _isTrainEmpty;
}

[System.Serializable]
public struct PriceData
{
    public string ResID { get => _salingResource.ID; }
    public int BaseCost { get => baseCost; }

    [SerializeField] private ResourceSO _salingResource;
    [SerializeField] private int baseCost;
}