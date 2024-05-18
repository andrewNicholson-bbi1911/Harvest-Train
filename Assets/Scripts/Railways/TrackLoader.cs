using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackLoader : MonoBehaviour
{
    [SerializeField] private TrackDataContainer _dataContainer;
    [SerializeField] private List<RailwayTrack> _tracks;
    [SerializeField] private float loadingTime = 5f;
    [SerializeField] private MergingTrain _train;
    [Space]
    [Header("events")]
    [SerializeField] private UnityEvent _onTrackStartLoading;
    [SerializeField] private UnityEvent _onTrackLoaded;

    [SerializeField][Min(1)] private int _curentTrackLevel = 1;

    private bool _loading = false;

    private void OnValidate()
    {
        _train.SetCurrentCell(_tracks[0].RootCell);
        _train.HardPlaceToActualCell();

        var cargoCont = _train.GetComponent<CargoContainer>();

        foreach(var track in _tracks)
        {
            foreach(var plantF in track.GetComponentsInChildren<PlantField>())
            {
                plantF.SetHarvestingContainer(cargoCont);
            }
        }
    }

    private void Start()
    {
        for(int i = 0; i < _tracks.Count; i++)
        {
                LoadTrack(i, _dataContainer.IsTrackEnabled(i), true);
        }

    }

    public void LoadTrack(int trackID)
    {
        LoadTrack(trackID, true);
    }

    private void LoadTrack(int trackId, bool value, bool skipPreLoading = false)
    {
        if (_loading)
        {
            StopAllCoroutines();
            LoadTrackInstantly(trackId, value);
            _onTrackLoaded.Invoke();
            return;
        }

        if (!skipPreLoading)
        {
            StartCoroutine(StartLoadingTrack(trackId));
        }
        else
        {
            LoadTrackInstantly(trackId, value);
            _onTrackLoaded.Invoke();
        }
    }

    private void LoadTrackInstantly(int trackId, bool value)
    {
        int i = 0;
        int id = trackId % _tracks.Count;
        _tracks[id].EnableTrack(value, _train);
        _dataContainer.EnableTrack(id, value);
    }

    private IEnumerator StartLoadingTrack(int id)
    {
        _loading = true;
        _onTrackStartLoading.Invoke();
        yield return new WaitForSeconds(loadingTime);
        LoadTrackInstantly(id, true);
        _onTrackLoaded.Invoke();
        _loading = false;
    }
}
