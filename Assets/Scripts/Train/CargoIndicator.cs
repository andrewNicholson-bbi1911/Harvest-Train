using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoIndicator : MonoBehaviour
{
    [SerializeField] private Transform _cargoTransform;
    [SerializeField] private List< MeshRenderer> _meshRenderers;
    [Space]
    [Header("Empty position")]
    [SerializeField] private Color _emptyColor = Color.white;
    [SerializeField] private Vector3 _emptyPosition;
    [SerializeField] private Vector3 _emptyScale;
    [Space]
    [Header("Full position")]
    [SerializeField] private Color _cargoColor;
    [SerializeField] private Vector3 _maxPosition;
    [SerializeField] private Vector3 _maxScale;



    public void UpdateIndicator(float normValue)
    {
        if(normValue <= 0.01f)
        {
            UpdateMeshesColor(_emptyColor);
            _cargoTransform.transform.localPosition = GetPosition(0);
            _cargoTransform.transform.localScale = GetScale(0);
        }
        else
        {
            UpdateMeshesColor(_cargoColor);
            _cargoTransform.transform.localPosition = GetPosition(normValue);
            _cargoTransform.transform.localScale = GetScale(normValue);
        }
    }


    private Vector3 GetPosition(float normValue)
    {
        return Vector3.Lerp(_emptyPosition, _maxPosition, normValue);
    }

    private Vector3 GetScale(float normValue)
    {
        return Vector3.Lerp(_emptyScale, _maxScale, normValue);
    }

    private void UpdateMeshesColor(Color color)
    {
        foreach(var meshR in _meshRenderers)
        {
            meshR.material.color = color;
        }
    }
}
