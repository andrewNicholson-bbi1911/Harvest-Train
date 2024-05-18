using UnityEngine;

namespace JsonLoaderCore
{
    public interface IJsonLoader
    {
        public bool IsIgnoredForSaving();
        public bool LoadDataFromJsonStr(string jsonStr);
        public string GetDataAsJsonStr();
    }
}

