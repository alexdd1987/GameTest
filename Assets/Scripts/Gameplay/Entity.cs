using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public struct EntityParams
    {
        public int Health;
        public float Speed;
        public float Range;
        public float RotationSpeed;
    }

    public Action OnDeath;
    public Action<EntityParams> OnEntityParamsSet;
    public Action<int> OnHealthUpdated;

    [SerializeField] protected SphereCollider RangeCollider;
    [SerializeField] protected AudioSource AttackAudioSource;
    [SerializeField] protected AudioSource MoveAudioSource;

    [SerializeField] private EntityStats _stats;


    protected Transform Transform;
    protected Rigidbody RigidBody;
    protected Vector3 Direction;
    protected Quaternion Rotation;

    protected HitCollisions HitCollisions;
    protected RangeCollisions RangeCollisions;
    protected EntityParams MyEntityParams;

    protected virtual void Awake()
    {
        Transform = transform;
        RigidBody = GetComponent<Rigidbody>();

        HitCollisions = GetComponentInChildren<HitCollisions>();
        RangeCollisions = GetComponentInChildren<RangeCollisions>();
        HitCollisions.OnHitTriggerEvent += ApplyDamage;

        MyEntityParams = new EntityParams
        {
            Health = _stats.Health,
            Range = _stats.Range,
            RotationSpeed = _stats.RotationSpeed,
            Speed = _stats.Speed
        };

        RangeCollider.radius = _stats.Range;
    }

    protected virtual void Start()
    {
        OnEntityParamsSet?.Invoke(MyEntityParams);
    }

    protected virtual void ApplyDamage(int damage)
    {
        MyEntityParams.Health -= damage;

        OnHealthUpdated?.Invoke(Mathf.Max(MyEntityParams.Health, 0));

        if (MyEntityParams.Health > 0) return;

        OnDeath?.Invoke();
    }
}