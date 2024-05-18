using UnityEngine;
using System.Collections.Generic;


namespace ResourceSystem
{
    [CreateAssetMenu(fileName = "SO/Resource", menuName = "SO/Create New Resource")]
    public class ResourceSO : ScriptableObject
    {
        public string Name { get => _resourceNameStr; }
        public string ID { get => _resourceGameID; }

        [SerializeField] private string _resourceNameStr;

        //[SerializeField] private LocalizedNames _resourceName;
        [SerializeField] private string _resourceGameID;
        public Sprite Icon;
    }


    [System.Serializable]
    public class LocalizedNames
    {
        [SerializeField] public SystemLanguage defaultLanguage = SystemLanguage.Russian;
        [SerializeField] public List<LocalizedNameData> _names = new List<LocalizedNameData>();

        public string GetString()
        {
            var lang = _GetLanguage();
            var data = _names.Find(x => x.language == lang);
            if(data == null)
            {
                data = _names.Find(x => x.language == defaultLanguage);
                if (data == null)
                {
                    return "";
                }
            }
            return data.strValue;
        }

        public SystemLanguage _GetLanguage()
        {
            return defaultLanguage;
        }

    }


    [System.Serializable]
    public class LocalizedNameData
    {
        [SerializeField] public SystemLanguage language;
        [SerializeField] public string strValue;
    }
}

