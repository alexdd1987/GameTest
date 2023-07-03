using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    private Transform _transform;
    public WeaponData WeaponData { get; set; }

    void Awake()
    {
        _transform = transform;
    }

    void Update()
    {
        transform.Rotate(transform.up, 0.1f);
    }

}
