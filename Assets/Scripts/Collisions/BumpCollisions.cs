using System;
using UnityEngine;

public class BumpCollisions : MonoBehaviour
{
    public Action<int> OnBumpCollisionEvent;

    // TODO Make this parameters configurable at some point
    private const float Cooldown = 4f;
    private const int BumpDamage = 30;

    private bool _canInvoke = true;
    private float _enterTime;

    void OnCollisionEnter(Collision collision)
    {
        if (!_canInvoke) return;

        SetTimerAndInvokeCallback();
    }

    void OnCollisionStay(Collision collision)
    {
        if (!_canInvoke) return;

        SetTimerAndInvokeCallback();
    }

    private void SetTimerAndInvokeCallback()
    {
        _canInvoke = false;
        _enterTime = Time.time;

        OnBumpCollisionEvent?.Invoke(BumpDamage);
    }


    void Update()
    {
        if (Time.time - _enterTime > Cooldown)
        {
            _canInvoke = true;
        }
    }
}
