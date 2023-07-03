using System;
using System.Collections;
using UnityEngine;

public class RotateHeroTowardsTarget : RotateEntityTowardsTarget
{
    public Action<bool> OnRotationToTargetSet;
    public Action OnRotationComplete;

    [SerializeField] private SphereCollider _rangeCollider;

    private const int EnemyLayerMask = 1 << 13;
    private const float SmallValue = 0.1f;

    private bool _isRotating;
    private bool _isMoving;

    protected override void Awake()
    {
        base.Awake();

        ColliderOffset = new Vector3(0f, 1.154727f, 0f);

        GetComponentInChildren<WeaponInventory>().OnWeaponEquipped += SetOverlapSphereRadius;

        RangeCollisions.OnRangeStayEvent += FindClosestTargetAndLockTarget;
        AnimationEvents.OnHeroAttackAnimationEvent += SetEntityAttack;

        PlayerInput.OnMouseDragged += SetPlayerMoving;
        PlayerInput.OnMouseUnclicked += SetPlayerStopped;
    }

    private void SetEntityAttack(bool attacking)
    {
        IsAttacking = attacking;
    }

    private void SetOverlapSphereRadius(WeaponData data)
    {
        SetOverlapSphereRadius(_rangeCollider.radius * data.Range);
    }

    private void SetPlayerMoving(Vector3 direction)
    {
        _isMoving = true;

        // Reset targeting flags
        IsLockedToTarget = false;
        IsAttacking = false;

        if (!_isRotating) return;

        // Interrupt Rotation
        SetRotationAndInvokeCallback(false);
    }

    private void SetPlayerStopped()
    {
        _isMoving = false;
    }

    void Update()
    {
        if (!IsLockedToTarget) return;

        SetRotationAndInvokeCallback(true);

        RotateTowardsTarget();

        if (Quaternion.Angle(Transform.rotation, LookRotation) <= SmallValue)
        {
            SetRotationAndInvokeCallback(false);

            // this tries to fix a bug where sometimes an
            // attack animation gets triggered even when it should not
            OnRotationComplete?.Invoke();
        }
    }

    private void SetRotationAndInvokeCallback(bool isRotating)
    {
        _isRotating = isRotating;
        OnRotationToTargetSet?.Invoke(_isRotating);
    }

    private void FindClosestTargetAndLockTarget(Transform other)
    {
        if (!CanLookForTarget())
        {
            return;
        }

        var targets = Physics.OverlapSphere(Transform.position + ColliderOffset, OverlapSphereRadius, EnemyLayerMask);

        var closestIndex = FindClosestTargetIndex(ref targets);

        var targetObject = targets[closestIndex].gameObject;
        Target = targetObject.transform;

        targetObject.GetComponentInParent<Entity>().OnDeath += SetTargetLockStop;

        IsLockedToTarget = true;
    }

    private bool CanLookForTarget()
    {
        return !_isMoving && !IsLockedToTarget && !IsAttacking;
    }

    private void SetTargetLockStop()
    {
        // prevent locking on the enemy just killed, after one frame
        // its colliders won't be around anymore
        StartCoroutine(WaitOneFrameAndSetLockStop());
    }

    private IEnumerator WaitOneFrameAndSetLockStop()
    {
        yield return 0;
        IsLockedToTarget = false;
    }

    private int FindClosestTargetIndex(ref Collider[] targets)
    {
        float minSquaredDistance = Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < targets.Length; ++i)
        {
            var colliderTransform = targets[i].gameObject.transform;

            var squaredDistance = CalculateSquaredDistance(Transform, colliderTransform);

            if (!(squaredDistance <= minSquaredDistance)) continue;

            minSquaredDistance = squaredDistance;
            closestIndex = i;
        }

        return closestIndex;
    }

    private float CalculateSquaredDistance(Transform t1, Transform t2)
    {
        return Mathf.Pow(t1.position.x - t2.position.x, 2) +
               Mathf.Pow(t1.position.z - t2.position.z, 2);
    }

    void OnDestroy()
    {
        PlayerInput.OnMouseDragged -= SetPlayerMoving;
        PlayerInput.OnMouseUnclicked -= SetPlayerStopped;
    }
}