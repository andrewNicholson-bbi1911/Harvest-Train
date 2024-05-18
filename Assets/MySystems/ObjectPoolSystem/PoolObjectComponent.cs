using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolObject : MonoBehaviour
{
    public int IndexInPool { get => _indexInPool; }
    private ObjectPool _parentPool = null;
    private int _indexInPool;

    [SerializeField] private Component[] _dataComponents = new Component[0];
    [HideInInspector][SerializeField] private int _componentsAmount = 0;
    [SerializeField] private UnityEvent _onInit;
    [SerializeField] private UnityEvent _onDisable;
    [SerializeField] private bool _callInitOnEnable = true;

    public void Init(ObjectPool objectPool, int indexInPool)
    {
        //_onDisable.Invoke();
        _parentPool = objectPool;
        _indexInPool = indexInPool;
        _onInit.Invoke();
    }

    private void OnEnable()
    {
        if(_callInitOnEnable)
            Init(_parentPool, _indexInPool);
    }

    private void OnDisable()
    {
        _onDisable.Invoke();
    }


    public T GetDataComponent<T>() where T : Component
    {
        for(int i = 0; i < _componentsAmount; i++)
        {
            if (_dataComponents[i] is T)
                return _dataComponents[i] as T;
        }

        return null;
    }

    public void DisableAfterSeconds(float seconds)
    {
        StartCoroutine(HideAfter(seconds));
    }

    public void HideAndDisable()
    {
        _parentPool.ReturnObject(this);
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        _componentsAmount = _dataComponents.Length;
    }

    private IEnumerator HideAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false) ;
    }
}
