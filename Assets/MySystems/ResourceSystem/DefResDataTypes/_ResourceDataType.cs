using UnityEngine;
using System;

namespace ResourceSystem
{
    [Serializable]
    public abstract class  ResourceData<ResourceType> where ResourceType : struct, IComparable
    {
        public string ResourceName { get => _resource.name; }
        public string ResourceID { get => _resource.ID; }
        public ResourceType CurrentValue { get => _value; }

        public Sprite ResourceIcon { get => _resource.Icon; }

        [SerializeField] protected ResourceSO _resource;
        [SerializeField] protected ResourceType _value;
        [SerializeField] protected bool _isInfinitySource = false;



        public ResourceData(ResourceSO resource, ResourceType value, bool infSource = false)
        {
            _resource = resource;
            _value = value;
            _isInfinitySource = infSource;
        }


        public void LoadFromSimpleData(SimplResourceData simpleData)
        {
            if(simpleData.id == _resource.ID)
            {
                try
                {
                    _value = ValueFromString(simpleData.value);
                    return;
                }
                catch(System.Exception e)
                {
                    Debug.LogWarning(e);
                }
            }
            _value = default(ResourceType);
        }

        public ResourceType TakeResource()
        {
            var value = _value;
            if (!_isInfinitySource)
                _value = default(ResourceType);

            return value;
        }


        public virtual ResourceData<ResourceType> Clone()
        {
            return CloneWithValue(_value);
        }


        public abstract ResourceData<ResourceType> CloneWithValue(ResourceType value);


        public SimplResourceData GetAsSimpleData()
        {
            
            return new SimplResourceData(_resource.ID, SimplResourceData.ValueAsJsonStr(_value));
        }

        public abstract ResourceType ValueFromString(string str);

    }

    [Serializable]
    public class SimplResourceData
    {
        public string id;
        public string value;

        internal SimplResourceData(string id, string value)
        {
            this.id = id;
            this.value = value;
        }


        internal static SimplResourceData FromJson(string jsonStr)
        {
            return JsonUtility.FromJson<SimplResourceData>(jsonStr);
        }

        internal static string ValueAsJsonStr<ResourceType>(ResourceType value) where ResourceType : struct, IComparable
        {
            var str = JsonUtility.ToJson(value);
            if (str == "{}")
            {
                str = value.ToString();
            }
            Debug.Log($"SRD>>>trying convert value: {value} to string {str}");
            return str;
        }

        internal static ResourceType GetValueFromJsonStr<ResourceType>(string str) where ResourceType : struct, IComparable
        {
            var res = JsonUtility.FromJson<ResourceType>(str);
            Debug.Log($"SRD>>> trying convert {str} to {typeof(ResourceType)} : {res}");
            return res;
        }

        public string GetAsJsonStr()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
