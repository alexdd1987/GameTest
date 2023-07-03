using System;
using UnityEngine;

public class RangeCollisions : MonoBehaviour
{
    public Action OnRangeEnterEvent;
    public Action OnRangeExitEvent;
    public Action<Transform> OnRangeStayEvent;

    void OnTriggerEnter(Collider other)
    {
        OnRangeEnterEvent?.Invoke();
    }

    void OnTriggerStay(Collider other)
    {
        OnRangeStayEvent?.Invoke(other.transform.parent.transform);
    }

    void OnTriggerExit(Collider other)
    {
        OnRangeExitEvent?.Invoke();
    }
}