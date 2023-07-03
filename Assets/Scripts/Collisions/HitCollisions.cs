using System;
using UnityEngine;

public class HitCollisions : MonoBehaviour
{
    public Action<int> OnHitTriggerEvent;

    private Weapon _weapon;

    void OnTriggerEnter(Collider other)
    {
        if (!_weapon)
        {
            _weapon = other.GetComponent<Weapon>();
        }

        OnHitTriggerEvent?.Invoke(_weapon.DamageDealt);
    }
}
