using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SetFollowHero : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.Spawner.OnHeroSpawned += SetHeroToFollow;
    }

    void SetHeroToFollow(Transform hero)
    {
        GetComponent<CinemachineVirtualCamera>().Follow = hero;
    }
}
