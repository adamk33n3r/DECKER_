using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ModuleList : ScriptableObject
{
    public List<Module> modules = new List<Module>();
}
