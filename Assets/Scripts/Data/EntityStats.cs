using UnityEngine;

[CreateAssetMenu(fileName = "EntityStats", menuName = "ScriptableObjects/EntityStats", order = 1)]
public class EntityStats : ScriptableObject
{
    public int Health;
    public float Speed;
    public float Range;
    public float RotationSpeed;
}