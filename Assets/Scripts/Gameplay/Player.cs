using UnityEngine;

public class Player : Entity
{
    private bool _ignoreInput;
    private float _currentSpeed;

    protected override void Awake()
    {
        base.Awake();
        
        AttackAudioSource = GetComponent<AudioSource>();
        GetComponentInChildren<AnimationEvents>().OnHeroWeaponSwing += PlayWeaponSwingSound;

        GetComponentInChildren<BumpCollisions>().OnBumpCollisionEvent += ApplyDamage;
        GetComponent<RotateHeroTowardsTarget>().OnRotationToTargetSet += SetIgnoreInput;
        GetComponentInChildren<WeaponInventory>().OnWeaponEquipped += SetRangeAndSpeedAndMoveSoundPitch;

        PlayerInput.OnMouseClicked += StopMoveAudioSource;
        PlayerInput.OnMouseDragged += SetDirectionAndRotation;
        PlayerInput.OnMouseUnclicked += ResetDirection;
    }

    private void StopMoveAudioSource(Vector3 clickPosition)
    {
        if (MoveAudioSource)
        {
            MoveAudioSource.Stop();
        }
    }

    private void PlayWeaponSwingSound()
    {
        if (AttackAudioSource.isPlaying) return;
        AttackAudioSource.Play();
    }

    private void SetRangeAndSpeedAndMoveSoundPitch(WeaponData newWeaponData)
    {
        RangeCollider.radius = MyEntityParams.Range * newWeaponData.Range;
        _currentSpeed = MyEntityParams.Speed / newWeaponData.Weight;

        MoveAudioSource.pitch = newWeaponData.MovePitch;
    }

    private void SetIgnoreInput(bool isRotating)
    {
        _ignoreInput = isRotating;

        if (!isRotating)
        {
            Rotation = Transform.rotation;
        }
    }

    private void SetDirectionAndRotation(Vector3 direction)
    {
        ProjectOnXZ(ref direction);
        Direction = direction.normalized;

        Rotation = Quaternion.LookRotation(Direction).normalized;

        if (MoveAudioSource.isPlaying) return;
        MoveAudioSource.Play();
    }

    private void ProjectOnXZ(ref Vector3 direction)
    {
        direction.x = -direction.x;
        direction.z = -direction.y;
        direction.y = 0f;
    }

    private void ResetDirection()
    {
        Direction = Vector3.zero;
        MoveAudioSource.Stop();
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.HasGameStarted) return;

        if (!CanMove()) return;

        RigidBody.velocity = Direction * _currentSpeed;
        RigidBody.MoveRotation(Rotation.normalized);
    }

    private bool CanMove()
    {
        return !_ignoreInput && GameManager.Instance.HasGameStarted;
    }

    protected override void ApplyDamage(int damage)
    {
        base.ApplyDamage(damage);
        if (MyEntityParams.Health > 0) return;

        _ignoreInput = true;
        RigidBody.velocity = Vector3.zero;
    }

    void OnDestroy()
    {
        PlayerInput.OnMouseDragged -= SetDirectionAndRotation;
        PlayerInput.OnMouseUnclicked -= ResetDirection;
    }
}