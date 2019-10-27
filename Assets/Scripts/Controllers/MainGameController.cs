using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates { mainMenu, positioningCenter, preBattle, battle, postBattle }
public class MainGameController : MonoBehaviour
{
    public GameStates currentGameState = GameStates.mainMenu;

    public LevelGenerator levelGenerator;
    public PlayerController playerController;
    public EnemyAIController enemyAIController;

    public LevelData levelData1;


    public int currentLevel = 0;
    public bool currentLevelInitialized;
    public bool currentLevelStarted;

    public static MainGameController instance = null;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        // If on MainMenu
        if(currentGameState == GameStates.mainMenu)
        {
            // Check for inputs on canvas
        }
        else if (currentGameState == GameStates.positioningCenter)
        {
            // Check for inputs on canvas
        }
        else if (currentGameState == GameStates.preBattle)
        {
            // Check for inputs on canvas
        }
        else if (currentGameState == GameStates.battle)
        {
            // If battle has not been started initialize it
            if (!currentLevelInitialized)
            {
                // Init level reading from our levelData
                levelGenerator.GenerateLevel(levelData1);

                currentLevelInitialized = true;
            }

            enemyAIController.battleStarted = true;
            playerController.battleStarted = true;


            // Check for inputs in PlayerController
        }else if (currentGameState == GameStates.postBattle)
        {
            // Check for inputs on canvas
        }
    }

    public void ChangeGameState(GameStates sentGameState)
    {
        currentGameState = sentGameState;
    }
    public void StartBattle()
    {
        currentGameState = GameStates.battle;
    }
}
