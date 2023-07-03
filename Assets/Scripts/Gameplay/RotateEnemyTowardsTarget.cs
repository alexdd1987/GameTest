using System;
using UnityEngine;

// This is a separated script and is not in the Enemy script because we might 
// want to have static enemies that only face towards the hero and shoot 
// projectiles at them. Hence I am separating the rotation and the movement.
public class RotateEnemyTowardsTarget : RotateEntityTowardsTarget
{
    public Action<Transform> OnTargetLock;

    private const int PlayerLayerMask = 1 << 3;

    private bool _isDamaged;

    protected override void Awake()
    {
        base.Awake();

        ColliderOffset = new Vector3(0f, 1.137429f, 0f);

        RangeCollisions.OnRangeEnterEvent += SetTargetLocked;
        RangeCollisions.OnRangeExitEvent += SetTargetNotLocked;

        AnimationEvents.OnEnemyAttackAnimationEvent += SetIsAttacking;
        AnimationEvents.OnEnemyDamageAnimationEvent += SetIsDamaged;

        GetComponentInParent<Entity>().OnEntityParamsSet += SetOverlapSphereRadius;
    }

    private void SetOverlapSphereRadius(Entity.EntityParams entityParams)
    {
        SetOverlapSphereRadius(entityParams.Range);
    }

    private void SetIsAttacking(bool attacking)
    {
        IsAttacking = attacking;
    }

    private void SetIsDamaged(bool isDamaged)
    {
        _isDamaged = isDamaged;

        // stop attack state if attacking animation
        // gets interrupted by damage animation
        IsAttacking = false;
    }

    void Update()
    {
        if (!CanRotate()) return;

        RotateTowardsTarget();
    }

    private bool CanRotate()
    {
        return IsLockedToTarget && !IsAttacking && !_isDamaged;
    }

    private void SetTargetLocked()
    {
        IsLockedToTarget = true;

        var targets = Physics.OverlapSphere(Transform.position + ColliderOffset, OverlapSphereRadius, PlayerLayerMask);

        if (targets.Length == 0) return;

        Target = targets[0].transform;
        OnTargetLock?.Invoke(Target);
    }

    private void SetTargetNotLocked()
    {
        IsLockedToTarget = false;
        OnTargetLock?.Invoke(null);
    }
}