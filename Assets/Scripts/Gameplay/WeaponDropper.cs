using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponDropper : MonoBehaviour
{
    [SerializeField] private PlayerWeapons _playerWeapons;
    [SerializeField] private GameObject _weaponDropPrefab;

    private static byte[] _weaponDistribution;

    private GameObject _instantiatedWeapon;

    void Awake()
    {
        GetComponent<AnimateBodyDisappear>().OnBodyDisappeared += RandomlyDropRandomWeapon;

        if (_weaponDistribution == null)
        {
            InitWeaponDistribution();
        }
    }

    // Total weapon distribution is 100%, each weapon has a
    // percent assigned in its data
    private void InitWeaponDistribution()
    {
        _weaponDistribution = new byte[100];

        var count = 0;
        var weapons = _playerWeapons.WeaponData;

        for (byte i = 0; i < weapons.Length; ++i)
        {
            for (var j = 0; j < weapons[i].DropPercentage; ++j)
            {
                _weaponDistribution[count] = i;
                ++count;
            }
        }
    }

    private void RandomlyDropRandomWeapon(Vector3 dropPosition)
    {
        int random = Random.Range(1, 101);

        // in default settings, percentDropRate is 50%
        if (random > _playerWeapons.PercentDropRate) return;

        random = Random.Range(0, 100);

        var randomIdx = _weaponDistribution[random];
        var weaponData = _playerWeapons.WeaponData[randomIdx];

        var weaponDrop = Instantiate(_weaponDropPrefab, dropPosition + Vector3.up * weaponData.DropPositionOffset, Quaternion.identity);
        weaponDrop.GetComponentInChildren<TouchCollisions>().OnWeaponPickedUp += DestroyInstantiatedWeapon;
        
        _instantiatedWeapon = Instantiate(weaponData.WeaponPrefab, weaponDrop.transform);
        _instantiatedWeapon.transform.localEulerAngles = Vector3.right * weaponData.DropTiltAngle;

        weaponDrop.GetComponent<WeaponDrop>().WeaponData = weaponData;
    }

    private void DestroyInstantiatedWeapon()
    {
        Destroy(_instantiatedWeapon);
    }
}
