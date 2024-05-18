using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ResourceSystem;

public class MergingTrain : Train<MergingWagon>
{
    [SerializeField] private Component _dataContainer;
    private IMergingDataContainer _mergingData = null;

    public List<int> Levels { get => _levels; }
    public bool CanBeMerged
    {
        get
        {
            if(_levels.Count <= 0)
            {
                return false;
            }

            for(int i = 1; i <= _levels[0]; i++)
            {
                if (_levels.FindAll(x => x == i).Count > 1)
                    return true;
            }
            return false;
        }
    }
    [Space]
    [SerializeField] private List<int> _levels = new();
    [SerializeField] private int _firstLeveledWagonIndex = 1;
    [SerializeField] private List<int> _mergedIndexes = new();
    [Space]
    [Header("Extra data")]
    [SerializeField] private IntResourceContainer _extraDataContainer;
    [SerializeField] private ResourceSO _speedResource;

    public UnityAction onMerged;
    public UnityAction onWagonsChanged;


    private void OnValidate()
    {
        base.OnValidate();

        if(!(_dataContainer is IMergingDataContainer))
        {
            Debug.LogWarning($"{this}>>>{_dataContainer} is NOT IMergingDataContainer");
            _dataContainer = null;
        }

    }


    protected override void Start()
    {
        _mergingData = _dataContainer as IMergingDataContainer;
        _levels = _mergingData.GetLevels();

        base.Start();

        for (int i = _firstLeveledWagonIndex; i > 0; i--)
        {
            AddWagon();
        }


        foreach(var level in _levels)
        {
            if(level > 0)
            {
                AddWagon();
            }
        }

        UpdateAllLevels();

        InitValues();
    }


    public void AddNewWagon(int level = 1)
    {
        AddWagon();
        _levels.Add(level);
        _levels.Sort((a, b) => b - a);
        _levels.RemoveAll(x => x <= 0);
        UpdateAllLevels();

        _mergingData.UpdateLevels(_levels);
    }


    public void Merge(int amount = 1)
    {
        while(amount > 0)
        {
            if (CanBeMerged)
            {
                StopCoroutine(DelayMerging());

                for (int i = _levels[0]; i >= 1; i--)
                {

                    var inexesOfLevelI = _levels.FindAll(x => x == i);
                    if (inexesOfLevelI.Count > 1)
                    {

                        var mergingLevelIndex = _levels.FindIndex(0, x => x == i);

                        _mergedIndexes.Add(mergingLevelIndex);

                        _levels[mergingLevelIndex]++;
                        _levels.RemoveAt(mergingLevelIndex + 1);
                        _levels.Sort((a, b) => b - a);

                        for (int mergingWagonIndex = mergingLevelIndex + _mergedIndexes.Count + _firstLeveledWagonIndex; mergingWagonIndex < _wagons.Count; mergingWagonIndex++)
                        {
                            _wagons[mergingWagonIndex].IncreaseMergingSpeed();
                        }

                        Debug.Log($"can be merged wagons with lvl {i}, starts index = {inexesOfLevelI[0]}");
                        //Debug.Log($"can be merged wagons with lvl {i}");
                        break;

                    }

                }

                onWagonsChanged?.Invoke();
                onMerged?.Invoke();

                StartCoroutine(DelayMerging());
            }
            amount--;
        }

        _mergingData.UpdateLevels(_levels);
    }


    public void UpdateAllLevels()
    {
        int wagonIndex = 0;
        int removeWagonsAmount = 0;

        foreach(var wagon in _wagons)
        {
            if (wagonIndex < _firstLeveledWagonIndex)
            {

            }
            else
            {
                var levelIndex = wagonIndex - _firstLeveledWagonIndex;
                if (levelIndex < _levels.Count && _levels[levelIndex] > 0)
                {
                    wagon.SetLevel(_levels[levelIndex], _mergedIndexes.Contains(levelIndex));
                }
                else
                {
                    removeWagonsAmount++;
                }
                wagon.StopMergingState();
            }
            wagonIndex++;
        }

        for(int j = 0; j < removeWagonsAmount; j++)
        {
            RemoveWagon();
        }

        _mergedIndexes.Clear();
        UpdateWagons();

        onWagonsChanged?.Invoke();

    }


    private IEnumerator DelayMerging()
    {

        var timeRemaining = (_relatedSpeed > 0.01f) ? (1f / _relatedSpeed) : 100f;
        while(timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        UpdateAllLevels();
    }

    public void InitValues()
    {
        var speed = _extraDataContainer.GetResourceCurrentValue(_speedResource);
        _currentSpeed = _relatedSpeed * ( speed / 100f);
    }

    
}


interface IMergingDataContainer
{
    public List<int> GetLevels();
    public void UpdateLevels(List<int> levels);
}