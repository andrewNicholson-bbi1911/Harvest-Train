using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsDataContainer : MonoBehaviour, IDataContainer<ButtonData>
{
    [SerializeField] private string _gameDataField = "buttons";
    [SerializeField] private List<string> _buttonNames;

    private ButtonData _data = null;

    private void Start()
    {
        ForceLoadData();
    }

    public int GetLevelFor(string name)
    {
        if(_data == null)
        {
            ForceLoadData();
        }
        return _data.GetLevelForButton(name);
    }

    public void SetLevel(string name, int value)
    {
        if (_data == null)
        {
            ForceLoadData();
        }
        _data.SetLevelForButton(name, value);
        DataLoader.SaveData(_gameDataField, this);
    }


    public void ForceLoadData()
    {
        var data = DataLoader.GetData<ButtonData>(_gameDataField);
        if(data == null)
        {
            _data = new ButtonData(_buttonNames);
            DataLoader.SaveData(_gameDataField, this);
        }
        else
        {
            LoadData(data);
        }
    }

    public string GetJsonString()
    {
        return JsonUtility.ToJson(_data);
    }

    public void LoadData(ButtonData data) => _data = data;
}


[System.Serializable]
public class ButtonData
{
    [SerializeField] private string[] _names = new string[0];
    [SerializeField] private int[] _levels = new int[0];

    public ButtonData(List<string> names)
    {
        int count = names.Count;
        _names = new string[count];
        _levels = new int[count];
        int i = 0;
        foreach(var name in names)
        {
            _names[i] = name;
            _levels[i] = 0;
            i++;
        }
    }

    public int GetLevelForButton(string name)
    {
        for (int i = 0; i < _names.Length; i++)
        {
            if (_names[i] == name)
            {
                return _levels[i];
            }
        }
        return -1;
    }

    public void SetLevelForButton(string name, int value)
    {
        for(int i = 0; i< _names.Length; i++)
        {
            if(_names[i] == name)
            {
                _levels[i] = value;
                break;
            }
        }
    }
}
