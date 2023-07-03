using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimator : MonoBehaviour
{
    protected const string RunFlag = "Run";
    protected const string AttackTrigger = "Attack";
    protected const string DamagedTrigger = "Damaged";
    protected const string DeadTrigger = "Dead";

    protected Animator Animator;
    protected RangeCollisions RangeCollisions;

    protected bool IsInAttackRange;
    protected bool IsMoving;

    protected float AttackStartTime;
    protected float AttackCooldown;

    protected virtual void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        RangeCollisions = GetComponentInChildren<RangeCollisions>();
        GetComponent<Entity>().OnDeath += TriggerDeathAnimation;
    }

    protected virtual void SetMovingAnimation(bool isMoving)
    {
        IsMoving = isMoving;
        Animator.SetBool(RunFlag, isMoving);
    }

    protected void TriggerAttackAnimation()
    {
        AttackStartTime = Time.time;
        Animator.SetTrigger(AttackTrigger);
    }

    private void TriggerDeathAnimation()
    {
        Animator.SetTrigger(DeadTrigger);
    }
}
