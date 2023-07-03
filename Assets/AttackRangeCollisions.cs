using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeCollisions : MonoBehaviour
{
    public Action OnAttackRangeEnterEvent;
    public Action OnAttackRangeExitEvent;
    void OnTriggerEnter(Collider other)
    {
        OnAttackRangeEnterEvent?.Invoke();
        GameManager.Instance.PrintLog("Entity " + other.gameObject + "In AttackRange from " + gameObject.name);
    }

    void OnTriggerStay(Collider other)
    {
        GameManager.Instance.PrintLog("In Range Stay from " + other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        OnAttackRangeExitEvent?.Invoke();
    }
}
