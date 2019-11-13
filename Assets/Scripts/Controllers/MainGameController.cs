using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates { positioningCenter, mainMenu, levelSelectionScreen, levelBriefing, battle, victoryScreen, defeatScreen }
public class MainGameController : MonoBehaviour
{
    public GameStates currentGameState = GameStates.mainMenu;

    public ARFoundationController aRFoundationController;
    public LevelGenerator levelGenerator;
    public PlayerController playerController;
    public EnemyAIController enemyAIController;
    public BattleManager battleManager;

    public LevelData currentLevelData;

    public LevelsCompletionScriptable levelsCompletedData;



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
       
        if (currentGameState == GameStates.positioningCenter)
        {
            // Check for inputs on canvas
        }
        // If on MainMenu
        else if(currentGameState == GameStates.mainMenu)
        {
            // Check for inputs on canvas
        }
        else if (currentGameState == GameStates.levelSelectionScreen)
        {
            // Level selection logic
            LevelSelectionInput();
        }
        else if (currentGameState == GameStates.levelBriefing)
        {
            // Check for inputs on canvas
        }
        else if (currentGameState == GameStates.battle)
        {
            // If battle has not been started initialize it
            if (!currentLevelInitialized)
            {
                // Init level reading from our levelData
                levelGenerator.GenerateLevel(currentLevelData);

                currentLevelInitialized = true;
            }

            enemyAIController.battleStarted = true;
            playerController.battleStarted = true;

            


            // Check for inputs in PlayerController
        }else if (currentGameState == GameStates.victoryScreen)
        {
            // Disable/Destroy battle Elements

            // Show victory screen
        }
        else if (currentGameState == GameStates.defeatScreen)
        {
            // Disable/Destroy battle Elements

            // Show defeat screen
        }
    }

    // This method is used to select levels based on the hit raycast on the level selection GO
    private void LevelSelectionInput()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            // Raycast & hit elements
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(mRay, out hit))
            {
                // null check & not level selection tag check
                if (hit.collider == null || !hit.collider.CompareTag("LevelSelectionGO"))
                {
                    return;
                }
                LevelData levelDataSelected = hit.collider.GetComponent<LevelDataContainer>().levelData;

                currentLevelData = levelDataSelected;

                // Disable parent GO of levelSelected
                //hit.collider.transform.parent.gameObject.SetActive(false);

                // Start the tween for the mission briefing
                TweensManager.instance.levelBriefingTween.levelBrienfingCanvasGroup.gameObject.SetActive(true);
                TweensManager.instance.levelSelectorTween.HideLevelSelector();
                TweensManager.instance.levelBriefingTween.ShowBriefingScreen();

                // Load the level
                //levelGenerator.GenerateLevel(levelDataSelected);
                //currentLevelInitialized = true;
                currentGameState = GameStates.levelBriefing;


            }
        }

    }

    public void ChangeGameState(GameStates sentGameState)
    {
        currentGameState = sentGameState;
    }
    public void ChangeToLevelSelection()
    {
        currentGameState = GameStates.levelSelectionScreen;
    }
    public void StartBattle()
    {



        currentGameState = GameStates.battle;
    }
}
