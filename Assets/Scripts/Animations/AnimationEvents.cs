using System;

using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public Action<bool> OnHeroAttackAnimationEvent;
    public Action<bool> OnHeroDamageAnimationEvent;
    public Action<bool> OnEnemyAttackAnimationEvent;
    public Action<bool> OnEnemyDamageAnimationEvent;
    public Action<bool> OnEnemyDieAnimationEvent;
    public Action OnHeroDieAnimationEnded;
    public Action OnHeroWeaponSwing;

    private void HeroAttackEvent(int hasStarted)
    {

        OnHeroAttackAnimationEvent?.Invoke(Convert.ToBoolean(hasStarted));
    }

    private void HeroDamageEvent(int hasStarted)
    {

        OnHeroDamageAnimationEvent?.Invoke(Convert.ToBoolean(hasStarted));
    }

    private void EnemyAttackAnimationEvent(int hasStarted)
    {
        OnEnemyAttackAnimationEvent?.Invoke(Convert.ToBoolean(hasStarted));
    }

    private void EnemyDamageAnimationEvent(int hasStarted)
    {
        OnEnemyDamageAnimationEvent?.Invoke(Convert.ToBoolean(hasStarted));
    }

    private void EnemyDieAnimationEvent(int hasStarted)
    {
        OnEnemyDieAnimationEvent?.Invoke(Convert.ToBoolean(hasStarted));
    }

    private void HeroWeaponSwing()
    {
        OnHeroWeaponSwing?.Invoke();
    }

    private void HeroDieEvent()
    {
        OnHeroDieAnimationEnded?.Invoke();
    }
}
