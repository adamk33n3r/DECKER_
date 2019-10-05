using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Card : ScriptableObject
{
    public int physicalDamage = 1;
    public int magicDamage = 0;

    public List<Effect> effects;
}
