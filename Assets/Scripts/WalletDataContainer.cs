using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceSystem;

public class WalletDataContainer : MonoBehaviour, IDataContainer<WalletData>
{
    [SerializeField] private string _gameDataField = "wallet";
    [SerializeField] private IntResourceContainer _container;
    [SerializeField] private ResourceSO _moneyRes;
    [SerializeField] private ResourceSO _gemRes;

    private WalletData _data = null;


    private async void Start()
    {
        ForceLoadData();

        SimpleResContainer cont = new SimpleResContainer(
            new List<SimplResourceData>()
            {
                new SimplResourceData(_moneyRes.ID, _data.Money.ToString()),
                new SimplResourceData(_gemRes.ID, _data.Gems.ToString()),
            }
        ); ;

        _container.LoadDataFromJsonStr(JsonUtility.ToJson(cont));
    }


    public void SaveData()
    {
        _data.Money = _container.GetResourceCurrentValue(_moneyRes);
        _data.Gems = _container.GetResourceCurrentValue(_gemRes);
        DataLoader.SaveData(_gameDataField, this);
    }


    public void ForceLoadData()
    {
        var data = DataLoader.GetData<WalletData>(_gameDataField);
        if (data == null)
        {
            _data = new WalletData();
            DataLoader.SaveData(_gameDataField, this);
        }
        else
        {
            LoadData(data);
        }
    }

    public string GetJsonString() => JsonUtility.ToJson(_data);

    public void LoadData(WalletData data)
    {
        _data = data;
    }
}

public class WalletData
{
    public int Money = 0;
    public int Gems = 100;

    public WalletData()
    {
        Money = 0;
        Gems = 100;
    }
}
