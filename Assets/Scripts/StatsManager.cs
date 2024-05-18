using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceSystem;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private ResourceDataContainer _statsContainer;
    [Space]
    [SerializeField] private ResourceSO _speedResource;
    [SerializeField] private ResourceSO _incomeResource;
    [SerializeField] private ResourceSO _totalIncomeResource;


    public void ChangeSpeed(int value)
    {
        _statsContainer.ChangeResourceValue(_speedResource, value);
    }

    public void ChangeIncome(int value)
    {
        _statsContainer.ChangeResourceValue(_incomeResource, value);
    }

    public void ChangeTotalIncome(int value)
    {
        _statsContainer.ChangeResourceValue(_totalIncomeResource, value);
    }
}
