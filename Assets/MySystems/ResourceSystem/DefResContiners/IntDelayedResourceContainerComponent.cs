using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

namespace ResourceSystem
{
    public class IntDelayedResourceContainerComponent : IntResourceContainer
    {
        [Space]
        [SerializeField] private UnityEvent<ResurceChangingState<int>> _onResourceStartChanging;

        
        protected override async Task _RemoveValue(IntResourceData changingResource, int value)
        {
            var resState = new ResurceChangingState<int>(value);

            _onResourceStartChanging.Invoke(resState);
            var i = 0;
            while (!resState.IsFinished)
            {
                Debug.Log($"{this}>>> still waiting for the reward video finishing");
#if UNITY_EDITOR
                await Task.Delay(250);
                i++;
                if(i > 20)
                {
                    Debug.LogError($"{this}>>>it takes too long to update reward");
                }
#else
                await Task.Yield();
#endif
            }
            Debug.Log($"{this}>>> got resault for the Reward video: {resState.Status}");

            if (resState.Status == TResurceChangingStatus.Failed || resState.Status == TResurceChangingStatus.InProgress || !changingResource.TryChangeValue(-value))
                throw new System.Exception("affter this changing the value become less then 0. " +
                    "It's prohibited in Default IntResourceContainer. " +
                    "You can create other ResourceContainer inherited from " +
                    "ResourceContainer<IntResourceData, int> by yourself " +
                    "if you don't like this limitation");
        }
    }
}
