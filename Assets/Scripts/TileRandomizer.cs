using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRandomizer : MonoBehaviour
{
    [SerializeField] private bool _useHardPosition = true;
    [SerializeField] private bool _randomizeRotation = true;
    [SerializeField] private bool _randomizeScale = true;
    [SerializeField] private Vector3 _baseLocalPosition;
    [SerializeField] private float _heightDelta = 0.03f;
    [SerializeField] private float _scaleAvg = 1f;

    private void OnValidate()
    {
        if (_randomizeRotation)
        {
            transform.rotation = Quaternion.Euler(0, Random.Range(-20f, 20f), 0);
            //transform.Rotate(transform.up, Random.Range(-20f, 20f));
        }
        if (_randomizeScale)
            transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f) * _scaleAvg;

        if (_useHardPosition)
        {
            transform.localPosition = _baseLocalPosition;
            transform.position += Vector3.up * Random.Range(-_heightDelta, _heightDelta);
        }
    }


}
