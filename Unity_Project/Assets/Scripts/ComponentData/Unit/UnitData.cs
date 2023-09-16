using Unity.Entities;
using UnityEngine;
///Done
/// <summary>
/// This hold unit basic component such as HP speed is a player or not
/// </summary>
public struct UnitData : IComponentData
{
    public float healthPoints;
    public float movementSpeed;
    public bool isPlayer;
    public bool gotHit;
}

/// <summary>
/// This holds player attack info
/// </summary>
public struct AttackData : IComponentData
{
    public float attack;
    public float attackSpeed;
    public float attackRange;
}

/// <summary>
/// This holds unit current target entity
/// </summary>
public struct EnemyFinderData : IComponentData
{
    public Entity target;
}

