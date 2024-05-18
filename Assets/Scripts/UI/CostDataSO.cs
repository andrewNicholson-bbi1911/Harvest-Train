using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePlay Features/Cost Data", menuName ="GamePlay Feature/Cost Data")]
public class CostDataSO : ScriptableObject
{
    [SerializeField] [TextArea(1, 5)] private string _rawExelList = "";
    [Space]
    [SerializeField] private int _roundValue = -1;
    [SerializeField] private float _plexer = 1.063f;
    [SerializeField] private int[] _costs;

    private void OnValidate()
    {
        if(_rawExelList != "")
        {
            List<int> costs = new();
            foreach (var str in _rawExelList.Split("\n"))
            {
                int cost = 0;

                if(int.TryParse(str, out cost))
                {
                    costs.Add(cost);
                }
                else
                {

                    costs.Add(Mathf.RoundToInt( Mathf.RoundToInt((costs[costs.Count - 1] * _plexer * Mathf.Pow(10, _roundValue))) / Mathf.Pow(10, _roundValue)));
                    Debug.LogWarning($"{this}>>> _rawExelList contains anapropriate data: {str}");
                }
            }
            _rawExelList = "";

            _costs = new int[costs.Count];
            costs.CopyTo(_costs);
        }
    }

    public int GetValueForLevel(int level)
    {
        if (level >= _costs.Length)
        {
            return Mathf.RoundToInt( Mathf.RoundToInt(_costs[_costs.Length - 1] * Mathf.Pow(_plexer, 1 + level - _costs.Length) * Mathf.Pow(10, _roundValue)) / Mathf.Pow(10, _roundValue));
        }
        else
        {
            return _costs[level];
        }
    }
}
