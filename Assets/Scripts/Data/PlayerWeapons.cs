using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeapons", menuName = "ScriptableObjects/PlayerWeapons", order = 1)]
public class PlayerWeapons : ScriptableObject
{
    public WeaponData[] WeaponData;
    public int PercentDropRate;
}