using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn
{
    Player,
    Enemies,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //public event Action<EnemyController> OnEnemyAction = (enemy) => { };
    public event Action OnActionOnPlayer = () => { };

    public IntegerVariable maxPlayerHP;
    public IntegerVariable playerHP;

    public Turn WhichTurn = Turn.Player;

    private void Awake()
    {
        Instance = this;
        playerHP.Value = maxPlayerHP.Value;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
    }

    public void ProcessCard(Card card, EnemyController enemy)
    {
        if (WhichTurn == Turn.Enemies)
            return;

        Debug.Log(card.name);
        if (enemy == null)
        {
            enemy = FindObjectOfType<EnemyController>();
        }
        // TODO: Add resistances
        enemy.health -= card.physicalDamage;
        enemy.health -= card.magicDamage;
        if (enemy.health < 0)
            enemy.health = 0;
        enemy.AddEffects(card.effects);
        Debug.Log(enemy.name);
        enemy.TakeDamage();

        WhichTurn = Turn.Enemies;
    }

    public void ProcessEnemies()
    {
        if (WhichTurn == Turn.Player)
            return;

        var enemies = FindObjectsOfType<EnemyController>();
        foreach (var enemy in enemies)
        {
            enemy.ProcessEffects();
            if (enemy.health <= 0)
                continue;
            enemy.DoAction();
            //OnEnemyAction(enemy);
        }

        WhichTurn = Turn.Player;
    }

    public void DoActionOnPlayer(int damage)
    {
        Debug.Log("enemy did damage: " + damage);
        this.playerHP.Value -= damage;
        OnActionOnPlayer();
    }
}
