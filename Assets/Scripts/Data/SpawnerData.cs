using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerData", menuName = "ScriptableObjects/SpawnerData", order = 1)]
public class SpawnerData : ScriptableObject
{
    public float SpawnAreaRadius;
    public float SpawnIntervalInSeconds;
    public int MaxConcurrentEntities;

    public GameObject SpawnedEntity;
    public GameObject SpawnedHero;
    
    public float OffsetFromHero;
    public float InitialDelayInSeconds;
}