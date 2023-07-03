using System;
using System.Collections;
using System.Security.AccessControl;
using UnityEngine;

public class EnemyAnimator : EntityAnimator
{
    public Action<bool> OnEnemyMoving;
    protected override void Awake()
    {
        base.Awake();

        GetComponentInChildren<AttackRangeCollisions>().OnAttackRangeEnterEvent += SetInAttackRange;
        GetComponentInChildren<AttackRangeCollisions>().OnAttackRangeExitEvent += SetNotInAttackRange;

        GetComponentInChildren<HitCollisions>().OnHitTriggerEvent += StartHitAnimation;
        GetComponent<Enemy>().OnWeaponSet += SetAttackCooldown;

        RangeCollisions.OnRangeEnterEvent += StartMoveAnimation;
        RangeCollisions.OnRangeExitEvent += StopMoveAnimation;
    }

    private void SetAttackCooldown(EnemyWeaponStats stats)
    {
        AttackCooldown = stats.CooldownInSeconds;
    }

    private void SetInAttackRange()
    {
        IsInAttackRange = true;

        SetMovingAnimation(!IsInAttackRange);
    }

    private void SetNotInAttackRange()
    {
        IsInAttackRange = false;

        SetMovingAnimation(!IsInAttackRange);
    }

    private void StartMoveAnimation()
    {
        SetMovingAnimation(true);
    }
    private void StopMoveAnimation()
    {
        SetMovingAnimation(false);
    }

    protected override void SetMovingAnimation(bool isMoving)
    {
        base.SetMovingAnimation(isMoving);
        OnEnemyMoving?.Invoke(isMoving);
    }

    private void StartHitAnimation(int damage)
    {
        Animator.SetTrigger(DamagedTrigger);
    }

    void Update()
    {
        if (!CanAttack()) return;

        TriggerAttackAnimation();
    }

    private bool CanAttack()
    {
        return IsInAttackRange && !IsMoving && (Time.time - AttackStartTime > AttackCooldown);
    }

}
