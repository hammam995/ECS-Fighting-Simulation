using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    // is to choose the current target
    /// <summary>
    /// This class finds the enemy for each unit i.e. for player it
    /// finds an enemy unit and for enemy it finds player unit
    /// </summary>
    public partial class EnemyFinderSystem : SystemBase
    {


        private List<Entity> playerUnits = new List<Entity>();
        private List<Entity> enemyUnits = new List<Entity>();
        private GameData gameManagementData; // because it has the Entities of the previous variables

        protected override void OnCreate() // on Awake
        {
            base.OnCreate();
            RequireForUpdate<GameData>();
        }

        protected override void OnStartRunning() // on Start
        {

            Entities.ForEach((GameData gameData) =>
            {
                gameManagementData = gameData;
            }).WithoutBurst().Run(); // dont make unity optimize looping
        }

        protected override void OnUpdate()
        {
            if (!gameManagementData.gameStart) return; // if it is not start exit
            playerUnits.Clear(); // empty the player list
            enemyUnits.Clear(); // empty the Entity list
            Entities.ForEach((Entity unit, UnitData unitData) =>
            {
                if (unitData.isPlayer) // this from the unit data to decide if the Entity we have is player or Enemy
                {
                    playerUnits.Add(unit); // if yes then added to the player list
                }
                else
                {
                    enemyUnits.Add(unit); // if no then added to the enemy list
                }
            }).WithoutBurst().Run();
            if (playerUnits.Count == 0) return; //  quick optimization to avoid unnecessary processing if there are no player units left to check.
            var allPlayerDead = true;
            var allEnemiesDead = true;

            Entities.ForEach((Entity unit, ref EnemyFinderData unitFinder) =>
            {
                var target = unitFinder.target;

                if (target != Entity.Null)
                {
                    if(target == null)
                    {
                        unitFinder.target = Entity.Null;
                    }
                    else
                    {
                        var targetData = SystemAPI.GetComponent<UnitData>(target); // take the Unit Data from the target Entity
                        if (targetData.healthPoints <= 0) // it means the current target died , so we make it null
                        {
                            unitFinder.target = Entity.Null;
                        }
                    }
                    
                }
                var unitComponent = SystemAPI.GetComponent<UnitData>(unit);

                // to determine if the player or the enemie alive or no

                if (unitComponent.healthPoints > 0 && unitComponent.isPlayer)
                {
                    allPlayerDead = false;
                }
                else if (unitComponent.healthPoints > 0 && !unitComponent.isPlayer )
                {
                    allEnemiesDead = false;
                }
                // Find a random target from the opposite team:
                if (unitFinder.target == Entity.Null)
                {
                    if (unitComponent.isPlayer) // if the entity data variable condition true of the player 
                    {
                        var newTarget = enemyUnits[Random.Range(0, enemyUnits.Count)]; // Enemy Entity
                        var newTargettData = SystemAPI.GetComponent<UnitData>(newTarget);
                        if (newTargettData.healthPoints > 0) // if the target is alive then we assign it
                        {
                            unitFinder.target = newTarget;
                        }
                    }
                    else if (!unitComponent.isPlayer) // the opposite of the above
                    {
                        var newTarget = playerUnits[Random.Range(0, playerUnits.Count)];
                        var newTargettData = SystemAPI.GetComponent<UnitData>(newTarget);
                        if (newTargettData.healthPoints > 0) // enemy targiting the player
                        {
                            unitFinder.target = newTarget;
                        }
                    }
                }

            }).WithoutBurst().Run();

            // the wining booleans for every side

            if (allPlayerDead)
            {
                gameManagementData.enemyWins = true;
            }

            if (allEnemiesDead)
            {
                gameManagementData.playerWins = true;
            }
        }
    }

}
