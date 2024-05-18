using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class RailwayStation : RailwayStationBase<MergingTrain, MergingWagon>
{
    public RailwayCell RootCell { get => _rootCell; }

    [SerializeField] private RailwayCell _rootCell;
    [SerializeField] private List<GameObject> _stationEnablers;

    private void OnValidate()
    {
        foreach(var obj in _stationEnablers)
        {
            IStationEnabler enabler;
            if (!obj.TryGetComponent(out enabler))
            {
                _stationEnablers.Remove(obj);
                Debug.LogWarning($"{this}>>>{obj} doesnt contain any components realising IStationEnabler interface ");
            }
        }
        _stationEnablers.RemoveAll(x => x == null);
    }

    protected override void Start()
    {
        foreach(var obj in _stationEnablers)
        {
            foreach(var comp in obj.GetComponents<IStationEnabler>())
            {
                AddEnabler(comp);
            }
        }
        base.Start();
    }
}




public class RailwayStationBase<TrainT, WT> : MonoBehaviour where TrainT : Train<WT> where WT: Wagon
{
    [SerializeField] private float _tickTime = 0.05f;
    [SerializeField] private UnityEvent<TrainT> _onTrainStays;
    [SerializeField] private TrainT _actualTrain = null;

    private List<IStationEnabler> _stationEnablers = new();
    private bool _isTrainInside = false;


    protected virtual void Start()
    {
        StartCoroutine(UpdateTrainInside());
    }


    public void AddEnabler(IStationEnabler enabler)
    {
        if (!_stationEnablers.Contains(enabler))
        {
            _stationEnablers.Add(enabler);
        }
    } 


    public void EnterTrain(GameObject trainObj)
    {
        TrainT train;
        if(trainObj.TryGetComponent(out train))
        {
            if (_actualTrain != null)
            {
                RemoveTrain(_actualTrain.gameObject);
            }
            _actualTrain = train;
            train.EnableMovment(false);
            _isTrainInside = true;
        }
    }


    public void RemoveTrain(GameObject trainObj)
    {
        TrainT train;
        if (trainObj.TryGetComponent(out train))
        {
            if (_actualTrain == train)
            {
                _actualTrain = null;
            }
            _isTrainInside = false;
        }
    }


    private bool NeedToReleaseTrain()
    {
        bool enablersAllow = true;
        foreach(var enabler in _stationEnablers)
        {
            enablersAllow &= enabler.CanTrainMove();
        }
        return enablersAllow;
    }


    private IEnumerator UpdateTrainInside()
    {
        var timeRemaining = _tickTime;
        while (true)
        {
            timeRemaining = _tickTime;

            while(timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                yield return null;
            }

            if (_isTrainInside)
            {
                _onTrainStays.Invoke(_actualTrain);

                if (NeedToReleaseTrain())
                {
                    _actualTrain.EnableMovment(true);
                    //RemoveTrain(_actualTrain.gameObject);
                }
            }

            
        }
    }
}


public interface IStationEnabler
{
    bool CanTrainMove();
}