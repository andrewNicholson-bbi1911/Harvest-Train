using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour, IDataContainer<SceneData>
{
    private static SceneLoader _inst;
    [SerializeField] private string _dataFieldName = "scenes";
    [SerializeField][Min(0)] private int _baseActiveSceneIndex = 0;
    [SerializeField] private List<string> _scenesName;
    [SerializeField] private float _baseWaitingTime = 3f;
    private SceneData _data = null;


    public static void LoadNewScene(int index)
    {
        _inst.LoadScene(index);
    }


    private void Start()
    {
        if(_inst == null)
        {
            _inst = this;
        }
        else
        {
            if(_inst != this)
            {
                Destroy(gameObject);
                return;
            }
        }
        ForceLoadData();
        StartCoroutine(LoadSceneCour(_data.LastID));
    }


    private void LoadScene(int index)
    {
        if (_data == null)
        {
            ForceLoadData();
        }


        index %= _scenesName.Count;

        _data.UpdateActiveScene(index);

        DataLoader.SaveData(_dataFieldName, this);
        SceneManager.LoadScene(_scenesName[index]);
    }


    public string GetJsonString()
    {
        return JsonUtility.ToJson(_data);
    }


    public void ForceLoadData()
    {
        var data = DataLoader.GetData<SceneData>(_dataFieldName);
        if (data == null) 
        {
            _data = new SceneData(_baseActiveSceneIndex);
            DataLoader.SaveData(_dataFieldName, this);
        }
        else
        {
            LoadData(data);
        }
    }

    public void LoadData(SceneData data)
    {
        _data = data;
    }

    private IEnumerator LoadSceneCour(int index)
    {
        yield return new WaitForSeconds(_baseWaitingTime);
        LoadScene(index);
    }
}

[System.Serializable]
public class SceneData
{
    public int LastID { get => _lastActiveSceneID; }
    [SerializeField] private int _lastActiveSceneID = 0;

    public SceneData(int baseID)
    {
        _lastActiveSceneID = baseID;
    }

    public void UpdateActiveScene(int value)
    {
        _lastActiveSceneID = value;
    }
}
