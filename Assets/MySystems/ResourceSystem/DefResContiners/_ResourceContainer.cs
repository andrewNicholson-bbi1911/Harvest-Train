using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using JsonLoaderCore;
using System.Threading.Tasks;

namespace ResourceSystem
{
    public abstract class ResourceContainer<RD, T> : MonoBehaviour, IJsonLoader
        where RD : ResourceData<T>
        where T : struct, IComparable
    {
        [HideInInspector] private bool _isIgnoredForDataSaving = false;
        [SerializeField] protected List<RD> _containedResources = new List<RD>();
        [SerializeField] private List<ResourseRedirectionData<ResourceContainer<RD, T>, RD, T>> _redirectionResourcesData = new List<ResourseRedirectionData<ResourceContainer<RD, T>, RD, T>>();
        [Space]
        [SerializeField] protected UnityEvent<T, string> _onRecourceValueChanged = new UnityEvent<T, string>();
        [SerializeField][Min(0)] private int _saveHistoryAmount = 0;

        public HystoryChangeData<T> LastChange
        {
            get
            {
                if(_changeHystory.Count <= 0) return default;
                return _changeHystory[0];
            }
        }

        private bool _initDataSet = false;

        protected List<HystoryChangeData<T>> _changeHystory = new List<HystoryChangeData<T>>();
        private int _lastHistoryID = 0;

        public void ClearInitData()
        {
            _initDataSet = false;
        }

        public void SetInitData(List<RD> resources)
        {
            if (_initDataSet) return;

            _containedResources = resources;
            _initDataSet = true;
            foreach(var res in _containedResources)
            {
                _onRecourceValueChanged.Invoke(res.CurrentValue,res.ResourceID);
            }
        }

        public void AddResource(ResourceSO resource, T value) => AddResource(resource.ID, value);
        public void AddResource(string id, T value)
        {
            var changingResource = _containedResources.Find(x => x.ResourceID == id);
            if (changingResource != null)
            {
                _AddResource(changingResource, value);
                UpdateHystory(new HystoryChangeData<T>(_lastHistoryID, changingResource.ResourceID, value, true, changingResource.ResourceIcon));
                _onRecourceValueChanged.Invoke(changingResource.CurrentValue, changingResource.ResourceID);
            }
            else
            {
                var redirCont = _redirectionResourcesData.Find(x => x.ResourceID == id);
                if (redirCont != null)
                {
                    redirCont.GetRedirectionContainer().AddResource(id, value);
                }
            }
        }

        [Obsolete]
        public void AddResourceOld(RD resourceData)
        {
            var changingResource = _containedResources.Find(x => x.ResourceID == resourceData.ResourceID);
            if (changingResource != null)
            {
                var addingValue = resourceData.TakeResource();
                _AddResource(changingResource, addingValue);
                UpdateHystory(new HystoryChangeData<T>(_lastHistoryID, changingResource.ResourceID, addingValue, true, changingResource.ResourceIcon));
                _onRecourceValueChanged.Invoke(changingResource.CurrentValue, changingResource.ResourceID);
            }
            else
            {
                var redirCont = _redirectionResourcesData.Find(x => x.ResourceID == resourceData.ResourceID);
                if(redirCont != null)
                {
                    redirCont.GetRedirectionContainer().AddResourceOld(resourceData);
                }
            }
        }


        public async Task<bool> TryTakeResource(ResourceSO resource, T value)
        {
            return await TryTakeResource(resource.ID, value);
        }


        public async Task<bool> TryTakeResource(string resourceID, T value)
        {
            var usingResource = _containedResources.Find(x => x.ResourceID == resourceID);
            if (usingResource != null)
            {
                if (usingResource.CurrentValue.CompareTo(value) >= 0)
                {
                    try
                    {
                        await _RemoveValue(usingResource, value);
                        UpdateHystory(new HystoryChangeData<T>(_lastHistoryID, usingResource.ResourceID, value, false, usingResource.ResourceIcon));
                        _onRecourceValueChanged.Invoke(usingResource.CurrentValue, usingResource.ResourceID);
                        return true;
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning(e);
                    }
                }
            }
            else
            {
                var redirCont = _redirectionResourcesData.Find(x => x.ResourceID == resourceID);
                if (redirCont != null)
                {
                    var r =  await redirCont.GetRedirectionContainer().TryTakeResource(resourceID, value);
                    return r;
                }
            }
            return false;
        }


        public List<string> GetContainedResourcesID()
        {
            var returnList = new List<string>();
            foreach(var resource in _containedResources)
            {
                returnList.Add(resource.ResourceID);
            }
            foreach(var redirDatum in _redirectionResourcesData)
            {
                returnList.Add(redirDatum.ResourceID);
            }
            return returnList;
        }


        public T GetResourceCurrentValue(ResourceSO resource)
        {
            return GetResourceCurrentValue(resource.ID);
        }


        public T GetResourceCurrentValue(string resourceID)
        {
            var res = _containedResources.Find(x => x.ResourceID == resourceID);
            if(res != default)
            {
                return res.CurrentValue;
            }
            else
            {
                var redirCont = _redirectionResourcesData.Find(x => x.ResourceID == resourceID);
                if (redirCont != null)
                {
                    var r = redirCont.GetRedirectionContainer().GetResourceCurrentValue(resourceID);
                    return r;
                }
                return default;
            }
            
        }


        protected abstract Task _AddResource(RD changingResource, T value);


        protected abstract Task _RemoveValue(RD changingResource, T value);


        protected void UpdateHystory(HystoryChangeData<T> newData)
        {
            _changeHystory.Insert(0, newData);
            while(_changeHystory.Count > _saveHistoryAmount)
            {
                _changeHystory.RemoveAt(_changeHystory.Count - 1);
            }
            _lastHistoryID++;

        }


        public virtual bool LoadDataFromJsonStr(string jsonStr)
        {
            Debug.Log($"{this}>>>Loading {jsonStr}");
            var simpleResources = JsonUtility.FromJson<SimpleResContainer>(jsonStr);
            if(simpleResources.resources == null)
            {
                return false;
            }

            Debug.Log($"{this}>>>built {simpleResources} with {simpleResources.resources.Length} resources: {simpleResources.resources}");
            var reses = new List<RD>();
            foreach (var sres in simpleResources.resources)
            {
                var res = _containedResources.Find(x => x.ResourceID == sres.id);
                if (res != null)
                {
                    Debug.Log($"{this}>>>found res with id {sres.id}");
                    res.LoadFromSimpleData(sres);
                    reses.Add(res);
                }
                else
                {
                    Debug.Log($"{this}>>>couldn't find ID {sres.id}");
                }
            }

            _initDataSet = false;

            if (reses.Count != _containedResources.Count)
            {
                SetInitData(_containedResources);
//                FindObjectOfType<DataSaver>().SaveDataFor(this);
                Debug.Log($"{this}>>>Successfully loaded data:\n{jsonStr}");
                return false;
            }
            else
            {
                SetInitData(new List<RD>(reses));
                Debug.Log($"{this}>>>UNsuccessfully loaded data:\n{jsonStr}");
                return true;
            }

        }


        public virtual string GetDataAsJsonStr()
        {
            var data = new List<SimplResourceData>();

            foreach (var res in _containedResources)
            {
                var addingData = res.GetAsSimpleData();
                data.Add(addingData);
                //Debug.Log($"{this}>>>added {addingData} : {addingData.id} and {addingData.value}");
            }

            var simpleResContainer = new SimpleResContainer(data);
            var jsonStr = JsonUtility.ToJson(simpleResContainer);
            //Debug.Log($"{this}>>>got {jsonStr}");

            return jsonStr;
        }

        public void SubscribeOnOnValueChangedEvent(UnityAction<T, string> action)
        {
            _onRecourceValueChanged.AddListener(action);
        }

        public void RemoveOnOnValueChangedEvent(UnityAction<T, string> action)
        {
            _onRecourceValueChanged.RemoveListener(action);
        }

        public bool IsIgnoredForSaving()
        {
            return _isIgnoredForDataSaving;
        }
    }


