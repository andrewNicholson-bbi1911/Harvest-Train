using System;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

namespace ResourceSystem
{
    public abstract class ResourceTransferComponent<ResourceDataType, T> : MonoBehaviour
        where ResourceDataType : ResourceData<T>
        where T : struct, IComparable
    {
        [SerializeField] private ResourceContainer<ResourceDataType, T> _sourceContainer;
        [SerializeField] private ResourceContainer<ResourceDataType, T> _transferContainer;
        [SerializeField] private ResourceDataType _transferResourceData;
        //[SerializeField] private bool _transferEntirely;

        [SerializeField] private UnityEvent _onTransferComplited;
        [SerializeField] private UnityEvent _onTransferError;

        public void SetSource(ResourceContainer<ResourceDataType, T> sourceContainer)
        {
            _sourceContainer = sourceContainer;
        }

        public void SetTarget(ResourceContainer<ResourceDataType, T> transferContainer)
        {
            _transferContainer = transferContainer;
        }

        public async void Transfer()
        {
            var data = GetTransferData();
            var transRes = await _TryTransfer(GetTransferData());
            if (!transRes)
            {
                var maxTransRes = await _TryTransfer(GetMaximumPosibleTranserData());

                if (maxTransRes)
                {
                    _onTransferComplited.Invoke();
                    return;
                }
                else
                {
                    _onTransferError.Invoke();
                    return;
                }
            }
            _onTransferComplited.Invoke();
        }

        protected ResourceDataType GetTransferData()
        {
            return _transferResourceData.CloneWithValue(_transferResourceData.CurrentValue) as ResourceDataType;
        }

        protected ResourceDataType GetMaximumPosibleTranserData()
        {
            return _transferResourceData.CloneWithValue(_sourceContainer.GetResourceCurrentValue(_transferResourceData.ResourceID)) as ResourceDataType;
        }

        private async Task<bool> _TryTransfer(ResourceDataType data)
        {
            var res = await _sourceContainer.TryTakeResource(data.ResourceID, data.CurrentValue);

            if (res)
            {
                _transferContainer.AddResourceOld(data);
                return true;
            }
            return false;
        }
    }
}

