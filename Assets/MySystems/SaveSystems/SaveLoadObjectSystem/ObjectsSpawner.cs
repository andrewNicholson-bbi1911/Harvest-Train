using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonLoaderCore;

namespace SaveLoadObjectSystem
{
    public class ObjectsSpawner : MonoBehaviour
    {
        [SerializeField] private ReferenceObjectsLibrary _usingLibrary = null;
        [SerializeField] private List<DataMangedObject> _objectsToDestroy = new List<DataMangedObject>();

        private void Awake()
        {
            _usingLibrary.CheckLibrary();
        }

        public bool LoadDataFromJsonStr(string jsonStr, bool destroyObjects = false)
        {
            Debug.Log($"{this}>>>loading {jsonStr}");
            var data = JsonUtility.FromJson<AnchoredObjectsDataContainer>(jsonStr);

            if (data.containerID == "")
            {
                Debug.Log($"{this}>>>data.containerID is empty of {data}");

                return false;
            }

            if (destroyObjects)
                _DestroyObjects();

            Debug.Log($"{this}>>>loading {data.containerID} with amount of data: {data.data.Length}");

            foreach (var datum in data.data)
            {
                var objectData = JsonUtility.FromJson<AnchoredObjectData>(datum);

                if (!_usingLibrary.HasRefID(objectData.refID))
                {
                    Debug.LogError($"{this}>>>Couldn't find ref object with id {objectData.refID}");
                    continue;
                }

                if (_SpawnObject(objectData.refID, datum))
                {
                    Debug.Log($"{this}>>>Successfully spawned {objectData.refID} with data {datum}");
                }
                else
                {

                    Debug.Log($"{this}>>>Couldn't spawn {objectData.refID} with data {datum}");
                }
            }

            return true;
        }


        private bool _SpawnObject(string refID, string dataStr)
        {
            var newObject = Instantiate(_usingLibrary.GetReferenceObject(refID));
            var anchoredObject = newObject.GetComponent<DataMangedObject>();
            return anchoredObject.LoadDataFromJsonStr(dataStr);
        }

        private void _DestroyObjects()
        {
            
            var destroyingList = new List<DataMangedObject>(FindObjectsByType<DataMangedObject>(FindObjectsSortMode.None));
            destroyingList = destroyingList.FindAll(x => _usingLibrary.HasRefID(x.RefID));

            foreach (var obj in destroyingList)
            {
                DestroyImmediate(obj.gameObject);
            }
            Debug.Log($"{this}>>>Objects Destroyed");
        }
    }
}
