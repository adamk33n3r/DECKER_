using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Enemy : ScriptableObject
{
    public Inventory startingInventory;
    public CardPool pool;

    [Range(-5, 5)]
    public int levelMod;

    public int startingHealth = 20;
}
