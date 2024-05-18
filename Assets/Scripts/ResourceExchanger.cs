using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ResourceSystem;

public class ResourceExchanger : MonoBehaviour, IStationEnabler
{
    [SerializeField] private float _maxExchangeTime = 5f;
    [SerializeField] private float _exchangeStepTime = 0.1f;

    [SerializeField] private ExchengeData _exchengeData;
    [SerializeField] private IntResourceData _outResource;
    [SerializeField] private UnityEvent<int, int> _onSuccessfulyTaken;
    [SerializeField] private UnityEvent _onCouldntTake;

    private bool _isTrainEmpty = false;

    public bool CanTrainMove() => _isTrainEmpty;

    public async void Exchange(MergingTrain train)
    {
        _isTrainEmpty = false;
        CargoContainer cargo;

        if (!train.TryGetComponent(out cargo)) return;
        int steps = Mathf.Max(1, Mathf.CeilToInt( cargo.MaxCargo / (_maxExchangeTime / _exchangeStepTime)));
        bool _resourcesEnded = false;
        int successfulyTakenParts = 0;
        for (int i = steps; i > 0 && !_resourcesEnded; i--)
        {
            if (cargo.AvaliableCargoAmount > _outResource.CurrentValue - _exchengeData.InAmount
            && await cargo.TryTakeResource(_exchengeData.InID, _exchengeData.InAmount))
            {
                cargo.AddResourceOld(_outResource);
                successfulyTakenParts++;
            }
            else
            {
                _resourcesEnded = true;
            }
        }

        if(successfulyTakenParts > 0)
        {
            _onSuccessfulyTaken.Invoke(successfulyTakenParts * _exchengeData.InAmount, successfulyTakenParts * _outResource.CurrentValue);
        }
        else
        {
            //train.EnableMovment(true);
            _onCouldntTake.Invoke();
            _isTrainEmpty = true;
        }
    }
}


[System.Serializable]
public struct ExchengeData
{
    public string InID { get => _inResource.ID; }
    public int InAmount { get => _inAmount; }

    [SerializeField] private ResourceSO _inResource;
    [SerializeField] private int _inAmount;
}
