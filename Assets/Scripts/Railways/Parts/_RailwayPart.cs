using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RailwayPart : MonoBehaviour, ILinkedListObject<RailwayPart>, ICellInfoContainer
{
    protected RailwayCell FirstCell { get => _cells.Count > 0 ? _cells [0] : null; }
    protected RailwayCell LastCell { get => _cells.Count > 0 ? _cells[_cells.Count - 1] : null; }

    [SerializeField] private bool _autoPlace = true;
    [SerializeField] protected RailwayPart _nextRailwayPart = null;
    [SerializeField] protected RailwayPart _previousRailwayPart = null;

    [SerializeField] protected List<RailwayCell> _cells = new();

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (_nextRailwayPart != null)
        {
            _nextRailwayPart._previousRailwayPart = this;
        }

        if (_previousRailwayPart != null && _autoPlace)
        {
            var cell = _previousRailwayPart.LastCell;
            transform.position = cell.transform.position + cell.transform.forward;
            transform.forward = cell.transform.forward;
        }

        ReplaceCells();
        UpdateCellNeighbours();


    }

    protected abstract void ReplaceCells();

    protected void UpdateCellNeighbours()
    {
        foreach (var cell in _cells)
        {
            if (cell != null)
            {
                cell.LoadData(this);
            }
        }
    }
#endif


    public RailwayPart GetNextObject() => _nextRailwayPart;


    public RailwayPart GetPreviouseObject() => _previousRailwayPart;


    public virtual RailwayCell GetNextCell(RailwayCell cell)
    {
        if (!_cells.Contains(cell)) return null;

        var index = _cells.FindIndex((x) => x == cell);
        if(index == _cells.Count - 1)
        {
            if (_nextRailwayPart != null)
            {
                return _nextRailwayPart.FirstCell;
            }
            else
            {
                return null;
            }
            
        }
        else
        {
            return _cells[index + 1];
        }
    }

    public virtual RailwayCell GetPreviouseCell(RailwayCell cell)
    {
        if (!_cells.Contains(cell)) return null;

        var index = _cells.FindIndex((x) => x == cell);
        if (index == 0)
        {
            if (_previousRailwayPart != null)
            {
                return _previousRailwayPart.LastCell;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return _cells[index - 1];
        }
    }


}


public interface ICellInfoContainer
{
    RailwayCell GetNextCell(RailwayCell cell);
    RailwayCell GetPreviouseCell(RailwayCell cell);
}