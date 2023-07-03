using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWeaponStats", menuName = "ScriptableObjects/EnemyWeaponStats", order = 1)]
public class EnemyWeaponStats : ScriptableObject
{
    public int Damage;
    public float AttackRangeMultiplier;
    public float CooldownInSeconds;
}