using UnityEngine;


namespace SaveLoadObjectSystem
{
    [System.Serializable]
    public class ReferenceObjectData
    {
        public string ReferenceID { get => _referenceGOPrefab.GetComponent<DataMangedObject>().RefID; }
        public GameObject ReferenceObject { get => _referenceGOPrefab; }

        //[SerializeField] private string _referenceObjectID;
        [SerializeField] private GameObject _referenceGOPrefab;
    }
}