using UnityEngine;

public class DestroyAfterTimeout : MonoBehaviour
{
    [SerializeField] private float _timeout;

    private float _spawnTime;

    void Awake()
    {
        _spawnTime = Time.time;
    }

    void Update()
    {
        if (!HasTimerExpired()) return;

        Destroy(gameObject);
    }

    private bool HasTimerExpired()
    {
        return Time.time - _spawnTime > _timeout;
    }
}
