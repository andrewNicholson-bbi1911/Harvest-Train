using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ResourceSystem;

public class CargoContainer : IntResourceContainer
{
    public int MaxCargo { get => _maxCargo; }
    public int AvaliableCargoAmount {
        get
        {
            var busySpace = 0;
            foreach(var res in _containedResources)
            {
                busySpace += res.CurrentValue;
            }
            return _maxCargo - busySpace;
        }
    }

    [Space]
    [Header("Cargo values")]
    [SerializeField] private Component dataContainer;
    private ICargoDataContainer _cargoContiner;
    [Space]
    [SerializeField] private int _maxCargo = 100;
    [SerializeField] private int _cargoLevel = 0;
    [SerializeField] private CargoAmountIncreasingPlan _cargoPlan;
    [SerializeField] private UnityEvent<float> _onCargoAmountChanged;


    private void OnValidate()
    {
        if (!(dataContainer is ICargoDataContainer))
        {
            dataContainer = null;
        }
    }


    private void Start()
    {
        _cargoContiner = dataContainer as ICargoDataContainer;
        _onRecourceValueChanged.AddListener(UpdateNormValue);
        _cargoLevel = _cargoContiner.GetCargoLevel();

        UpdateNormValue();
        UpdateMaxCargoAmount();
    }


    public void IncrieaceMaxCargoAmount()
    {
        _cargoLevel++;
        _cargoContiner.UpdateCargoLevel(_cargoLevel);
        UpdateMaxCargoAmount();
    }


    private void UpdateNormValue(int val = 0,string str = "")
    {
        var busySpace = 0;
        foreach (var res in _containedResources)
        {
            busySpace += res.CurrentValue;
        }
        _onCargoAmountChanged.Invoke(busySpace / (float)_maxCargo);
    }


    private void UpdateMaxCargoAmount()
    {
        _maxCargo = _cargoPlan.GetMaxAmountForLevel(_cargoLevel);
    }
}


[System.Serializable]
public struct CargoAmountIncreasingPlan
{
    [SerializeField] private int baseIncreaser;
    [SerializeField] private int[] amountPlan;

    public int GetMaxAmountForLevel(int level)
    {
        if(level >= amountPlan.Length)
        {
            return amountPlan[amountPlan.Length - 1] + baseIncreaser * (level - amountPlan.Length + 1);
        }
        else
        {
            return amountPlan[level];
        }
    }
}


public interface ICargoDataContainer
{
    int GetCargoLevel();
    void UpdateCargoLevel(int level);
}