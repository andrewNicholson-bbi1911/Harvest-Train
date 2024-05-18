using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace JsonLoaderCore
{
    public class DataSaver : MonoBehaviour
    {
//        private ExternalSDKDataManager _sdk = new ExternalSDKDataManager();
        [SerializeField] private List<SavingObject> _savingObjects;
        [SerializeField] private UnityEvent _onDataLoaded = new UnityEvent();

        

        private void Start()
        {
            _LoadAllData();
        }

        private void _LoadAllData()
        {
            foreach (var sObject in _savingObjects)
            {
                Debug.Log($"{this}>>> Start loading {sObject.ID} as {sObject.SaveMode}. \n");
                //Debug.Break();

                switch (sObject.SaveMode)
                {
                    case SaveMode.AsOneObject:
                        _LoadObject(sObject);
                        break;
                    case SaveMode.Children:
                        _LoadChildren(sObject);
                        break;
                    case SaveMode.Disable:
                        break;
                    default:
                        _LoadObject(sObject);
                        break;
                }

            }

            _onDataLoaded.Invoke();
        }

        public void SaveAllData()
        {
            throw new System.NotImplementedException("SaveAllData is not implemented yet");
            /*
            foreach (var data in _savingObjects)
            {
                _SaveDataFor(data);
            }
#if !UNITY_EDITOR
            if (_sdk != null)
            {
                _sdk.ForceDataSave();
            }
#endif
            */
        }

        public void SaveDataFor(IJsonLoader jsonLoader)
        {
            var sData = _savingObjects.Find(x => x.JsonLoader == jsonLoader);
            _SaveDataFor(sData);
        }

        private void _SaveDataFor(SavingObject sObject)
        {
            if (sObject != null)
            {
                switch (sObject.SaveMode)
                {
                    case SaveMode.AsOneObject:
                        _SaveAsObject(sObject);
                        break;
                    case SaveMode.Children:
                        _SaveChildren(sObject);
                        break;
                    case SaveMode.Disable:
                        break;
                    default:
                        _SaveAsObject(sObject);
                        break;
                }
            }
        }

        private void _LoadDataTo(SavingObject sObject, string strData)
        {
            var loadSuccess = sObject.JsonLoader.LoadDataFromJsonStr(strData);
            if(loadSuccess == true)
            {
                Debug.Log($"{this}>>>loaded {sObject.ID} with value: {strData}");
            }
            else
            {
                Debug.LogError($"{this}>>>couldn't load to {sObject.ID} data: {strData}");
            }
        }

        private async Task<string> _GetStringDataFor(string id)
        {
#if UNITY_EDITOR
            

            if (PlayerPrefs.HasKey(id))
                return PlayerPrefs.GetString(id);
#else
            if (PlayerPrefs.HasKey(id))
                    return PlayerPrefs.GetString(id);
                    /*
            if(_sdk != null)
            {
                var res = await _sdk.GetStrData(id);
                return res;
            }
            else
            {
                if (PlayerPrefs.HasKey(id))
                    return PlayerPrefs.GetString(id);
            }
            */
#endif

            return null;
        }

        private async void _LoadObject(SavingObject sObject)
        {
            var strData = await _GetStringDataFor(sObject.ID);
            if (strData != null)
            {
                _LoadDataTo(sObject, strData);
            }
            else
            {
                sObject.OnNoData();
            }
            /*
            else
            {
                _SaveDataFor(sObject);
            }*/
        }


        private async void _SaveAsObject(SavingObject sObject)
        {
            var savingStr = sObject.JsonLoader.GetDataAsJsonStr();

#if UNITY_EDITOR
            


            PlayerPrefs.SetString(sObject.ID, savingStr);
#else
            PlayerPrefs.SetString(sObject.ID, savingStr);
            /*
            if(_sdk != null)
            {
                var res = await _sdk.SaveDataStr(sObject.ID, savingStr);
                if (!res)
                {
                    PlayerPrefs.SetString(sObject.ID, savingStr);

                    Debug.LogError($"{this}>>> couldnt save for {sObject.ID}:\n {savingStr}");
                    return;
                }

            }
            else
            {
                PlayerPrefs.SetString(sObject.ID, savingStr);
            }
            */
#endif

            Debug.Log($"{this}>>>saved {sObject.ID} as {savingStr}");
        }


        private void _LoadChildren(SavingObject sObject)
        {
            if (PlayerPrefs.HasKey(sObject.ID))
            {
                var jsonStr = PlayerPrefs.GetString(sObject.ID);
                var simpleListOfIDs = JsonUtility.FromJson<SimpleStringList>(jsonStr);
                List<KeyValuePair<string, string>> childDataList = new List<KeyValuePair<string, string>>();
                foreach (var id in simpleListOfIDs.list)
                {
                    if (PlayerPrefs.HasKey(id))
                    {
                        var dataStr = PlayerPrefs.GetString(id);
                        childDataList.Add(new KeyValuePair<string, string>(id, dataStr));
                    }
                }

                var childArr = childDataList.ToArray();

                _LoadDataTo(sObject, JsonUtility.ToJson(childArr));
            }
            else
            {
                //_SaveDataFor(sObject);
            }
        }

        private void _SaveChildren(SavingObject sObject)
        {
            List<IJsonLoader> jsons = new List<IJsonLoader>();
            jsons = JsonUtility.FromJson<List<IJsonLoader>>(sObject.JsonLoader.GetDataAsJsonStr());
            var childrenKeys = new List<string>();
            foreach(var loader in jsons)
            {
                var childData = JsonUtility.FromJson<KeyValuePair<string, string>>(loader.GetDataAsJsonStr());
                PlayerPrefs.SetString(childData.Key, childData.Value);
                childrenKeys.Add(childData.Key);
                Debug.Log($"saved child of {sObject.ID} - {childData.Key} with value {childData.Value}");
            }

            SimpleStringList sList = new SimpleStringList(childrenKeys);

            PlayerPrefs.SetString(sObject.ID, JsonUtility.ToJson(sList)); 
        }

    }


    [System.Serializable]
    public class SavingObject
    {
        public string ID { get => _savingID; }
        public IJsonLoader JsonLoader { get => _savingObject.GetComponent<IJsonLoader>(); }
        public SaveMode SaveMode { get => _saveMode; }
        [SerializeField] private string _savingID;
        [SerializeField] private GameObject _savingObject;
        [SerializeField] private SaveMode _saveMode = SaveMode.AsOneObject;

        [SerializeField] private UnityEvent _onNoData = new UnityEvent();

        public KeyValuePair<string, IJsonLoader> GetSavingData()
        {
            IJsonLoader loader;
            if (_savingObject.TryGetComponent(out loader))
            {
                return new KeyValuePair<string, IJsonLoader>(_savingID, loader);
            }
            else
            {
                throw new System.NullReferenceException($"connected _savingObject in {this} with id {_savingID} has not IJsonLoader components");
            }
        }

        public void OnNoData() => _onNoData.Invoke();

    }


    [System.Serializable]
    public class SimpleStringList
    {
        public string[] list;


        public SimpleStringList(List<string> list)
        {
            this.list = list.ToArray();
        }
    }


    public enum SaveMode
    {
        AsOneObject,
        Children,
        Disable

    }
}