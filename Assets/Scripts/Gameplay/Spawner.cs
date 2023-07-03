using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public Action<Transform> OnHeroSpawned;
    public Action<Entity> OnEnemySpawned;
    public Action OnEnemyKilled;


    [SerializeField] private SpawnerData _spawnerData;
    [SerializeField] private Transform _entitiesParent;

    private Transform _heroTransform;
    private Canvas _worldCanvas;

    private Vector3 _spawnPoint;

    private float _lastSpawnTime;
    private int _entitiesCount;
    private int _entitiesKilledCount;

    void Awake()
    {
        _worldCanvas = GetComponentInChildren<Canvas>();
    }
    void Start()
    {
        SpawnHero();

        // Spawn first enemy with some delay
        _lastSpawnTime = -_spawnerData.SpawnIntervalInSeconds + _spawnerData.InitialDelayInSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSpawnEnemy())
        {
            SpawnEnemy();
        }
    }

    private bool CanSpawnEnemy()
    {
        return Time.time - _lastSpawnTime > _spawnerData.SpawnIntervalInSeconds &&
               _entitiesCount < _spawnerData.MaxConcurrentEntities &&
               GameManager.Instance.HasGameStarted;
    }

    private void SpawnHero()
    {
        var hero = Instantiate(_spawnerData.SpawnedHero, Vector3.zero, Quaternion.identity, _entitiesParent);
        _heroTransform = hero.transform;

        SetHealthBar(hero);
    
        OnHeroSpawned?.Invoke(_heroTransform);
    }

    private void SpawnEnemy()
    {
        GenerateRandomSpawnPointInCircle(_heroTransform.position, _spawnerData.SpawnAreaRadius, out _spawnPoint);

        var enemy = Instantiate(_spawnerData.SpawnedEntity, _spawnPoint, Quaternion.identity, _entitiesParent);
        enemy.GetComponentInChildren<Weapon>(true).DamageDealt += _entitiesKilledCount;
        
        SetHealthBar(enemy);
        
        var entity = enemy.GetComponent<Entity>();
        entity.OnDeath += UpdateEntitiesCountersAndRallyBees;

        _lastSpawnTime = Time.time;

        ++_entitiesCount;

        OnEnemySpawned?.Invoke(entity);
    }

    private void UpdateEntitiesCountersAndRallyBees()
    {
        --_entitiesCount;
        ++_entitiesKilledCount;

        OnEnemyKilled?.Invoke();
    }

    private void SetHealthBar(GameObject spawnedEntity)
    {
        var healthBarTransform = spawnedEntity.GetComponentInChildren<HealthBar>().transform;

        healthBarTransform.SetParent(_worldCanvas.transform);
        healthBarTransform.localScale = Vector3.one;
        healthBarTransform.localRotation = Quaternion.identity;
    }

    private void GenerateRandomSpawnPointInCircle(Vector3 origin, float radius, out Vector3 spawnPoint)
    {
        float randomAngle = Random.value * 2f * Mathf.PI;
        float randomRadius = radius * Mathf.Sqrt(Random.value) + _spawnerData.OffsetFromHero;

        float x = randomRadius * Mathf.Cos(randomAngle);
        float z = randomRadius * Mathf.Sin(randomAngle);

        spawnPoint.x = origin.x + x;
        spawnPoint.y = 0;
        spawnPoint.z = origin.z + z;
    }
}