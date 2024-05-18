using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsClearer : MonoBehaviour
{
    [SerializeField] private int _touchesToClear = 5;
    

    public void UpdateTouch()
    {
        _touchesToClear -= 1;
        if(_touchesToClear <= 0)
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
