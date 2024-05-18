using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalForwarder : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private bool _useMaxParentForward = true;
    [SerializeField] Vector3 _forward = Vector3.back;
    private void OnValidate()
    {
        if (_useMaxParentForward)
        {
            Transform mparent = transform;

            while(mparent.parent != null)
            {
                mparent = mparent.parent;
            }

            transform.forward = mparent.forward;

        }
        else
        {
            transform.forward = _forward + Vector3.up * 0.02f;
        }

    }
#endif
}
