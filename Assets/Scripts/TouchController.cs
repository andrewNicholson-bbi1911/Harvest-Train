using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TouchController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private UnityEvent _whileTouchEnabled;

    private bool _isTouching = false;

    public void EnableTouch()
    {
        _isTouching = true;
    }

    public void DisableTouch()
    {
        _isTouching = false;
    }

    private void Update()
    {
        if (_isTouching)
        {
            _whileTouchEnabled.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DisableTouch();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        EnableTouch();
    }
}

