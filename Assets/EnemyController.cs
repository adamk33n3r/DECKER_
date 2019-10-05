using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ActiveEffect
{
    public Effect effect;
    public int timer;
}

public class EnemyController : MonoBehaviour
{
    public event Action OnUpdate = () => { };
    public List<ActiveEffect> activeEffects = new List<ActiveEffect>();

    public int damage = 1;
    public int damageMod = 0;
    public int health = 10;

    public void ProcessEffects()
    {
        this.damageMod = 0;
        foreach (var activeEffect in this.activeEffects)
        {
            switch (activeEffect.effect.type)
            {
                case EffectType.DOT:
                    this.health -= activeEffect.effect.amount;
                    break;
                case EffectType.HOT:
                    this.health += activeEffect.effect.amount;
                    break;
                case EffectType.Strengthen:
                    this.damageMod += activeEffect.effect.amount;
                    break;
                case EffectType.Weaken:
                    this.damageMod -= activeEffect.effect.amount;
                    break;
            }
            activeEffect.timer--;
        }
        this.activeEffects.RemoveAll((effect) => effect.timer <= 0);
        if (this.health < 0)
            this.health = 0;
        OnUpdate();
    }

    public void DoAction()
    {
        Debug.Log("Enemy: HIYA!");
        GameManager.Instance.DoActionOnPlayer(this.damage + this.damageMod);
        OnUpdate();
    }

    public void AddEffects(List<Effect> effects)
    {
        foreach (var effect in effects)
        {
            this.activeEffects.Add(new ActiveEffect { effect = effect, timer = effect.time });
        }
    }

    public void TakeDamage()
    {
        OnUpdate();
    }

}
