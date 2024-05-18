using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class Train : Train<Wagon>
{
   
}

public abstract class Train<WagonType> : Wagon where WagonType : Wagon
{
    public bool WagonCanBeAdded { get => _wagons.Count < _maxWagonsAwailable; }
    public bool _speedUp = false;

    public int TotalWagonsInPool { get => _wagonsPool.ObjectsMaxAmount; }
    [SerializeField] protected int _maxWagonsAwailable = 10;
    [SerializeField] protected float _relatedSpeed = 2f;
    protected float _currentSpeed = 2f;
    [SerializeField] private float _speedUpXSpeed = 2f;
    [SerializeField] protected float _defaultWagonSize = 1f;
    [SerializeField] private ObjectPool _wagonsPool;
    [SerializeField] private UnityEvent<List<RailwayCell>> _onCellUpdated;
    [SerializeField] private UnityEvent<bool> _onMovmentEnabled;

    [Space]
    [SerializeField]private List<RailwayCell> _passedCells = new();

    protected List<WagonType> _wagons = new();
    private float _lastNormalizedMovmentValue = 0;
    protected float _distanceBetweenNextCells = 0;

    private bool _movmentEnabled = true;

    protected void OnValidate()
    {
        if (_currentCell != null)
        {
            HardPlaceToActualCell();
        }
        InitPassedCells();
        _currentSpeed = _relatedSpeed;
    }

    public void SpeedUp()
    {
        _speedUp = true;
    }

    protected virtual void Start()
    {
        SetCurrentCell(_currentCell);
        _wagonsPool.Init(true);
        UpdateWagons();
        _onCellUpdated.Invoke(_passedCells);
    }

    private void Update()
    {
        var normalizedMovmentValue = CalculateNormalizedMovmentValue();

        if (normalizedMovmentValue >= 1f && _currentCell?.GetNextObject() != null)
        {
            SetCurrentCell(_currentCell.GetNextObject());
            normalizedMovmentValue -= 1f;
        }

        Move(normalizedMovmentValue);
        _speedUp = false;
    }

    private float CalculateNormalizedMovmentValue()
    {
        var speedup = _speedUp ? _speedUpXSpeed : 1f;
        if(_movmentEnabled)
            return _lastNormalizedMovmentValue + _currentSpeed * Time.deltaTime * speedup;
        return _lastNormalizedMovmentValue;
    }


    public void EnableMovment(bool enable)
    {
        _movmentEnabled = enable;
        _onMovmentEnabled.Invoke(enable);
    }


    public virtual void AddWagon()
    {
        if (WagonCanBeAdded)
        {
            var wagon = _wagonsPool.GetObject(true);
            UpdateWagons();
        }
    }


    public virtual void RemoveWagon()
    {
        if (_wagons.Count > 1)
        {
            _wagons[_wagons.Count - 1].GetComponent<PoolObject>().HideAndDisable();
            UpdateWagons();
        }
    }


    public virtual void UpdateWagons()
    {
        _wagons = new();
        foreach (var obj in _wagonsPool.GetListOfActiveObjects())
        {
            _wagons.Add(obj.GetDataComponent<WagonType>());
        }
        SetCurrentCell(_currentCell);
    }


    internal override void SetCurrentCell(RailwayCell currentCell)
    {
        _currentCell.TrainExit(gameObject);
        _currentCell = currentCell;
        AddPassedCells(_currentCell);
        _currentCell.TrainEnter(gameObject);

        RailwayCell nextCell = (_currentCell != null) ? _currentCell.GetNextObject() : null;

        if(nextCell != null)
        {
            _distanceBetweenNextCells = Vector3.Distance(_currentCell.transform.position, nextCell.transform.position);

            //var headCell = _currentCell.GetPreviouseObject();
            int i = 1;
            foreach (var wagon in _wagons)
            {
                if (_passedCells[i] != null)
                {
                    wagon.SetCurrentCell(_passedCells[i]);
                    //wagon.SetCurrentCell(headCell);
                    //headCell = _passedCells[i]; //headCell.GetPreviouseObject();
                }
                i++;
            }
        }
        _onCellUpdated.Invoke(_passedCells);
    }


    internal override void Move(float normalizedMovment)
    {
        base.Move(normalizedMovment);

        foreach (var wagon in _wagons)
        {
            wagon.Move(normalizedMovment);
        }

        _lastNormalizedMovmentValue = normalizedMovment;
    }


    internal void HardPlaceToActualCell()
    {
        transform.position = _currentCell.transform.position;
        transform.forward = _currentCell.transform.forward;
    }


    private void InitPassedCells()
    {
        if (_currentCell == null)
            return;

        _passedCells = new List<RailwayCell>();
        _passedCells.Add(_currentCell);

        for(int i = 0; i < _wagonsPool.ObjectsMaxAmount + 1; i++)
        {
            _passedCells.Add(_passedCells[i].GetPreviouseObject());
        }
    }

    private void AddPassedCells(RailwayCell newCell)
    {
        if(_passedCells[0] == newCell)
        {
            return;
        }
        _passedCells.Insert(0, newCell);
        _passedCells.RemoveAt(_wagonsPool.ObjectsMaxAmount + 1);
    }
}
