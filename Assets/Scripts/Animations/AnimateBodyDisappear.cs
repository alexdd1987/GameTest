using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateBodyDisappear : MonoBehaviour
{
    public Action<Vector3> OnBodyDisappeared;
    [SerializeField] private float _speed;

    private WaitForSeconds _waitForOneSecond;
    private Transform _transform;

    private const float Units = 5f;

    void Awake()
    {
        GetComponentInChildren<AnimationEvents>().OnEnemyDieAnimationEvent += MakeBodyDisappear;

        _waitForOneSecond = new WaitForSeconds(1f);
        _transform = transform;
    }

    private void MakeBodyDisappear(bool hasEnded)
    {
        if (hasEnded)
        {
            StartCoroutine(MoveObjectDownSlowly(Units));
        }
    }

    private IEnumerator MoveObjectDownSlowly(float units)
    {
        yield return _waitForOneSecond;

        var deathPosition = _transform.position;
        var endPosition = _transform.position + Vector3.down * units;

        while (_transform.position.y > endPosition.y)
        {
            _transform.position += Vector3.down * units * Time.deltaTime * _speed;

            yield return null;
        }

        Destroy(gameObject);

        OnBodyDisappeared?.Invoke(deathPosition);
    }
}
