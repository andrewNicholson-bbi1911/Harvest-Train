using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    [SerializeField] private List<Transform> _fruts;
    [SerializeField] private List<StatusColor> _growingStatuses;
    [SerializeField] private List<StatusColor> _readyStatuses;

    /*
    private void OnValidate()
    {
        SetState(true);
    }*/

    private void Awake()
    {
        SetState(true);
    }


    public void SetState(bool ready)
    {
        ShowFruts(_fruts.Count);

        foreach (var status in ready ? _readyStatuses : _growingStatuses)
        {
            status.UpdateColor();

        }

        if (ready)
        {
            UpdateFrutsGrowing(1);
        }
        else
        {
            UpdateFrutsGrowing(0);
        }

    }

    public void ShowFruts(int showingAmount)
    {
        foreach(var frut in _fruts)
        {
            frut.gameObject.SetActive(showingAmount > 0);
            showingAmount--;
        }
    }

    public void UpdateFrutsGrowing(float normValue)
    {
        foreach(var frut in _fruts)
        {
            frut.localScale = Vector3.one * normValue;
        }
    }
}


[System.Serializable]
public struct StatusColor
{
    [SerializeField] private Color _color;
    [SerializeField] private List<MeshRenderer> _meshes;

    public void UpdateColor()
    {
        foreach(var mesh in _meshes)
        {
            mesh.material.color = _color;
        }
    }
}
