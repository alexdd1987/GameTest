using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RotateEntityTowardsTarget : MonoBehaviour
{
    protected RangeCollisions RangeCollisions;
    protected AnimationEvents AnimationEvents;

    protected Vector3 ColliderOffset;
    protected Vector3 DirectionToTarget;
    protected Quaternion LookRotation;

    protected Transform Transform;
    protected Transform Target;

    protected bool IsLockedToTarget;
    protected bool IsAttacking;
    protected float OverlapSphereRadius;

    private float _rotationSpeed;

    protected virtual void Awake()
    {
        Transform = transform;

        RangeCollisions = GetComponentInChildren<RangeCollisions>();
        AnimationEvents = GetComponentInChildren<AnimationEvents>();

        GetComponentInParent<Entity>().OnEntityParamsSet += SetEntityRotationSpeed;
    }

    protected void SetOverlapSphereRadius(float range)
    {
        OverlapSphereRadius = range * Transform.GetChild(0).transform.localScale.x;
    }

    private void SetEntityRotationSpeed(Entity.EntityParams entityParams)
    {
        _rotationSpeed = entityParams.RotationSpeed;
    }

    protected void RotateTowardsTarget()
    {
        DirectionToTarget = Target.position - Transform.position;
        DirectionToTarget.y = transform.position.y;
        LookRotation = Quaternion.LookRotation(DirectionToTarget.normalized);

        transform.rotation = Quaternion.RotateTowards(Transform.rotation, LookRotation, _rotationSpeed * Time.deltaTime);
    }
}