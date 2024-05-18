using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ResourceSystem;

public class CargoUIIndicator : MonoBehaviour
{
    [SerializeField] private CargoContainer _cargoContainer;
    [SerializeField] private bool _autoUpdate = true;
    [Space]
    [SerializeField] private UnityEvent<int> _onMaxValueUpdated;
    [SerializeField] private UnityEvent<int> _onCurValueUpdated;
    [SerializeField] private UnityEvent<float> _onNormValueUpdated;

    private int _maxValue = 100;
    private int _curValue = 0;
    private float _normValue = 0f;


    private void Update()
    {
        if (_autoUpdate)
        {
            UpdateValues();
        }
    }


    public void SetCargoContainer(CargoContainer newContainer)
    {
        _cargoContainer = newContainer;
    }

    public void UpdateValues()
    {
        _maxValue = _cargoContainer.MaxCargo;
        _curValue = _maxValue - _cargoContainer.AvaliableCargoAmount;
        _normValue = _curValue / (float)_maxValue;

        _onMaxValueUpdated.Invoke(_maxValue);
        _onCurValueUpdated.Invoke(_curValue);
        _onNormValueUpdated.Invoke(_normValue);
    }
}
