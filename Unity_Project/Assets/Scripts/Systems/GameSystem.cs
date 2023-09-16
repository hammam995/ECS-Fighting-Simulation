using System;
using Unity.Entities;
namespace Systems
{
    /// <summary>
    /// This system is resposible to hanlde all the game logics
    /// Since all our system are derived from system base a
    ///  managed component "GameData" that hold reference to config and 
    ///  other variable is used and according to change in its value the 
    ///  system reflect changes.
    ///  For instance a boolean is use to determine game start
    /// </summary>
    public partial class GameSystem : SystemBase
    {
        private GameData _gameData; // to have from it the bool condition

        public event Action<bool> GameOverEvent; // Action delegate Invoke

        protected override void OnCreate() // when we start creating it it must be in the begenning like we are doing start
        {
            base.OnCreate();
            RequireForUpdate<GameData>();
        }
        protected override void OnStartRunning() // before the run start we make the system not iterate the loop
        {
            Entities.ForEach((in GameData gameData) =>
            {
                _gameData = gameData;
            }).WithoutBurst().Run();
        }
        protected override void OnUpdate() // in the main thread actual update
        {
            if (!_gameData.gameStart) return; // if it was start then we exit from the condition dont continue the iteration and the sequence of the code       
            if (_gameData.playerWins) // if we won
            {
                _gameData.playerWins = false;
                GameOver(true);
            }
            if (_gameData.enemyWins) // if enemy won
            {
                _gameData.enemyWins = false;
                GameOver(false);
            }
        }

        private void GameOver(bool playerWim) // we make the opposit of the condition
        {
            _gameData.gameStart = false;
            GameOverEvent?.Invoke(playerWim);
        }

        public void StartGame() // when we start the game
        {
            _gameData.gameStart = true;
        }

        public void ChangeEnemy(int enemyIndex) // taking the index , so from it we increase or not increasing the ScrollView
        {
            _gameData.allTeamConfigs.SetEnemyIndex(enemyIndex);
            ResetUnit();
        }

        public void ResetUnit() // resetting enemies
        {
            _gameData.ResetUnits = true;
        }
    }
}
