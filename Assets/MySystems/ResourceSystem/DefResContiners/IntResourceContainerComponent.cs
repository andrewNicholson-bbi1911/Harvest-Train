using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResourceSystem
{
    public class IntResourceContainer : ResourceContainer<IntResourceData, int>
    {
        //[SerializeField] private List<int> _ignoringValues = new List<int>();
        protected override Task _AddResource(IntResourceData changingResource, int value)
        {
            if (!changingResource.TryChangeValue(value))
                throw new System.Exception("affter this addition the value become less then 0. " +
                    "It's prohibited in Default IntResourceContainer. " +
                    "You can create other ResourceContainer inherited from " +
                    "ResourceContainer<IntResourceData, int> by yourself " +
                    "if you don't like this limitation");
            return Task.CompletedTask;
        }

        protected override Task _RemoveValue(IntResourceData changingResource, int value)
        {
            if (!changingResource.TryChangeValue(-value))
                throw new System.Exception("affter this changing the value become less then 0. " +
                    "It's prohibited in Default IntResourceContainer. " +
                    "You can create other ResourceContainer inherited from " +
                    "ResourceContainer<IntResourceData, int> by yourself " +
                    "if you don't like this limitation");
            return Task.CompletedTask;
        }
    }
}

