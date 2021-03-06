﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

[System.Serializable]
public class ActiveEffect
{
    public Effect effect;
    public int amount;
    public int timer;
}

public class EnemyController : MonoBehaviour
{
    public List<ActiveEffect> activeEffects = new List<ActiveEffect>();
    public Enemy enemyData;
    public Inventory inventory;
    public IntegerVariable playerLevel;
    public UnityEvent DamageEvent;
    public UnityEvent DeathEvent;

    public int damageMod = 0;
    public int health = 10;
    public int guard = 0;
    public bool scry = false;
    public bool stunned = false;
    public bool pierce = false;

    public event Action OnUpdate = () => { };

    private void Start()
    {
        this.inventory.deck.Clear();
        foreach (var card in this.enemyData.startingInventory.deck) {
            this.inventory.deck.Add(card);
        }

        this.health = this.enemyData.startingHealth;
        this.name = this.enemyData.name;
    }

    public void ProcessEffects()
    {
        foreach (var activeEffect in this.activeEffects)
            activeEffect.timer--;
        this.activeEffects.RemoveAll((effect) => effect.timer < 0);

        this.damageMod = 0;
        this.scry = false;
        this.guard = 0;
        foreach (var activeEffect in this.activeEffects)
        {
            switch (activeEffect.effect.name)
            {
                case "DOT":
                    this.health -= activeEffect.amount;
                    DamageEvent.Invoke();
                    break;
                case "Guard":
                    this.guard += activeEffect.amount;
                    break;
                case "HOT":
                    this.health += activeEffect.amount;
                    DamageEvent.Invoke();
                    break;
                case "Scry":
                    this.scry = true;
                    break;
                case "Strengthen":
                    this.damageMod += activeEffect.amount;
                    break;
                case "Stun":
                    this.stunned = true;
                    break;
                case "Weaken":
                    this.damageMod -= activeEffect.amount;
                    break;
                case "DOT+":
                    this.health -= activeEffect.amount;
                    DamageEvent.Invoke();
                    activeEffect.amount++;
                    break;
            }
        }
        if (this.health <= 0)
        {
            //this.health = 0;
            DeathEvent.Invoke();
        }
        OnUpdate();
    }

    public void DoAction()
    {
        if (!this.stunned)
        {
            //if (this.inventory.deck.Count > 0)
            //{
            //    int rand = UnityEngine.Random.Range(0, this.inventory.deck.Count);
            //    var cardToPlay = this.inventory.deck[rand];
            //    //if (this.scry) {
            //    Debug.Log("They are playing this card!!!!!");
            //    Debug.Log(cardToPlay);
            //    //}
            //    Debug.Log("Enemy: HIYA!");
            //    GameManager.Instance.PlayEnemyCard(this, cardToPlay);
            //}

            // Make card
            Debug.Log(string.Format("Enemy is level: {0}", this.playerLevel.Value + this.enemyData.levelMod));
            var cards = this.enemyData.pool.GetCards(this.playerLevel.Value + this.enemyData.levelMod);
            // Just add the first one
            var firstCard = cards.FirstOrDefault();
            if (firstCard != null)
            {
                //this.inventory.deck.Add(firstCard);
                if (this.scry)
                {
                    Debug.Log("They are playing this card!!!!!");
                    Debug.Log(firstCard);
                    //GameManager.Instance.Log(string.Format("", firstCard.name));
                }
                GameManager.Instance.PlayEnemyCard(this, firstCard);
            }
        }
        this.stunned = false;
        OnUpdate();
    }

    public void AddEffects(IEnumerable<CardEffect> effects)
    {
        foreach (var cardEffect in effects)
        {
            this.activeEffects.Add(new ActiveEffect { effect = cardEffect.effect, amount = cardEffect.amount, timer = cardEffect.time });
        }
    }

    public void TakeDamage(int damage, bool pierce)
    {
        Debug.Log("enemy taking damage: " + damage);
        Debug.Log("has pierce: " + pierce);
        int modDamage = damage - (pierce ? 0 : this.guard);
        int totalDamage = Mathf.Clamp(modDamage, 0, modDamage);
        this.health -= totalDamage;
        if (this.health <= 0)
        {
            //this.health = 0;
            DeathEvent.Invoke();
        }

        OnUpdate();
    }

}
