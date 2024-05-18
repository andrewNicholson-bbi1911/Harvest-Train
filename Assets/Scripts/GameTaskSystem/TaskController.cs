using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ResourceSystem;

public class TaskController : MonoBehaviour
{
    [SerializeField] private TaskDataContainer _dataContainer;
    [SerializeField] private List<Stage> _stages;
    [SerializeField] private List<IntResourceContainer> _trackingContainers;
    [Space]
    [Header("Task Progression")]
    [SerializeField] private GameObject _timer;
    [SerializeField] private UnityEvent<IntResourceData> _onTaskLoaded_res;
    [SerializeField] private UnityEvent<float> _onTaskNormProgress;
    [SerializeField] private UnityEvent<int> _onTaskValueProgress;
    [SerializeField] private UnityEvent<int> _onTimerStart;
    [SerializeField] private UnityEvent<int> _onTimeRemainingUppdated;
    [Space]
    [Header("Task Complition")]
    [SerializeField] private UnityEvent<GameTaskSO> _onTaskLoaded;
    [SerializeField] private UnityEvent<TaskReward> _onTaskComplited;
    [SerializeField] private UnityEvent<GameTaskSO> _onTaskFailed;
    [Space]
    [Header("Stage Complition")]
    [SerializeField] private UnityEvent<Stage> _onStageLoaded;
    [SerializeField] private UnityEvent<int> _onStageProgressUpdated;
    [SerializeField] private UnityEvent<StageReward> _onStageComplited;

    private int _currentStageID = 0;
    private int _currentTaskID = 0;
    private Stage _currentStage;
    private GameTaskSO _currentTask;
    private IntResourceData _targetTaskResource = null;
    private string _resID = "";
    [SerializeField] private int _taskAmount = int.MaxValue;
    [SerializeField] private int _currentValue = 0;

    private bool _taskStopped = false;
    private bool _taskComplited = false;


    private void Start()
    {
        int loadingStageID = _dataContainer.GetCurrentStage();
        int loadingTaskID = _dataContainer.GetCurrentTask() ;

        LoadStage(loadingStageID);
        StartTask(loadingTaskID);
    }

    public void StartNextStage()
    {
        _currentStage.onStageComplited.Invoke();
        LoadStage(Mathf.Min(_currentStageID + 1, _stages.Count - 1));
        StartTask(0);
    }

    public void StartNextTask()
    {
        if(_currentTaskID + 1 >= _currentStage.tasks.Count)
        {
            CompliteCurrentStage();
        }
        else
        {
            StartTask(_currentTaskID + 1);
        }
    }
    public void StartTaskAgiain() => StartTask(_currentTaskID);


    private void LoadStage(int stageID)
    {
        _currentStageID = stageID;
        if(_currentStageID >= _stages.Count)
        {
            _onStageLoaded.Invoke(_currentStage);
            _onStageProgressUpdated.Invoke(_currentStage.tasks.Count);
        }
        else
        {
            _currentStage = _stages[_currentStageID];
            _onStageLoaded.Invoke(_currentStage);
            _dataContainer.UpdateCurrentStage(_currentStageID);
        }
    }


    private void StartTask(int taskID)
    {
        if(taskID >= _currentStage.tasks.Count)
        {
            taskID = 0;
        }

        _taskStopped = false;
        _taskComplited = false;
        _currentTaskID = taskID;
        _currentTask = _currentStage.tasks[_currentTaskID];
        _dataContainer.UpdateCurrentTaskID(_currentTaskID);

        if(_currentTask.type == TaskType.HarvestWithTime || _currentTask.type == TaskType.EarnWithTime)
        {
            _timer.SetActive(true);
            StartCoroutine( StartTimer(_currentTask.taskTime));
        }
        else
        {
            _timer.SetActive(false);
        }


        _targetTaskResource = _currentTask.taskResource;
        _resID = _targetTaskResource.ResourceID;
        _currentValue = 0;
        _taskAmount = _targetTaskResource.CurrentValue;

        _onStageProgressUpdated.Invoke(_currentTaskID);
        _onTaskLoaded_res.Invoke(_targetTaskResource);
        _onTaskLoaded.Invoke(_currentTask);
        UpdateProgress();
    }

    private int _lastSavedId = -1;
    public void UpdateResourceData()
    {
        Debug.Log("TC>>updating progress");
        if (_taskStopped || _taskComplited) return;
        Debug.Log("TC>>>CAN updating progress");

        foreach (var cont in _trackingContainers)
        {
            var historyData = cont.LastChange;
            if (historyData.IsPositive && historyData.ID == _resID && historyData.HistoryId != _lastSavedId)
            {
                _currentValue += historyData.ChangeValue;
                UpdateProgress();
                _lastSavedId = historyData.HistoryId;
            }
        }

    }


    private void CompliteCurrentStage()
    {
        _onStageComplited.Invoke(_currentStage.stageReward);
        _onStageProgressUpdated.Invoke(_currentStage.tasks.Count);
    }


    private void UpdateProgress()
    {
        _currentValue = Mathf.Min(_currentValue, _taskAmount);
        _onTaskValueProgress.Invoke(_currentValue);
        _onTaskNormProgress.Invoke(_currentValue / (float)_taskAmount);

        if(_currentValue >= _taskAmount)
        {
            StopAllCoroutines();
            _taskComplited = true;
            _onTaskComplited.Invoke(_currentTask.reward);
        }
    }

    private IEnumerator StartTimer(float time)
    {
        _onTimerStart.Invoke((int)time);
        while (time > 0)
        {
            time -= Time.deltaTime;
            _onTimeRemainingUppdated.Invoke(Mathf.CeilToInt(time));
            yield return null;
        }
        if (!_taskComplited)
        {
            _taskStopped = true;
            _onTaskFailed.Invoke(_currentTask);
        }   
    }
}

[System.Serializable]
public struct Stage
{
    [SerializeField] public string stageValue;
    [SerializeField] public List<GameTaskSO> tasks;
    [SerializeField] public StageReward stageReward;
    [SerializeField] public UnityEvent onStageComplited;
}

[System.Serializable]
public struct StageReward
{
    [SerializeField] public List<IntResourceData> rewards;
}


