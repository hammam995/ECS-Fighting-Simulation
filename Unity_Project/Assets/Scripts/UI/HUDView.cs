using System;
using System.Collections;
using System.Collections.Generic;
using Systems;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Controll the View of the UI , Appearing and dissappearing

namespace UI
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button mainMenuButton; // when we click it we will swap between it and Start
        [SerializeField] private GameObject gameOverGO;
        [SerializeField] private TextMeshProUGUI gameOverText;
        [SerializeField] private Transform parent;
        [SerializeField] private GameConfig gameConfig; // to take from it the number of buttons we will create in the HUD se we created in the scriptable object
        private GameSystem gameManagerSystem; // connecting the GameSystem to update the HUD View
        private void Awake() // before the start , when we playe the Game we assign the things to the Listinners depending on the buttons we are clicking
        {
            gameManagerSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GameSystem>();
            // to obtain the reference to a specific system instance of the GameSystem we created
            gameManagerSystem.GameOverEvent += GameOver; // add it to the Action
            startButton.onClick.AddListener(StartGame); // when we click on the Start Button
            mainMenuButton.onClick.AddListener(RestartGame); // add it to the Action
            SetTeamButton();
        }

        private void RestartGame() // to connect the things between the scripts
        {
           gameManagerSystem.ResetUnit();
            gameOverGO.SetActive(false); // make Game Over Button Dissapear
            startButton.gameObject.SetActive(true); // make the start Button Appear
        }

        private void SetTeamButton()
        {
            int totalEnemies = gameConfig.teamConfigs.Count;// to take the count of the enemies team in the configuration
           for(int i = 1; i < totalEnemies; i++) // we start from 1 because always zero for the player , so the loop count how many team we have to create the buttons
            {
                Transform enemyButton = Instantiate(gameConfig.teamButton).transform; // we take the game object from the scriptable object
                enemyButton.parent = parent; // to take the paren transform in the UI
                enemyButton.localPosition = Vector3.zero; // is relative to the parent position
                enemyButton.localScale = Vector3.one; // 1 in all dimensions. to be appear otherwise if we didnt put it the player will not see it
                enemyButton.GetComponent<UnitButton>().UpdateIndex(i); // to update the last index of the scroll View in the UI
            }
        }

        public void ChangeEnemyTeam(int enemyIndex) // so from it we change the enemy team and make it acces to the Configuration 
        {
           gameManagerSystem.ChangeEnemy(enemyIndex);
        }
        private void OnDestroy() // before we destroy the monobehaviour script in case of winnig or loosing , or in start gaming
        {
            // so we are cleaning and doing undo
            startButton.onClick.RemoveListener(StartGame);
            mainMenuButton.onClick.RemoveListener(RestartGame);
            gameManagerSystem.GameOverEvent -= GameOver;
        }

        private void StartGame() // connectiong the things with GameSystem
        {
            gameManagerSystem.StartGame(); // true
            startButton.gameObject.SetActive(false);
        }

        private void GameOver(bool playerWon)
        {
            gameOverGO.SetActive(true);      
            gameOverText.text = playerWon ? "YOU WIN !!" : "ENEMY WIN";           
        }

        private void Update()
        {
        }
    }
}
