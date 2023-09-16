using Unity.Entities;
using UnityEngine;
//Done
public class GameManagementAuthority : MonoBehaviour
{
    public GameConfig allConfigs;
}

public class GameManagementBaker : Baker<GameManagementAuthority>
{
    public override void Bake(GameManagementAuthority authoring)
    {
        // because is converting from GameObject inside the ScriptableObject taking it from monobehaviour , and using the GameData
        AddComponentObject(GetEntity(authoring, TransformUsageFlags.None), new GameData
        {       
            gameStart = false,
            allTeamConfigs = authoring.allConfigs,
            playerEnitity = GetEntity(authoring.allConfigs.playerPrefab, TransformUsageFlags.Dynamic),
            enemyEnitity = GetEntity(authoring.allConfigs.enemyPrefab, TransformUsageFlags.Dynamic),
        });
    }
}