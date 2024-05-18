using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceSystem
{
    public class ResurceChangingState<T>
    {
        public bool IsFinished
        {
            get
            {
                return _status == TResurceChangingStatus.Successful || _status == TResurceChangingStatus.Failed;
            }
        }
        public TResurceChangingStatus Status { get => _status; }
        public T Value { get => _value; }

        private TResurceChangingStatus _status = TResurceChangingStatus.InProgress;


        private T _guessValue;
        private T _value = default;

        public ResurceChangingState(T guessValue)
        {
            _guessValue = guessValue;
            _value = _guessValue;
        }

        public bool SetValue(bool isSuccessful, T value)
        {
            Debug.Log($"{this}>>> (Reward video) setting resault {isSuccessful} and {value}");
            if (IsFinished)
            {
                return false;
            }

            if (isSuccessful)
            {
                _value = value;
                _status = TResurceChangingStatus.Successful;
            }
            else
            {
                _value = value;
                _status = TResurceChangingStatus.Failed;
            }
            return true;
        }
    }


    public enum TResurceChangingStatus
    {
        InProgress,
        Successful,
        Failed
    }
}