    [Serializable]
    public class SimpleResContainer
    {
        public SimplResourceData[] resources = null;


        public SimpleResContainer(List<SimplResourceData> resources)
        {
            this.resources = resources.ToArray();
        }
    }


    public struct HystoryChangeData<T> where T : struct, IComparable
    {
        public int HistoryId { get => _historyId; }
        public string ID { get => _resourceID; }
        public Sprite Sprite { get => _changeSprite; }
        public T ChangeValue { get => _value; }
        public bool IsPositive { get => _isPositive; }
        public DateTime Time { get => _time; }

        private int _historyId;
        private string _resourceID;
        private T _value;
        private bool _isPositive;
        private DateTime _time;
        private Sprite _changeSprite;


        public HystoryChangeData(int historyID, string id, T signedChangeValue, bool isPositive, Sprite changeSprite = null)
        {
            _historyId = historyID;
            _resourceID = id;
            _value = signedChangeValue;
            _isPositive = isPositive;
            _time = DateTime.Now;
            _changeSprite = changeSprite;
        }
    }

    public delegate void defaultVoidDelegate();

    [Serializable]
    public class ResourseRedirectionData<RC, RD, RT>
        where RD: ResourceData<RT>
        where RC : ResourceContainer<RD, RT>
        where RT : struct, IComparable
    {
        public string ResourceID { get => _resource.ID; }
        [SerializeField] private ResourceSO _resource = null;
        [SerializeField] private RC _redirectionContainer = null;

        public RC GetRedirectionContainer() => _redirectionContainer;
    }
}




