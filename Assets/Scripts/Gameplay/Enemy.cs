using System;
using UnityEngine;

public class Enemy : Entity
{
    public Action<EnemyWeaponStats> OnWeaponSet;

    [SerializeField] private EnemyWeaponStats _weaponStats;

    private Weapon _weapon;
    private Transform _target;

    private bool _shouldMove;
    private bool _isAttacking;
    private bool _isDamaged;
    private bool _isDead;

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.Spawner.OnEnemyKilled += IncreaseDamage;

        GetComponent<RotateEnemyTowardsTarget>().OnTargetLock += SetTargetToFollow;
        GetComponent<EnemyAnimator>().OnEnemyMoving += SetEnemyMoving;

        var animationEvents = GetComponentInChildren<AnimationEvents>();

        animationEvents.OnEnemyAttackAnimationEvent += SetIsAttacking;
        animationEvents.OnEnemyDamageAnimationEvent += SetIsDamaged;
        animationEvents.OnEnemyDieAnimationEvent += SetIsDead;

        _weapon = GetComponentInChildren<Weapon>(true);
        _weapon.DamageDealt = _weaponStats.Damage;
        GetComponentInChildren<AttackRangeCollisions>().transform.localScale *= _weaponStats.AttackRangeMultiplier;
    }

    protected override void Start()
    {
        base.Start();
        OnWeaponSet?.Invoke(_weaponStats);
    }

    private void SetEnemyMoving(bool isMoving)
    {
        _shouldMove = isMoving;
        ActivateMoveAudioSource(isMoving);
    }
    private void ActivateMoveAudioSource(bool isMoving)
    {
        if (!MoveAudioSource.isPlaying && isMoving)
        {
            MoveAudioSource.Play();
            return;
        }

        if (!isMoving)
        {
            MoveAudioSource.Stop();
        }
    }

    private void SetIsAttacking(bool isAttacking)
    {
        _isAttacking = isAttacking;

        if (_isAttacking)
        {
            AttackAudioSource.Play();
        }
    }

    private void SetIsDamaged(bool isDamaged)
    {
        _isDamaged = isDamaged;

        // stop attacking state if attacking animation
        // gets interrupted by damage animation
        _isAttacking = false;
    }

    private void SetIsDead(bool isDead)
    {
        _isDead = true;

        ActivateMoveAudioSource(false);
    }

    private void IncreaseDamage()
    {
        _weapon.DamageDealt += 1;
    }

    private void SetTargetToFollow(Transform target)
    {
        _target = target;
        _shouldMove = target != null;
    }

    void Update()
    {
        if (!CanMove()) return;

        Direction = _target.position - Transform.position;
        Direction.y = 0f;
        transform.position += Direction.normalized * Time.deltaTime * MyEntityParams.Speed;
    }

    private bool CanMove()
    {
        return _shouldMove && !_isAttacking &&
               !_isDamaged && !_isDead &&
               _target != null && GameManager.Instance.HasGameStarted;
    }
}
