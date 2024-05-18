using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsDM : MonoBehaviour, IDataManager
{
    public bool IsActive() => true;


    public void InitFields(string[] fields)
    {
        foreach(var field in fields)
        {
            if (!PlayerPrefs.HasKey(field))
            {
                PlayerPrefs.SetString(field, "");
            }
        }
    }


    public string GetJsonData(string field)
    {
        string dataStr = PlayerPrefs.GetString(field);

        return dataStr;
    }


    public void SaveData(string field, string jsonDataStr)
    {
        PlayerPrefs.SetString(field, jsonDataStr);
    }
}
