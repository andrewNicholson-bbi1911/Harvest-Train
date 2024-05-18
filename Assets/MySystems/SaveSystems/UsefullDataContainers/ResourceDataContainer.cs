using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using ResourceSystem;

public class ResourceDataContainer : MonoBehaviour, IDataContainer<ResourcesData>
{
    [SerializeField] private string _gameDataField = "wallet";
    [SerializeField] private IntResourceContainer _container;
    public UnityAction<int, string> onStatsChanged = null;
    private ResourcesData _data = null;


    private async void Start()
    {
        _container.SubscribeOnOnValueChangedEvent(onStatsChanged);
        ForceLoadData();
    }

    public int GetResourceValue(ResourceSO resource)
    {
        if(_data == null)
        {
            ForceLoadData();
        }

        return _container.GetResourceCurrentValue(resource);
    }

    public async void ChangeResourceValue(ResourceSO resource, int value)
    {
        if(_data == null)
        {
            ForceLoadData();
        }

        if(value >= 0)
        {
            _container.AddResource(resource, value);
            SaveData();

        }
        else
        {
            value *= -1;
            value = Mathf.Min(value, _container.GetResourceCurrentValue(resource));
            var res = await _container.TryTakeResource(resource, value);
            if (res)
            {
                SaveData();
            }
        }
    }

    public void SaveData()
    {
        _data.strData = _container.GetDataAsJsonStr();
        DataLoader.SaveData(_gameDataField, this);
    }


    public void ForceLoadData()
    {
        var data = DataLoader.GetData<ResourcesData>(_gameDataField);
        if (data == null)
        {
            _data = new ResourcesData();

            _data.strData = _container.GetDataAsJsonStr();

            DataLoader.SaveData(_gameDataField, this);
        }
        else
        {
            LoadData(data);
        }
    }

    public string GetJsonString() => JsonUtility.ToJson(_data);

    public void LoadData(ResourcesData data)
    {
        _data = data;
        _container.LoadDataFromJsonStr(_data.strData);
    }
}


[System.Serializable]
public class ResourcesData
{
    public string strData = "";
    public ResourcesData()
    {
        strData = "";
    }
}