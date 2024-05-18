using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailwaySwitchCell : RailwayCell
{
    [Space]
    [Header("Switch Data")]
    [SerializeField] private RailwayCell _baseNextCell = null;
    [SerializeField] private GameObject _baseCellModel;
    [Space]
    [SerializeField] private RailwayCell _switchedNextCell = null;
    [SerializeField] private GameObject _switchedCellModel;

    private bool _switched = false;


    private void OnValidate()
    {
        if(_nextCell != null)
        {
            _baseNextCell = _nextCell;
        }
    }


    private void Start()
    {
        UpdateNextCell();
    }


    public void SwitchOnly() => SwitchOnly(!_switched);

    public void SwitchOnly(bool isSwitched)
    {
        _switched = isSwitched;
        UpdateNextCell(false);
    }


    public void Switch(bool isSwitched)
    {
        _switched = isSwitched;
        UpdateNextCell();
    }

    public void Switch() => Switch(!_switched);


    private void UpdateNextCell(bool updateModel = true)
    {
        _nextCell = _switched ? _switchedNextCell : _baseNextCell;
        if (updateModel)
        {
            _baseCellModel.SetActive(!_switched);
            _switchedCellModel.SetActive(_switched);
        }
    }
}
