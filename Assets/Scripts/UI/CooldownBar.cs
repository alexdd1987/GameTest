using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    [SerializeField] private Slider _fillSlider;

    private float _cooldownTime;
    
    void Awake()
    {
        GameManager.Instance.Spawner.OnHeroSpawned += SubscribeToWeaponEquippedAndAttackStarted;
    }

    private void SubscribeToWeaponEquippedAndAttackStarted(Transform hero)
    {
        hero.GetComponentInParent<WeaponInventory>().OnWeaponEquipped += UpdateCooldownTime;
        hero.GetComponentInParent<HeroAnimator>().OnAttackStarted += StartCoolDown;
    }

    private void UpdateCooldownTime(WeaponData data)
    {
        _cooldownTime = data.Cooldown;
        _fillSlider.maxValue = _cooldownTime;
        _fillSlider.value = _cooldownTime;
    }

    private void StartCoolDown()
    {
        _fillSlider.value = 0f;
        StartCoroutine(RefillCooldownBar());
    }

    private IEnumerator RefillCooldownBar()
    {
        float time = 0;

        while (time < _cooldownTime)
        {
            time += Time.deltaTime;
            _fillSlider.value = time;

            yield return null;
        }
    }
}
