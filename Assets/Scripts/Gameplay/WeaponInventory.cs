using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponInventory : MonoBehaviour
{
    private struct EquippedWeapon
    {
        public GameObject GameObject;
        public WeaponData Data;
    }

    public Action<WeaponData> OnWeaponEquipped;

    [SerializeField] private PlayerWeapons _playerWeapons;
    [SerializeField] private Transform _weaponTransform;

    private EquippedWeapon _equippedWeapon;

    void Start()
    {
        InstantiateRandomWeapon();
    }

    private void InstantiateRandomWeapon()
    {
        int weaponIndex = Random.Range(0, _playerWeapons.WeaponData.Length);

        _equippedWeapon.Data = _playerWeapons.WeaponData[weaponIndex];
        _equippedWeapon.GameObject = Instantiate(_equippedWeapon.Data.WeaponPrefab, _weaponTransform, false);

        SetCollider(_equippedWeapon.Data);

        OnWeaponEquipped?.Invoke(_equippedWeapon.Data);
    }

    private void SetCollider(WeaponData weaponData)
    {
        var weaponColliderObject = GetComponentInChildren<BoxCollider>(true).gameObject;

        weaponColliderObject.transform.localPosition = _equippedWeapon.Data.WeaponColliderPosition;
        weaponColliderObject.transform.localScale = _equippedWeapon.Data.WeaponColliderScale;
        weaponColliderObject.GetComponent<Weapon>().DamageDealt = weaponData.Damage;
    }

    public void SetNewEquippedWeapon(WeaponData weaponData)
    {
        Destroy(_equippedWeapon.GameObject);

        _equippedWeapon.Data = weaponData;
        _equippedWeapon.GameObject = Instantiate(_equippedWeapon.Data.WeaponPrefab, _weaponTransform, false);
        
        SetCollider(_equippedWeapon.Data);

        OnWeaponEquipped?.Invoke(_equippedWeapon.Data);
    }
}