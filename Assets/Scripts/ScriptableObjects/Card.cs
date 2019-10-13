using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Target
{
    Enemy,
    Self,
}

[Serializable]
public class CardEffect
{
    public Effect effect;
    public Target target;
    public int amount = 1;
    public int time = 1;
}

[CreateAssetMenu()]
public class Card : ScriptableObject
{
    public int physicalDamage = 0;

    [Range(1, 5)]
    public int level = 1;

    public CardType type;

    public List<CardEffect> effects = new List<CardEffect>();

    public override string ToString()
    {
        return this.name;
    }
}
