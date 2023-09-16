using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    /// <summary>
    /// This system is responsible to handle the movement of each unit.
    /// As per the target for a unit determine in Enemy finder system
    ///, that target is used and its location is located and each unit 
    ///moves towards that target.
    ///This class is also responsible for deleting any enitites
    /// </summary>
    /// 
    [UpdateAfter(typeof(EnemyFinderSystem))]

    public partial class PlayerMovementSystem : SystemBase
    {
        private GameData _gameData;
        protected override void OnCreate() // on awake
        {
            base.OnCreate();
            RequireForUpdate<GameData>(); // the reqired dara 
        }
        protected override void OnStartRunning() // start
        {
            Entities.ForEach((in GameData gameData) =>
            {
                _gameData = gameData; // the data we want to use
            }).WithoutBurst().Run(); // dont iteratethe unity lope
        }

        protected override void OnUpdate() // 2 Entities , the target and ourself
        {
           
            // Start movement on game start , when we press the start button it will activated from there
            if (_gameData.gameStart)
            {
                float delta = SystemAPI.Time.DeltaTime; // creating the Deltatime variable
                var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp); // creating the Buffer for Dynamic strucre
                Entities.ForEach((ref Entity entity,ref UnitData unitComponent, in HealthData healthData) =>
                {
                    // passing them by refrence except the health data , because we only want to change the movement
                    if (unitComponent.healthPoints > 0) // check for every unit the health
                    {
                        // Move to the target unit:
                        // entity is the bridge of the object in normal herachy
                        var targetFinder = SystemAPI.GetComponent<EnemyFinderData>(entity); // by declaring the current target from the entity
                        var attackData = SystemAPI.GetComponent<AttackData>(entity); // by declaring the Attack data
                        if (targetFinder.target != Entity.Null) // if it not null then we are indicating to and target
                        {
                            var targetTranslation = SystemAPI.GetComponent<LocalTransform>(targetFinder.target); // target cordination
                            var localTransform = SystemAPI.GetComponent<LocalTransform>(entity); // ourselve cordination
                            var dist = math.distance(targetTranslation.Position, localTransform.Position); // distance
                            if (dist > attackData.attackRange) // moving every axis seperatly to the target , depending on the Attack range ,snipping it
                            {
                                if (targetTranslation.Position.x > localTransform.Position.x)
                                {
                                    localTransform.Position.x += unitComponent.movementSpeed * delta;
                                }
                                else
                                {
                                    localTransform.Position.x -= unitComponent.movementSpeed * delta;
                                }

                                if (targetTranslation.Position.z > localTransform.Position.z)
                                {
                                    localTransform.Position.z += unitComponent.movementSpeed * delta;
                                }
                                else
                                {
                                    localTransform.Position.z -= unitComponent.movementSpeed * delta;
                                }

                                healthData.healthUITransform.position = localTransform.Position + new float3(0, 1, 0); // updating the health position , it will be always positioned in Y==1
                                SystemAPI.SetComponent(entity, localTransform); // confirming the change
                            }
                        }
                    }
                    else  // the unit componnent not alive
                    {
                        //destroy the things
                        GameObject.Destroy(healthData.healthUITransform.gameObject);
                        ecb.DestroyEntity(entity);
                    }
                }
            ).WithoutBurst().Run(); // for the forEach, and not making unity iterate the loop and optimized
            ecb.Playback(EntityManager); // apply the changes by using the buffer and put it in the front thread
            }
        }

    }

}
