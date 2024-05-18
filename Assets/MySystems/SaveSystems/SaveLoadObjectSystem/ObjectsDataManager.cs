using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonLoaderCore;

namespace SaveLoadObjectSystem
{
    public class ObjectsDataManager : MonoBehaviour, IJsonLoader
    {
        [SerializeField] private string _dataContainerName;
        [SerializeField] private ObjectsSpawner _spawner;
        [SerializeField] private ObjectsSaver _saver;

        public string GetDataAsJsonStr()
        {
            return _saver.GetDataAsJsonStr(_dataContainerName);
        }

        public bool IsIgnoredForSaving()
        {
            return false;
        }

        public bool LoadDataFromJsonStr(string jsonStr)
        {
            Debug.Log($"{this}>>>Loading {jsonStr}");
            if(jsonStr != null && jsonStr != "")
            {
                return _spawner.LoadDataFromJsonStr(jsonStr, jsonStr != "");
            }
            Debug.LogWarning($"{this}>>>jsonStr is null or empty");

            return false;
        }
    }


    [System.Serializable]
    public class AnchoredObjectsDataContainer
    {
        public string containerID = "";
        public string[] data = new string[0];

        public void AddData(string newData)
        {
            var lastList = new List<string>(data);
            lastList.Add(newData);
            data = lastList.ToArray();
        }
    }
}
