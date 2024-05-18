using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataContainer<Data> where Data : class
{
    string GetJsonString();
    void ForceLoadData();
    void LoadData(Data data);
}
