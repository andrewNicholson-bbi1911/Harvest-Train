using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackDataContainer : MonoBehaviour, IDataContainer<TrackData>
{
    [SerializeField] private string _gameDataField = "track";
    [SerializeField] private int _baseTracksAmount = 5;

    private TrackData _data = null;


    public void EnableTrack(int id, bool isEnabled)
    {
        if(_data == null)
        {
            ForceLoadData();
        }

        _data.SetTrackEnabled(id, isEnabled);
        DataLoader.SaveData(_gameDataField, this);
    }

    public bool IsTrackEnabled(int id)
    {
        if(_data == null)
        {
            ForceLoadData();
        }

        return _data.IsTrackEnabled(id);
    }


    public void ForceLoadData()
    {
        var data = DataLoader.GetData<TrackData>(_gameDataField);
        if(data == null)
        {
            _data = new TrackData(_baseTracksAmount);
            DataLoader.SaveData(_gameDataField, this);
        }
        else
        {
            LoadData(data);
        }
    }

    public string GetJsonString()
    {
        return JsonUtility.ToJson(_data);
    }

    public void LoadData(TrackData data)
    {
        _data = data;
    }
}


[System.Serializable]
public class TrackData
{
    [SerializeField] private bool[] _enabledTracks = null;

    public TrackData(int tracksAmount)
    {
        _enabledTracks = new bool[tracksAmount];
        _enabledTracks[0] = true;
        for(int i = 1; i < _enabledTracks.Length; i++)
        {
            _enabledTracks[i] = false;
        }
    }

    public void SetTrackEnabled(int trackID, bool value)
    {
        trackID %= _enabledTracks.Length;
        _enabledTracks[trackID] = value;
    }

    public bool IsTrackEnabled(int trackID)
    {
        trackID %= _enabledTracks.Length;
        return _enabledTracks[trackID];
    }
}
