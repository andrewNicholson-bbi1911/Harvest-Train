using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDataContainer : MonoBehaviour, IDataContainer<TaskData>
{
    [SerializeField] private string _gameDataField = "tasks";
    private TaskData _data = null;


    private void Start()
    {
        ForceLoadData();
    }


    public void UpdateCurrentStage(int stageID)
    {
        if(_data == null)
        {
            ForceLoadData();
        }
        _data.currentStageID = stageID;
        DataLoader.SaveData(_gameDataField, this);
    }


    public void UpdateCurrentTaskID(int taskID)
    {
        if (_data == null)
        {
            ForceLoadData();
        }
        _data.currentTaskID = taskID;
        DataLoader.SaveData(_gameDataField, this);
    }


    public int GetCurrentStage()
    {
        if (_data == null)
        {
            ForceLoadData();
        }

        return _data.currentStageID;
    }


    public int GetCurrentTask()
    {
        if (_data == null)
        {
            ForceLoadData();
        }

        return _data.currentTaskID;
    }

    public void ForceLoadData()
    {
        var data = DataLoader.GetData<TaskData>(_gameDataField);
        if(data == null)
        {
            _data = new TaskData();
            DataLoader.SaveData(_gameDataField, this);
        }
        else
        {
            LoadData(data);
        }
    }


    public string GetJsonString()
    {
        return JsonUtility.ToJson(_data);
    }


    public void LoadData(TaskData data)
    {
        _data = data;
    }
}


[System.Serializable]
public class TaskData
{
    [SerializeField] public int currentStageID;
    [SerializeField] public int currentTaskID;

    public TaskData()
    {
        currentStageID = 0;
        currentTaskID = 0;
    }
}