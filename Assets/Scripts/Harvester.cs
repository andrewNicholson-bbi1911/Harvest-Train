using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Harvester : MonoBehaviour
{
    [HideInInspector][SerializeField] private float _harvestBasePeriod = 0.7f;
    [SerializeField] private int _startWagonIndex = 2;
    public const string HarvestKey = "harvest<int>";
    public const string HarvestedAmountKey = "harvestedAmo";
    public const string MaxHarvestedAmountKey = "maxHarvestedAmo";

    [SerializeField] private MergingTrain _train;
    [SerializeField] private CargoContainer _cargoContainer = null;
    [SerializeField] private List<RailwayCell> _harvestingCells = new();
    private RailwayCell _lastStartCell = null;
    private Dictionary<string, int> harvestActionData = new Dictionary<string, int>() { { HarvestKey, 0 } };

    [SerializeField]private bool _harvestingEnabled = true;

    /*
    private void Start()
    {
        StartCoroutine(StartHarvest());
    }*/


    public void EnableHarvesting(bool enable)
    {
        _harvestingEnabled = enable;
    }


    public void Harvest()
    {
        if (!_harvestingEnabled) return;


        //var harvestingCell = _harvestingCells;
        int i = _startWagonIndex;
        harvestActionData[HarvestedAmountKey] = 0;
        harvestActionData[MaxHarvestedAmountKey] = _cargoContainer.AvaliableCargoAmount;


        foreach (var lvl in _train.Levels)
        {
            if (_harvestingCells[i] != null)
            {
                //harvestingCell = harvestingCell.GetPreviouseObject();
                harvestActionData[HarvestKey] = GetStrengthForLevel(lvl);

                _harvestingCells[i].DoAction(harvestActionData);
            }
            i++;
        }

    }


    public void UpdateHarvestingCells(List<RailwayCell> cells)
    {
        if (_lastStartCell != cells[0])
        {
            _harvestingCells = cells;
            _lastStartCell = cells[0];
            Harvest();
        }

    } 


    private int GetStrengthForLevel(int level)
    {
        //return (Mathf.RoundToInt( Mathf.Pow(2, level)) - 1) * level;
        return Mathf.RoundToInt( Mathf.Pow(3, level - 1));
    }


    /*
    private IEnumerator StartHarvest()
    {
        while (true)
        {
            Harvest();
            yield return new WaitForSeconds(_harvestBasePeriod);
            
        }
    }
    */
}
