using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlantStateManager : MonoBehaviour
{
    [SerializeField] private List<PlantController> _plantsC;
    [SerializeField] private List<GameObject> _seeds;
    [SerializeField] private List<GameObject> _plants;

    public void UpdateGrowingState(float value)
    {
        EnableSeeds(true);
        EnablePlants(false);

        foreach(var seed in _seeds)
        {
            seed.transform.localScale = Vector3.forward + Vector3.right + Vector3.up * value;
        }
    }

    public void UpdatePlantStatus(float value)
    {
        EnableSeeds(false);
        EnablePlants(true);

        foreach (var plant in _plants)
        {
            plant.transform.localScale = (Vector3.forward + Vector3.right) * value + Vector3.up;
        }

    }

    private void EnableSeeds(bool enable)
    {
        foreach(var seedObj in _seeds)
        {
            seedObj.SetActive(enable);
        }
    }

    private void EnablePlants(bool enable)
    {
        foreach (var plantObj in _plants)
        {
            plantObj.SetActive(enable);
        }
    }
}
