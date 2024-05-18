using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollowSetter : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cameraController;
    [SerializeField] private Transform _defaultTarget;
    [SerializeField] private float _totalShowTime = 5f;
    [Space]
    [SerializeField] private List<Transform> _followPoints;


    public void MakeSnapshotOfPointByIndex(int index)
    {
        StopAllCoroutines();
        SetFollowPoint(_followPoints[index % _followPoints.Count]);
        StartCoroutine(FollowDefault());
    }

    private void SetFollowPoint(Transform point)
    {
        _cameraController.Follow = point;
    }

    private IEnumerator FollowDefault()
    {
        yield return new WaitForSeconds(_totalShowTime);
        SetFollowPoint(_defaultTarget);
    }
}
