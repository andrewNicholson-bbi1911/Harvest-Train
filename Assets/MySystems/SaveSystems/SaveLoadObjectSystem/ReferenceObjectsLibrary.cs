using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SaveLoadObjectSystem
{
    [CreateAssetMenu(fileName = "SO/AnchoreObjectDataSystem/Library", menuName = "SO/AnchoreObjectDataSystem/Create New Reference Library")]
    public class ReferenceObjectsLibrary : ScriptableObject
    {
        [SerializeField] private string _libraryID = "new Lib";
        [SerializeField] private List<ReferenceObjectData> _referenceObjects = new List<ReferenceObjectData>();


        public bool CheckLibrary()
        {
            foreach (var obj in _referenceObjects)
            {
                DataMangedObject anchoredObject;
                if (obj.ReferenceID == "")
                {
                    Debug.LogError($"{this}>>> Library contains objects with empty ID");
                    return false;
                }
                if (obj.ReferenceObject == null)
                {
                    Debug.LogError($"{this}>>> Reference object {obj.ReferenceID} has no GameObject");
                    return false;
                }
                else if (!obj.ReferenceObject.TryGetComponent(out anchoredObject))
                {
                    Debug.LogError($"{this}>>> Reference object {obj.ReferenceID} doesn't contain AnchoredObject component");
                    return false;
                }
            }

            return true;
        }

        public bool HasRefID(string id)
        {
            return _referenceObjects.Exists(x => x.ReferenceID == id);
        }

        public GameObject GetReferenceObject(string id)
        {
            var res = _referenceObjects.Find(x => x.ReferenceID == id);
            if (res == null)
                throw new System.NullReferenceException($"{this}>>> Library doesn't have reference object with id {id}");
            return res.ReferenceObject;
        }


    }
}