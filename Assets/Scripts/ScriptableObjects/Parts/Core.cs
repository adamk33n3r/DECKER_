using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parts/Core")]
public class Core : ScriptableObject
{
    public Company company;
    public CoreType type;
    public IntegerReference baseHP;
    public FloatReference HPScale;
}
