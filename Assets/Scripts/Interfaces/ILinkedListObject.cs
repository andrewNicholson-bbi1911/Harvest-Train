using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILinkedListObject<T> where T : class
{
    T GetNextObject();
    T GetPreviouseObject();
}
