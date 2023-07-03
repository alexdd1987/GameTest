using System;
using UnityEngine;

public class HeroAnimator : EntityAnimator
{
    public Action OnAttackStarted;

    private const string AttackSpeedParameter = "AttackSpeed";

    private bool _isRotating;
    private bool _isRotationComplete;
    private bool _isAttacking;

    protected override void Awake()
    {
        base.Awake();

        GetComponentInChildren<WeaponInventory>().OnWeaponEquipped += SetAttackAnimationSpeedAndCooldown;
        GetComponent<RotateHeroTowardsTarget>().OnRotationToTargetSet += SetIsRotating;
        GetComponent<RotateHeroTowardsTarget>().OnRotationComplete += SetRotationComplete;
        GetComponentInChildren<AnimationEvents>().OnHeroAttackAnimationEvent += SetAttack;

        RangeCollisions.OnRangeStayEvent += SetInAttackRange;
        RangeCollisions.OnRangeExitEvent += SetNotInAttackRange;

        PlayerInput.OnMouseDragged += StartMoveAnimation;
        PlayerInput.OnMouseUnclicked += StopMoveAnimation;
    }

    private void SetInAttackRange(Transform other)
    {
        IsInAttackRange = true;
    }

    private void SetNotInAttackRange()
    {
        IsInAttackRange = false;
    }

    private void StartMoveAnimation(Vector3 direction)
    {
        SetMovingAnimation(true);

        // move interrupts attack animation, so we set the flag accordingly
        _isAttacking = false;
    }

    private void StopMoveAnimation()
    {
        SetMovingAnimation(false);
    }

    private void SetIsRotating(bool isRotating)
    {
        _isRotating = isRotating;
    }

    private void SetRotationComplete()
    {
        _isRotationComplete = true;
    }

    private void SetAttackAnimationSpeedAndCooldown(WeaponData data)
    {
        Animator.SetFloat(AttackSpeedParameter, 1.0f / data.Weight);
        AttackCooldown = data.Cooldown;

        // set _attackStartTime so that it's ready to attack immediately on weapon equipped
        AttackStartTime = -AttackCooldown;
    }

    private void SetAttack(bool isAttacking)
    {
        _isAttacking = isAttacking;
    }

    void LateUpdate()
    {
        if (!CanAttack()) return;

        _isRotationComplete = false;
        TriggerAttackAnimation();

        OnAttackStarted?.Invoke();
    }

    private bool CanAttack()
    {
        return IsInAttackRange && !IsMoving && !_isRotating && _isRotationComplete && !_isAttacking &&
               (Time.time - AttackStartTime > AttackCooldown);
    }

    void OnDestroy()
    {
        PlayerInput.OnMouseDragged -= StartMoveAnimation;
        PlayerInput.OnMouseUnclicked -= StopMoveAnimation;
    }
}