using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ResourceSystem;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class CostButton : MonoBehaviour
{
    [SerializeField] private ButtonsDataContainer _dataContainer;
    [SerializeField] private string _buttonName;
    [Space]
    [SerializeField] private UnityEvent<int> _onPressed;
    [SerializeField] private int _argument = 1;
    [SerializeField] private UnityEvent _onErrored;
    [SerializeField] private IntResourceContainer _wallet;
    [Space]
    [SerializeField] private ActionPriceData _costData;
    [Space]
    [SerializeField] private Image _resImage;
    [SerializeField] private TextMeshProUGUI _costValue;
    [SerializeField] private TextMeshProUGUI _lvlTxt;
    private int _currentLevel = 0;


    private void Start()
    {
        _currentLevel = _dataContainer.GetLevelFor(_buttonName);
        UpdateCostUI();
    }


    public async void CallAction()
    {
        var cost = _costData.GetCostDataForLevel(_currentLevel);
        if (await _wallet.TryTakeResource(cost.costResource, cost.costValue))
        {
            _onPressed.Invoke(_argument);
            _currentLevel++;
            _dataContainer.SetLevel(_buttonName, _currentLevel);
        }
        else
        {
            _onErrored.Invoke();
        }

        UpdateCostUI();
    }

    private void UpdateCostUI()
    {
        var newCost = _costData.GetCostDataForLevel(_currentLevel);
        _resImage.sprite = newCost.costResource.Icon;
        _costValue.text = newCost.costValue.ToString();
        if (_lvlTxt != null)
        {
            _lvlTxt.text = $"lvl {_currentLevel}";
        }
    }
}


[System.Serializable]
public struct ActionPriceData
{
    [SerializeField] private ResourceSO _costResource;
    [SerializeField] private CostDataSO _costData;


    public CostData GetCostDataForLevel(int level)
    {
        return new CostData(_costResource, _costData.GetValueForLevel(level));
    }
}

public struct CostData
{
    public ResourceSO costResource;
    public int costValue;

    public CostData(ResourceSO costResource, int costValue)
    {
        this.costResource = costResource;
        this.costValue = costValue;
    }
}
