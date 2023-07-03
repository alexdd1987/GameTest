using UnityEngine;

public class UpdateWeaponModel : MonoBehaviour
{
    private GameObject _modelObject;

    void Awake()
    {
        GameManager.Instance.Spawner.OnHeroSpawned += SubscribeToWeaponEquipped;
    }

    private void SubscribeToWeaponEquipped(Transform hero)
    {
        hero.GetComponentInParent<WeaponInventory>().OnWeaponEquipped += UpdateHudWeaponFrameWithEquippedWeapon;
    }

    private void UpdateHudWeaponFrameWithEquippedWeapon(WeaponData data)
    {
        if (_modelObject)
        {
            Destroy(_modelObject);
        }

        GameManager.Instance.PrintLog("SHOWING A NICE WEAPON OF TYPE " + data.WeaponPrefab.name);

        _modelObject = Instantiate(data.WeaponPrefab, transform, false);
        transform.localPosition = data.WeaponFramePosition;
    }
}