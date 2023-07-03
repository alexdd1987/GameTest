using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    private Transform _transform;

    void Awake()
    {
        _transform = transform;
    }

    void Update()
    {
        _transform.position = _target.position + _offset;
    }
}
