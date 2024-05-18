using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonLoaderCore;
using UnityEngine.Events;

namespace SaveLoadObjectSystem
{
    public class DataMangedObject : MonoBehaviour, IJsonLoader
    {
        public string RefID { get => objectData.refID; }
        private List<IJsonLoader> _dataLoaders = new List<IJsonLoader>();
        private bool _loadersInited = false;

        [SerializeField] private AnchoredObjectData objectData = null;
        [SerializeField] private UnityEvent _onDataLoaded = new UnityEvent();
        private string _lastUpdatedJsonData = "";


        public void InitRefID(string id)
        {
            if (objectData == null || objectData.refID == "")
            {
                objectData = new AnchoredObjectData();
                objectData.refID = id;
                _loadersInited = false;
            }
        }

        public string GetDataAsJsonStr()
        {
            _InitLoaders();
            _UpdateAnchoredData();
            _lastUpdatedJsonData = _BuildDataJsonStr();
            return _lastUpdatedJsonData;
        }


        public bool LoadDataFromJsonStr(string jsonDataStr)
        {
            Debug.Log($"{this}>>>Loading data: {jsonDataStr}");
            try
            {
                _InitLoaders();
                _LoadData(jsonDataStr);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                Destroy(gameObject);
                return false;
            }
            return true;
        }

        public bool IsIgnoredForSaving()
        {
            return false;
        }


        private void _InitLoaders()
        {
            if (_loadersInited) return;
            _dataLoaders = new List<IJsonLoader>(GetComponents<IJsonLoader>());
            _dataLoaders.Remove(this);
            _loadersInited = true;
            Debug.Log($"{this}>>> has {_dataLoaders.Count} data loaders");
        }


        private void _LoadData(string jsonStr)
        {
            var data = JsonUtility.FromJson<AnchoredObjectData>(jsonStr);
            Debug.Log($"{this}>>>Loading {jsonStr} with id {data.refID} and {data.lData.Length}");
            foreach (var dl in _dataLoaders)
            {
                Debug.Log($"{this}>>>loading data to {dl}");
                int dataStrIndex = 0;
                while (dataStrIndex < data.lData.Length && !dl.LoadDataFromJsonStr(data.lData[dataStrIndex]))
                {
                    Debug.Log($"{this}>>>|>>>loading {data.lData[dataStrIndex]}");
                    dataStrIndex++;
                }
            }
            _UpdateAnchoredData();
            _lastUpdatedJsonData = _BuildDataJsonStr();
            _onDataLoaded.Invoke();

        }

        private void _UpdateAnchoredData()
        {
            objectData.Clear();
            foreach (var dl in _dataLoaders)
            {
                objectData.AddLoaderData(dl);
            }
        }

        private string _BuildDataJsonStr()
        {
            return JsonUtility.ToJson(objectData);
        }
    }

    [System.Serializable]
    public class AnchoredObjectData
    {
        public string refID = "";
        [HideInInspector] public string[] lData = new string[0];


        internal void AddLoaderData(IJsonLoader dataLoader)
        {
            var newStrData = dataLoader.GetDataAsJsonStr();
            var dataList = new List<string>(lData);
            dataList.Add(newStrData);
            lData = dataList.ToArray();
        }

        internal void Clear()
        {
            lData = new string[0];
        }
    }
}
