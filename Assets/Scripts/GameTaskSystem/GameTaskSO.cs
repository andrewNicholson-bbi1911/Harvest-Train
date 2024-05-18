using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceSystem;

[CreateAssetMenu(fileName = "GamePlay Features/Tasks", menuName = "GamePlay Feature/Resource Game Task")]
public class GameTaskSO : ScriptableObject
{
    [SerializeField] public TaskType type;
    [SerializeField] public IntResourceData taskResource;
    [SerializeField] public float taskTime;
    [SerializeField] public TaskReward reward;
}


[System.Serializable]
public struct TaskReward
{
    [SerializeField] public List<IntResourceData> rewards;
}

public enum TaskType
{
    Harvest,
    HarvestWithTime,
    Earn,
    EarnWithTime,
}