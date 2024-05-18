using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchingSign : MonoBehaviour
{
    [SerializeField] private bool _turnedOn = false;
    [Space]
    [SerializeField] private MeshRenderer _signMesh;
    [SerializeField] private Material _offMaterial;
    [SerializeField] private Material _onMaterial;
    [Space]
    [SerializeField] private bool _reverseValue = false;
    [SerializeField] private UnityEvent<bool> _onSwitched;


    private void OnValidate()
    {
        UpdateSign();
    }


    public void HardSwitch() => Switch(!_turnedOn);

    public void Switch(bool turnOn)
    {
        if (!isActiveAndEnabled)
            return;

        _turnedOn = turnOn;
        _onSwitched.Invoke(_reverseValue ? !_turnedOn : _turnedOn);
        UpdateSign();
    }


    private void UpdateSign()
    {
        _signMesh.material = _turnedOn ? _onMaterial : _offMaterial;

    }
}
