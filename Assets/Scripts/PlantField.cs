using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ResourceSystem;

public class PlantField : MonoBehaviour
{
    [SerializeField] private IntResourceData _startContainigData = null;
    [SerializeField] private int _startHealth = 10;
    [SerializeField] private float _respawnTime = 5f;
    [Space]
    [SerializeField] private IntResourceContainer _harvestedContainer;
    [SerializeField] private UnityEvent _onHarvested;
    [SerializeField] private UnityEvent<bool> _onReadynesStateUpdate;
    [SerializeField] private UnityEvent<int> _onHarvestStateUpdate;
    [SerializeField] private UnityEvent<float> _onGrowingStateUpdate;
    [SerializeField] private int _currentHealth = 0;
    private IntResourceData _currentResData = null;
    private bool _isGrowingState = false;

    private void Start()
    {
        _currentResData = _startContainigData.Clone() as IntResourceData;
        _currentHealth = _startHealth;
        ResetData();
    }


    internal void SetHarvestingContainer(IntResourceContainer container) => _harvestedContainer = container;

    public void Harvest(Dictionary<string, int> data)
    {
        if (!_isGrowingState && data.ContainsKey(Harvester.HarvestKey))
        {
            var harvestAmount = data[Harvester.HarvestKey];
            var successfullyHarvestedAmo = TryHarvest(harvestAmount, data[Harvester.MaxHarvestedAmountKey] - data[Harvester.HarvestedAmountKey]);
            if (successfullyHarvestedAmo >= 0)
            {
                data[Harvester.HarvestedAmountKey] += successfullyHarvestedAmo;

                if(successfullyHarvestedAmo > 0)
                    _harvestedContainer?.AddResource(_currentResData.ResourceID, successfullyHarvestedAmo);

                _onHarvested.Invoke();
                _onHarvestStateUpdate.Invoke((_currentResData.CurrentValue));
                if (_currentHealth <= 0)
                {
                    StartCoroutine(StartGrowPlant());
                }
            }
        }
    }


    private int TryHarvest(int value, int maxResValueAvailable)
    {
        if(_currentHealth > 0 && maxResValueAvailable > 0)
        {
            var newHealth = Mathf.Clamp(_currentHealth - value, 0, _startHealth);
            
            var newPotentialResAmo = newHealth > 0 ? Mathf.CeilToInt( (newHealth / (float)_startHealth) * _startContainigData.CurrentValue) : 0;
            var curResAmo = _currentResData.CurrentValue;

            var resDelta = curResAmo - newPotentialResAmo;
            if(resDelta <= maxResValueAvailable)
            {
                if (_currentResData.TryChangeValue(-resDelta))
                {
                    _currentHealth = newHealth;
                    return resDelta;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                resDelta = maxResValueAvailable;
                if (_currentResData.TryChangeValue(-resDelta))
                {
                    _currentHealth = Mathf.CeilToInt( (_currentResData.CurrentValue / (float)_startContainigData.CurrentValue) * _startHealth);
                    return resDelta;
                }
                else
                {
                    return -1;
                }
            }

        }
        /*

        if(_currentResData.CurrentValue > 0)
        {
            value = (value > _currentResData.CurrentValue) ? _currentResData.CurrentValue : value;
            return _currentResData.TryChangeValue(-value);
        }*/
        return -1;
    }


    private void ResetData()
    {
        _currentResData.TryChangeValue(_startContainigData.CurrentValue - _currentResData.CurrentValue);
        _currentHealth = _startHealth;
        _onReadynesStateUpdate.Invoke(true);
        //_onHarvestStateUpdate.Invoke(_currentResData.CurrentValue);
    }


    private IEnumerator StartGrowPlant()
    {
        _isGrowingState = true;
        _onReadynesStateUpdate.Invoke(false);
        var timeRemaining = _respawnTime;
        while(timeRemaining > 0)
        {
            yield return null;
            timeRemaining -= Time.deltaTime;
            _onGrowingStateUpdate.Invoke(1 - timeRemaining / _respawnTime);
        }

        _isGrowingState = false;
        ResetData();
    }
}
