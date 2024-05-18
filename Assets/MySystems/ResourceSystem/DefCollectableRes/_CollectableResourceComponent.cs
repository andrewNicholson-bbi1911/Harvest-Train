using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ResourceSystem
{
    [RequireComponent(typeof(Collider))]
    public abstract class CollectableResourceComponent<ResourceDataType, T> : MonoBehaviour
        where ResourceDataType : ResourceData<T>
        where T : struct, IComparable
    {
        [SerializeField] private List<ResourceDataType> _containedResources = new List<ResourceDataType>();
        [Space]
        [SerializeField] private UnityEvent _onRecourceCollected = new UnityEvent();
        [SerializeField] private bool _destroyOnCollect = true;
        private bool _initDataSet = false;

        public void SetInitData(List<ResourceDataType> resources)
        {
            if (_initDataSet) return;

            _containedResources = resources;
            _initDataSet = true;
            _onRecourceCollected.Invoke();
        }


        private void OnTriggerEnter(Collider other)
        {
            ResourceContainer<ResourceDataType, T> container;
            if(other.TryGetComponent(out container))
            {
                CollectTo(container);
            }
        }

        protected void CollectTo(ResourceContainer<ResourceDataType, T> container)
        {
            foreach(var res in _containedResources)
            {
                container.AddResourceOld(res);
            }

            _onRecourceCollected.Invoke();
            if(_destroyOnCollect) Destroy(gameObject);
        }

        public List<string> GetContainedResourcesID()
        {
            var returnList = new List<string>();
            foreach (var resource in _containedResources)
            {
                returnList.Add(resource.ResourceID);
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
            if (res != default)
            {
                return res.CurrentValue;
            }
            else
            {
                return default;
            }
        }
    }


}

