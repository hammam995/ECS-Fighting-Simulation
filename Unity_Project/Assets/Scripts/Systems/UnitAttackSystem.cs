using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    /// <summary>
    /// This system is responsible to hanlde all the attacks towards unit
    /// base of the unit configs value.
    /// Each unit attacks once per second.
    /// </summary>
    public partial class UnitAttackSystem : SystemBase
    {
        private GameData gameManagement;
        private float time = 1f;

        protected override void OnCreate() // on Awake
        {
            base.OnCreate();
            RequireForUpdate<GameData>();
        }
        protected override void OnStartRunning() // on Start
        {
            Entities.ForEach((in GameData gameManager) =>
            {
                gameManagement = gameManager;
            }).WithoutBurst().Run();
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((ref Entity entity, ref AttackData attackData, in HealthData healthData) =>
            {
                if (gameManagement.gameStart)
                {
                    var targetFinder = SystemAPI.GetComponent<EnemyFinderData>(entity);
                    if (targetFinder.target != Entity.Null) // to avoid shooting error
                    {
                        var playerData = SystemAPI.GetComponent<UnitData>(entity); // from the entity we want to retrive
                        if (playerData.gotHit) // if the entity hit
                        {
                            healthData.healthText.text = $"{playerData.healthPoints}"; // to create interpolated strings
                            playerData.gotHit = false; // resetting it
                        }
                        //Get Required Component
                        var target = targetFinder.target;
                        var targetTranslation = SystemAPI.GetComponent<LocalTransform>(target);
                        var translation = SystemAPI.GetComponent<LocalTransform>(entity);
                        var dist = math.distance(targetTranslation.Position, translation.Position);
                    
                        // Attack if target is in range and cooldown timer is finished:
                        DeductDataFromUnit(ref attackData,ref target, dist);
                    }
                }
            }).WithStructuralChanges().Run();
        }

        private void DeductDataFromUnit(ref AttackData attackData,ref Entity target, float dist)
        {
            if (dist < attackData.attackRange)
            {
                if (time  > 0)
                {
                    time -= SystemAPI.Time.DeltaTime * attackData.attackSpeed;
                }
                else // reset
                {
                    var targetData = SystemAPI.GetComponent<UnitData>(target);
                    targetData.healthPoints -= attackData.attack;
                    targetData.gotHit = true;
                    time = attackData.attackSpeed;
                    SystemAPI.SetComponent(target, targetData);
                }
            }
        }       
    }
}
