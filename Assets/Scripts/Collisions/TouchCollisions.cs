using System;
using UnityEngine;

public class TouchCollisions : MonoBehaviour
{
    public Action OnWeaponPickedUp;

    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        _audioSource.Play();

        var weaponInventory = other.gameObject.GetComponentInParent<WeaponInventory>();
        var weaponDrop = GetComponentInParent<WeaponDrop>();

        weaponInventory.SetNewEquippedWeapon(weaponDrop.WeaponData);
        
        Destroy(GetComponent<BoxCollider>());

        OnWeaponPickedUp?.Invoke();
    }
}
