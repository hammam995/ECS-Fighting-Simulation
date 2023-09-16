using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Done
// Scriptable object of the Unit will hav List of EachUnitConfig class

[CreateAssetMenu(fileName ="Team Config", menuName = "Config/Team Config", order = 1)]
public class TeamConfig : ScriptableObject
{
    public  List<EachUnitConfig> allUnitConfigList = new List<EachUnitConfig>();  
}

[Serializable]
public class EachUnitConfig
{
    public float HealthPoint;
    public float attack;
    public float attackSpeed;
    public float attackRange;
    public float MovementSpeed;
    public EPosition ePosition;
}
