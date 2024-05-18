using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MergingWagon : Wagon
{
    public int Level { get => _level; }

    [SerializeField] private int _level;
    [SerializeField] private UnityEvent<int> _onLevelChanged;
    [SerializeField] private UnityEvent _onMerged;

    private float _mergingStartEtta = -2f;
    private float _lastNormalizedMovment = -1f;
    private float extraNDelta = 0f;
    private int _mergingState = 0;


    public void SetLevel(int newLevel, bool _wasMerged)
    {
        _level = newLevel;
        _onLevelChanged.Invoke(_level);
        if (_wasMerged)
        {
            _onMerged.Invoke();
        }
    }


    public void IncreaseMergingSpeed()
    {
        _mergingState++;
    }


    public void StopMergingState()
    {
        _mergingStartEtta = -2f;
        _mergingState = 0;
        extraNDelta = 0;
    }


    internal override void Move(float normalizedMovment)
    {
        

        if(_mergingState <= 0)
        {
            base.Move(normalizedMovment);
        }
        else
        {
            bool _placedOnNewCell = false;
            if (_mergingStartEtta < -1f)
            {
                _mergingStartEtta = normalizedMovment;
                _lastNormalizedMovment = normalizedMovment;
            }

            if (normalizedMovment < _lastNormalizedMovment)
            {
                _placedOnNewCell = true;
            }

            _lastNormalizedMovment = normalizedMovment;

            if (_placedOnNewCell)
            {
                _mergingStartEtta -= 1f;
            }

            var newEtta = _mergingStartEtta + (1 + _mergingState) * (normalizedMovment - _mergingStartEtta);// - extraNDelta;

            //base.Move(newEtta);
            
            RailwayCell nextCell =  _currentCell?.GetNextObject();
            RailwayCell realCurrentCell = _currentCell;
            while(newEtta > 1)
            {
                if(nextCell != null)
                {
                    realCurrentCell = realCurrentCell.GetNextObject();
                    nextCell = realCurrentCell.GetNextObject() ;
                }
                newEtta-=1f;
            }

            if (nextCell != null)
            {
                transform.forward = Vector3.SlerpUnclamped(realCurrentCell.GetDirection(), nextCell.GetDirection(), newEtta);
                transform.position = Vector3.Lerp(realCurrentCell.transform.position, nextCell.transform.position, newEtta);
            }
        }
    }
}
