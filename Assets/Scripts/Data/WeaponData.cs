using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]

public class WeaponData : ScriptableObject
{
    // Instantiation
    public GameObject WeaponPrefab;
    public Vector3 WeaponColliderPosition;
    public Vector3 WeaponColliderScale;

    // Stats
    public float Weight;
    public float Range;
    public float Cooldown;
    public int Damage;

    // WeaponDrop
    public int  DropPercentage;
    public float  DropPositionOffset;
    public float  DropTiltAngle;

    // Hud
    public Vector3 WeaponFramePosition;

    //Audio
    public float MovePitch;
}