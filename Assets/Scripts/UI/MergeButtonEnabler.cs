using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeButtonEnabler : MonoBehaviour
{
    [SerializeField] private MergingTrain _connectedTrain;
    [SerializeField] private Button _mergeButton;


    private void Start()
    {
        _connectedTrain.onWagonsChanged += UpdateEnabled;
        _connectedTrain.onMerged += UpdateEnabled;
        UpdateEnabled();
    }


    public void UpdateEnabled()
    {
        _mergeButton.interactable = _connectedTrain.CanBeMerged;
    }
}
