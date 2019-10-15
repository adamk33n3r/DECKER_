using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parts/Motherboard")]
public class Motherboard : ScriptableObject
{
    public Company company;
    public CoreType coreType;
    public IntegerReference moduleBays;
    public IntegerReference power;
}
