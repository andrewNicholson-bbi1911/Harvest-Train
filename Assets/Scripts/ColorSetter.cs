using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> _coloredMeshes;
    [SerializeField] private List<Color> _levelColors;

    public void UpdateColor(int number)
    {
        var color = _levelColors[(number - 1) % _levelColors.Count];
        foreach(var mesh in _coloredMeshes)
        {
            mesh.material.color = color;
        }
    }
}



