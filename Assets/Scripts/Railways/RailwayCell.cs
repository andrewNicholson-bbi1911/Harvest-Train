using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RailwayCell : MonoBehaviour, ILinkedListObject<RailwayCell> 
{
    [SerializeField] private bool _autoPlace = true;
    [SerializeField] protected RailwayCell _nextCell = null;
    [SerializeField] protected RailwayCell _previouseCell = null;
    [SerializeField] protected UnityEvent<Dictionary<string, int>> _onAction;
    [Space]
    [SerializeField] private UnityEvent<GameObject> _onTrainEntered;
    [SerializeField] private UnityEvent<GameObject> _onTrainExited;
    
    private void OnValidate()
    {
        if (_nextCell != null)
        {
            _nextCell._previouseCell = this;

        }

        if (_autoPlace && _previouseCell != null)
        {
            transform.position = _previouseCell.transform.position + _previouseCell.transform.forward * 1;
        }
    }

#if UNITY_EDITOR
    public void EnableAutoPlace(bool enable) => _autoPlace = enable;


    public virtual void LoadData(ICellInfoContainer infoContainer)
    {
        RailwayCell nCell = infoContainer.GetNextCell(this);
        _nextCell = nCell != null ? nCell : _nextCell;

        RailwayCell pCell = infoContainer.GetPreviouseCell(this);
        _previouseCell = pCell != null ? pCell : _previouseCell;
    }
#endif



    public Vector3 GetDirection() => transform.forward;

    public RailwayCell GetNextObject() => _nextCell;

    public RailwayCell GetPreviouseObject() => _previouseCell;

    public void DoAction(Dictionary<string, int> values)
    {
        Debug.Log($"{this}>>> doing action with values:");
        foreach(var keyPair in values)
        {
            Debug.Log($"{keyPair.Key}:{keyPair.Value}");

        }
        _onAction.Invoke(values);
    }

    public virtual void TrainEnter(GameObject train)
    { 
        Debug.Log($"{this}>>>{train} entered");
        _onTrainEntered.Invoke(train);
    }

    public virtual void TrainExit(GameObject train)
    {
        Debug.Log($"{this}>>>{train} exited");
        _onTrainExited.Invoke(train);
    }

    private void OnDrawGizmos()
    {
        
        if(_nextCell != null)
        {
            Gizmos.color = Color.green * 0.5f + Color.black;
            Gizmos.DrawLine(transform.position - transform.right * 0.1f + transform.up * 0.1f, _nextCell.transform.position);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + transform.forward  + transform.up / 3, Vector3.one * 0.5f);
        }

        if (_previouseCell != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position + transform.right * 0.1f + transform.up * 0.1f, _previouseCell.transform.position);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position + transform.up / 3, Vector3.one * 0.4f);
        }
    }

    private void OnDrawGizmosSelected()
    {

        if (_nextCell != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(_nextCell.transform.position - (_nextCell.transform.position - transform.position) / 2, _nextCell.transform.lossyScale * 0.4f); ;
        }

        if (_previouseCell != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(_previouseCell.transform.position + (transform.position - _previouseCell.transform.position) / 2, _previouseCell.transform.lossyScale * 0.7f * 0.4f) ;
        }
    }
}