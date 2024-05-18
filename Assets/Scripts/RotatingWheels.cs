using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWheels : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _k = 1;
    [SerializeField] private Vector3 _axis = Vector3.right;
    [SerializeField] private List<Transform> _wheels;


    private int _enableFactor = 1;


    public void SetSpeed(float speed) => _speed = speed;


    public void EnableRotation(bool enable)
    {
        _enableFactor = (enable == true) ? 1 : 0;
    }


    void Update()
    {
        foreach(var wheel in _wheels)
        {
            wheel.Rotate(Vector3.right, Mathf.Rad2Deg * _speed * _k * Time.deltaTime * _enableFactor);
        }
    }
}
