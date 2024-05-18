using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataLoader : MonoBehaviour
{
    private static GameDataContainer _DataContainer;
    [SerializeField] private GameDataFields _fields;
    [SerializeField] private GameObject _defaultDMObject;

    private IDataManager _defaultDataManager;


    private void OnValidate()
    {
        IDataManager dm;
        if (_defaultDMObject != null && !_defaultDMObject.TryGetComponent(out dm))
        {
            Debug.LogWarning($"{this}>>> {_defaultDMObject} doesnt contain components realising IDataManager interface");
            _defaultDMObject = null;
        }
    }


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(_defaultDMObject.gameObject != this)
        {
            DontDestroyOnLoad(_defaultDMObject);
        }
        LoadDataLoader();
        LoadData();
    }


    protected virtual void LoadDataLoader()
    {
        IDataManager dm;
        _defaultDMObject.TryGetComponent(out dm);

        if (dm != null && dm.IsActive())
        {
            _defaultDataManager = dm;
        }
    }


    private void LoadData()
    {
        _DataContainer = new GameDataContainer(_fields.DataFields, _defaultDataManager);
    }


    public static void SaveData<Data>(string field, IDataContainer<Data> dataContainer) where Data : class
    {
        var str = dataContainer.GetJsonString();
        Debug.Log($"SaveSyste>>> Saved {field} as:\n {str}");
        _DataContainer.SaveJsonData(field, str);
    }


    public static DataType GetData<DataType>(string field) where DataType : class
    {
        var str = _DataContainer.GetJsonData(field);
        if (str == "")
        {
            return null;
        }
        Debug.Log($"SaveSyste>>> Loaded {field} as:\n {str}");
        return JsonUtility.FromJson<DataType>(str);
    }
}


public interface IDataManager
{
    bool IsActive();
    void InitFields(string[] fields);
    string GetJsonData(string field);
    void SaveData(string field, string jsonDataStr);
}


public class GameDataContainer
{
    private IDataManager _dataManager;

    public GameDataContainer(string[] loadingFields, IDataManager dataManager)
    {
        dataManager.InitFields(loadingFields);
        _dataManager = dataManager;
    }

    public string GetJsonData(string field)
    {
        return _dataManager.GetJsonData(field);
    }

    public void SaveJsonData(string field, string data)
    {
        _dataManager.SaveData(field, data);
    }
}