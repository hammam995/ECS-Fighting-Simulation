
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

//Done
// it doesnt have GameObject
public class SpawnPointAuthority : MonoBehaviour
{
    public bool isPlayer;
    public EPosition ePositions;
}

public class SpawnPointBaker : Baker<SpawnPointAuthority>
{
    public override void Bake(SpawnPointAuthority authoring)
    {
        // becaus there is not GameObjec and is only using data to add it to an existing GameObject
        AddComponent(GetEntity(authoring, TransformUsageFlags.WorldSpace), new SpawnPointData
        {
            isPlayer = authoring.isPlayer,
            tranformPosition = authoring.transform.position,
            positionIndex = authoring.ePositions,
        });
      // all the data taken from the SpawnData
    }
}
