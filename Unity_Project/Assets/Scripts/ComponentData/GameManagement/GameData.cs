using Unity.Entities;
//Done
public class GameData : IComponentData
{
    public bool gameStart;
    public bool playerWins;
    public bool enemyWins;
    public GameConfig allTeamConfigs;
    public Entity playerEnitity;
    public Entity enemyEnitity;
    public bool ResetUnits;
}