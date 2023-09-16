using System.Collections.Generic;
using Unity.Assertions;
using UnityEngine;

[CreateAssetMenu(fileName = "All Team Configs", menuName = "Database/All Team Configs", order = 1)]
public class GameConfig : ScriptableObject
{
    /// <summary>
    /// Team Config in index 0 holds player config.
    /// All the other configs are enemy Configs
    /// To create a new set of enemy you can simply 
    /// create a TeamConfig scriptable Object and 
    /// add that to this list.
    /// </summary>
    public List<TeamConfig> teamConfigs = new List<TeamConfig>(); // the list which have the data of the Unit , so it will be list of list
    public int enemyIndex = 1; // to force the loop to start from the enemy index not the player
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject healthTextPrefab;
    public GameObject teamButton; // related with the Hud to decide the number of buttons , to create the transform


    // to go through all the Player Units Positions , so go to index [0] of the list and acces it , return the position
    public EachUnitConfig ContainPlayerPosition(EPosition position)
    {
        foreach(var eachUnitConfig in this.teamConfigs[0].allUnitConfigList)
        {
            if(eachUnitConfig.ePosition == position)
                return eachUnitConfig;
        }
        return null;
    }

    // to go through all the Enemy Units Positions , the same but we enter from index[1] , because index[0] always for the player will be
    public EachUnitConfig ContainsEnemyPosition(EPosition position)
    {
        foreach (var teamConfig in this.teamConfigs[enemyIndex].allUnitConfigList)
        {
            if (teamConfig.ePosition == position)
                return teamConfig;
        }
        return null;
    }
    public void SetEnemyIndex(int enemyIndex)
    {
        this.enemyIndex = enemyIndex;
    }


    // false. If the condition is true, it will trigger an assertion error with the provided error message
    private void OnValidate()
    {
        Assert.IsFalse(teamConfigs.Count < 2, $"There should be at least two team configs one for player and one for enemy");
    }


}

