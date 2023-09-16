using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
///Done
/// <summary>
/// This holds unit spawning data
/// </summary>
public struct SpawnPointData :  IComponentData
{
    public bool isPlayer;
    public float3 tranformPosition;
    public EPosition positionIndex;
}

/// <summary>
/// A managed component class to hold nullable data.
/// TextMeshPro is still not compatible with ECS subscene 
/// and thus is necessary to add it to manage data.
/// </summary>
public class HealthData : IComponentData
{
    public Transform healthUITransform;
    public TextMeshPro healthText;
}