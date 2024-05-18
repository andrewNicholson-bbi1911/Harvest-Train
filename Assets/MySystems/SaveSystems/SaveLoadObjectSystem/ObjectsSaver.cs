using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonLoaderCore;

namespace SaveLoadObjectSystem
{
    public class ObjectsSaver : MonoBehaviour
    {
        [SerializeField] private ReferenceObjectsLibrary _savingLibrary = null;
        [SerializeField] private List<DataMangedObject> _anchoredObjects = new List<DataMangedObject>();
        private AnchoredObjectsDataContainer _lastContainer = null;
        private string _lastReadData;

        public void CheckSaver()
        {
            GetDataAsJsonStr("test");
        }

        public string GetDataAsJsonStr(string containerName)
        {
            _UpdateAnchoredObjects();
            var container = _BuildContainer(containerName);
            _lastContainer = container;
            var str = JsonUtility.ToJson(container);
            _lastReadData = str;
            return str;
        }


        private void _UpdateAnchoredObjects()
        {
            var objs = new List<DataMangedObject>(FindObjectsByType<DataMangedObject>(FindObjectsSortMode.None));
            _anchoredObjects = objs.FindAll(x => _savingLibrary.HasRefID(x.RefID));
        }


        private AnchoredObjectsDataContainer _BuildContainer(string name)
        {
            var retCont = new AnchoredObjectsDataContainer();
            retCont.containerID = name;

            foreach (var obj in _anchoredObjects)
            {
                retCont.AddData(obj.GetDataAsJsonStr());
            }

            return retCont;
        }
    }
}