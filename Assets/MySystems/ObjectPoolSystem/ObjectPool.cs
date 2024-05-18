using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public string PoolID { get => _poolID; }
    public int ObjectsMaxAmount { get => _pool.Length; }
    [SerializeField] private string _poolID;
    [SerializeField] private Transform _poolRoot;
    [SerializeField] private bool _reanableOnTake = false;
    [SerializeField] private bool _takeIfAllBusy = true;
    [Space]
    [SerializeField] private PoolObject[] _pool;
    private bool[] _freePool;
    private int _currentObjectID = 0;

    public void Init(bool hideOnInit = true)
    {
        _freePool = new bool[_pool.Length];
        for(int i = 0; i<_freePool.Length; i++)
        {
            _freePool[i] = true ;
        }
        if (hideOnInit)
        {
            HidePool();
        }
    }

    public PoolObject GetObject(bool initObject = true)
    {
        var objectsRemaining = _pool.Length;
        while (!_freePool[_currentObjectID] && objectsRemaining > 0)
        {
            _currentObjectID = (_currentObjectID + 1) % _pool.Length;
            objectsRemaining--;
        }

        if(objectsRemaining<=0 && !_takeIfAllBusy)
        {
            throw new System.IndexOutOfRangeException($"{this}>>> each object is busy in this pool and this pool is marked as _takeIfAllBusy = false");
        }
        var obj = _pool[_currentObjectID];
        _freePool[_currentObjectID] = false;
        if (_reanableOnTake)
        {
            obj.gameObject.SetActive(true);
        }
        if (initObject)
        {
            obj.Init(this, _currentObjectID);
        }
        _currentObjectID = (_currentObjectID + 1) % _pool.Length;

        return obj;
    }

    public void HidePool()
    {
        foreach(var obj in _pool)
        {
            obj.gameObject.SetActive(false);
        }
        _currentObjectID = 0;
    }

    public void ReturnObject(PoolObject poolObject)
    {
        poolObject.transform.SetParent(_poolRoot);
        _freePool[poolObject.IndexInPool] = true;
    }

    public List<PoolObject> GetListOfActiveObjects() 
    {
        var list = new List<PoolObject>();

        foreach(var obj in _pool)
        {
            if (obj.isActiveAndEnabled)
            {
                list.Add(obj);
            }
        }

        return list;
    }

}


