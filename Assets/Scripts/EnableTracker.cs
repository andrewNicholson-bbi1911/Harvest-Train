using UnityEngine;
using UnityEngine.Events;

public class EnableTracker : MonoBehaviour
{
    [SerializeField] private UnityEvent _onEnable;
    [SerializeField] private UnityEvent _onDisable;

    private void OnEnable()
    {
        _onEnable.Invoke();
    }

    private void OnDisable()
    {
        _onDisable.Invoke();
    }
}
