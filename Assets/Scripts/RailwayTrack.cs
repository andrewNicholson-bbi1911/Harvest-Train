using UnityEngine;
using UnityEngine.Events;

public class RailwayTrack : MonoBehaviour
{
    public RailwayCell RootCell { get => _trackStation.RootCell; }

    [SerializeField] private RailwayStation _trackStation;
    //[SerializeField] private UnityEvent _onFirstLoad;
    [SerializeField] private UnityEvent _onTrackEnabled;
    [SerializeField] private UnityEvent _onTrackDisabled;

    [SerializeField] private bool _enabled = true;
    //[SerializeField] private bool _wasLoaded = false;
    

    public void EnableTrack(bool enable, MergingTrain train)
    {
        _enabled = enable;
        //gameObject.SetActive(enable);
        if (_enabled)
        {
            if (_trackStation != null)
            {
                //train.SetCurrentCell(_trackStation.RootCell.GetNextObject());
                //_trackStation.EnterTrain(train.gameObject);
            }
            //train.SetCurrentCell(_trackStation.RootCell.GetPreviouseObject());
            //train.HardPlaceToActualCell();
            _onTrackEnabled.Invoke();
        }
        else
        {
            _onTrackDisabled.Invoke();
        }
    }


}
