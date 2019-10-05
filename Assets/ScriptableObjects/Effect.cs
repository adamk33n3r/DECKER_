using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    DOT,
    HOT,
    Weaken,
    Strengthen,
}

public enum Target
{
    Single,
    All,
    Self,
}

public class Effect : ScriptableObject
{
    [HideInInspector]
    public EffectType type;
    [Range(0, 10)]
    public int amount = 1;
    public int time = 3;
}
