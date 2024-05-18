using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreightPart : RailwayPart
{
    [Space]
    [Header("Streight Part data")]
    [SerializeField] private GameObject _baseCellPart;
    [SerializeField][Min(2)] private int _length = 2;


#if UNITY_EDITOR

    protected override void OnValidate()
    {
        RailwayCell cell;
        if(!_baseCellPart.TryGetComponent(out cell))
        {
            Debug.LogWarning($"{this}>>>You tries to use _baseCellPart without RailwayPart Component");
            _baseCellPart = null;
        }
        else
        {
            if (_cells.Count != _length)
            {
                GeneratePart();
            }
        }

        base.OnValidate();
    }


    protected override void ReplaceCells()
    {
        Vector3 curPosition = Vector3.zero;
        Vector3 step = transform.forward;

        foreach (var cell in _cells)
        {
            cell.transform.localPosition = curPosition;
            cell.transform.forward = transform.forward;
            curPosition += Vector3.forward;
        }
    }


    private void GeneratePart()
    {
        var cellIndex = 0;
        List<RailwayCell> newCells = new();

        foreach(var cell in _cells)
        {
            if(cellIndex >= _length)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    DestroyImmediate(cell.gameObject);
                };
            }
            else
            {
                newCells.Add(cell);
            }

            cellIndex++;
        }

        while(cellIndex < _length)
        {
            var newCell = UnityEditor.PrefabUtility.InstantiatePrefab(_baseCellPart, transform);
            var cell = (newCell as GameObject).GetComponent<RailwayCell>();
            cell.EnableAutoPlace(false);
            newCells.Add(cell );

            cellIndex++;
        }

        _cells = newCells;
    }

#endif

}
