using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    [SerializeField] protected RailwayCell _currentCell;

    internal virtual void SetCurrentCell(RailwayCell cell)
    {
        _currentCell = cell;
    }

    internal virtual void Move(float normalizedMovment)
    {
        RailwayCell nextCell = _currentCell?.GetNextObject();
        if (nextCell != null)
        {
            transform.forward = Vector3.Slerp(_currentCell.GetDirection(), nextCell.GetDirection(), normalizedMovment);
            transform.position = Vector3.LerpUnclamped(_currentCell.transform.position, nextCell.transform.position, normalizedMovment);
        }
    }
}
