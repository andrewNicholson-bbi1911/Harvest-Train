using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshPro _levelText;

    public void UpdateLevelState(int level)
    {
        _levelText.text = level.ToString();
    }
}
