using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDataContainer : MonoBehaviour, IDataContainer<TrainData>, IMergingDataContainer, ICargoDataContainer
{
    private TrainData Data {
        get
        {
            if(_data == null)
            {
                ForceLoadData();
            }

            return _data;
        }
    }

    [SerializeField] private string _gameDataField = "train_1";
    private TrainData _data = null;


    public List<int> GetLevels() => Data.Levels;
    public int GetCargoLevel() => Data.CargoLevel;


    public void UpdateLevels(List<int> levels)
    {
        Data.UpdateLevels(levels);
        DataLoader.SaveData(_gameDataField, this);
    }


    public void UpdateCargoLevel(int level)
    {
        Data.UpdateCargoLevel(level);
        DataLoader.SaveData(_gameDataField, this);

    }


    public void ForceLoadData()
    {
        var data = DataLoader.GetData<TrainData>(_gameDataField);
        if (data == null)
        {
            _data = new TrainData();
            DataLoader.SaveData(_gameDataField, this);
        }
        else
        {
            LoadData(data);
        }
    }


    public string GetJsonString() => JsonUtility.ToJson(_data);


    public void LoadData(TrainData data) => _data = data;

    
}

[System.Serializable]
public class TrainData
{
    public List<int> Levels { get => new List<int>(_levels); }
    public int CargoLevel => _cargoLevel;

    [SerializeField] private int[] _levels = { 1 };
    [SerializeField] private int _cargoLevel = 0;

    public TrainData()
    {
        _levels = new int[1]{ 1 };
        _cargoLevel = 0;
    }

    public void UpdateLevels(List<int> levels)
    {
        _levels = new int[levels.Count];
        levels.CopyTo(_levels);
    }

    public void UpdateCargoLevel(int lvl) => _cargoLevel = lvl;
}