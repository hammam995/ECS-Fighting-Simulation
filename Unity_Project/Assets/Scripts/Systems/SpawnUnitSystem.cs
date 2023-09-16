using System;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    /// <summary>
    /// This system is responsible for spawning of the enitites in the world.
    /// According to the spawn point data the position is located and an entity is 
    /// spawned in that position
    /// Each time the enemy is changed all the spawned units are destroyed and new ones 
    /// are replace. 
    /// </summary>
    public partial class SpawnUnitSystem : SystemBase
    {
        private GameData gameManagementData;
        private GameConfig allTeamConfigs;

        protected override void OnCreate() //Allocate a resource for the system , no assigning values , like start , it called once
        {
            base.OnCreate(); // if not using it the things not working well , because we want to keep using the Base of the System
            RequireForUpdate<GameData>(); // from using the ISystamBase  because OnCreate() is not Dynamic Updated , so we need to have the last data
        }
        protected override void OnStartRunning() // before the actual run or the final update , we initiate and installing the things
        {
            // Initialization , preparing the resources we need , assigning values for our behaviour
            InstantiateAllTeam();
        }

        private void InstantiateAllTeam()
        {
            // for each Game Entity have Game Data
            Entities.ForEach((GameData gameManager) =>
            {
                gameManagementData = gameManager; // making the Actual Game Data taking the value in the ForEach
                allTeamConfigs = gameManagementData.allTeamConfigs; // to Acces the ScriptableObject in the Data we have
            }).WithoutBurst().Run(); // not making the Unity do it's Loop iteraion
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp); // locating and creating Command Buffer , because we are using Dynamic Data

            foreach (var spawnPointData in SystemAPI.Query<SpawnPointData>()) // to acces all the Query of the Data we already have
            {
                var eachUnitConfig = spawnPointData.isPlayer ? allTeamConfigs.ContainPlayerPosition(spawnPointData.positionIndex) : allTeamConfigs.ContainsEnemyPosition(spawnPointData.positionIndex); // to set the position , in the Grid
                if (eachUnitConfig != null) // if the unity is not empty
                {
                    var unitEntity = EntityManager.Instantiate(spawnPointData.isPlayer?gameManagementData.playerEnitity :gameManagementData.enemyEnitity); // from the previous line when we want to creat the Entity of the Object is the Bridge of the Prefab
                    SystemAPI.SetComponent(unitEntity, new LocalTransform
                    {
                        // we use systeAPI , because we already have the data
                        Position = spawnPointData.tranformPosition,
                        Scale = 1,
                        Rotation = Quaternion.identity,
                    }); // all the previous for the entity to be sett and to be putting on the Grid
                    ecb = SetComponentBuffer(ecb, spawnPointData, eachUnitConfig, unitEntity);
                }
            };
            ecb.Playback(EntityManager); //  Playback the recorded commands to apply changes
        }


        // creating Customized Function to do what we want in the Player and the Enemy
        private EntityCommandBuffer SetComponentBuffer(EntityCommandBuffer ecb, SpawnPointData spawnPointData, EachUnitConfig eachUnitConfig, Entity unitEntity)
        {

            // here every Unit will have it's own Data thats why we create new everytime

            ecb.AddComponent(unitEntity, new EnemyFinderData { }); // for the Target
            var componentData = new UnitData  
            {
                isPlayer = spawnPointData.isPlayer,
                healthPoints = eachUnitConfig.HealthPoint,
                movementSpeed = eachUnitConfig.MovementSpeed
            };
            var healthUIGO = GameObject.Instantiate(allTeamConfigs.healthTextPrefab); // creating the health text of the UI 
            var healthData = new HealthData 
            {
                healthUITransform = healthUIGO.transform,
                healthText = healthUIGO.GetComponent<TextMeshPro>(), // but not using it only taking the Text , but not TextMeshPro , it will be in the main
            };
            healthData.healthUITransform.position = spawnPointData.tranformPosition + new float3(0, 1, 0); // making the position on the top a little bit for visualization
            healthData.healthText.text = $"{eachUnitConfig.HealthPoint}"; //  interpolated strings
            var attackData = new AttackData 
            {
                attack = eachUnitConfig.attack,
                attackSpeed = eachUnitConfig.attackSpeed,
                attackRange = eachUnitConfig.attackRange
            };
            // adding the previous componnent data we did
            ecb.AddComponent(unitEntity, healthData);
            ecb.AddComponent(unitEntity, componentData);
            ecb.AddComponent(unitEntity, attackData);
            return ecb; // at the end we return it
        }

        protected override void OnUpdate() // critical part of the system's lifecycle to put the things in the main thread
        {
            if (gameManagementData.ResetUnits) // in the resiting sitiuation , it happens in the clicking (Enemies number button) , GameOver ,Start in the Button , in the Hud Script and the GameSystem
            {
                var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp); // we will use Dynamic data function
                // passing by refrence , passing by value == copy
                // for each object entity we want to do this
                Entities.ForEach((ref Entity entity, in HealthData healthData) =>
                    {
                        ecb.DestroyEntity(entity); // destroy the entity
                        GameObject.Destroy(healthData.healthUITransform.gameObject); // destroying the UI
                    }).WithoutBurst().Run();
                gameManagementData.ResetUnits = false; // make it false we finish what we want to do 
                ecb.Playback(EntityManager); // is command to apply the changes we want to do
                InstantiateAllTeam();
            }

        }
    }

}
