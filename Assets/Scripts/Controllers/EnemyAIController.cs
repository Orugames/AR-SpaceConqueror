using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{

    public List<PlanetView> planets = new List<PlanetView>();
    public List<PlanetView> enemyPlanets = new List<PlanetView>();
    public List<PlanetView> nonEnemyPlanets = new List<PlanetView>();

    public List<GameObject> planetsGO = new List<GameObject>();

    public GameObject allPlanetsContainer;

    public float AITurnTime = 10;

    public bool battleStarted;
    public bool AIDecisionMakingRunning;

    [Range(1, 2)]
    public float playerPlanetDecisionWeight;
    [Range(0.5f, 2)]
    public float neutralPlanetDecisionWeight;

    public bool initAIController;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (battleStarted && !initAIController)
        {
            initAIController = true;
            AIDecisionMakingRunning = true;

            UpdatePlanetsList();

            StartCoroutine(EnemyAITurn());
        }
    }
    public IEnumerator EnemyAITurn()
    {

        while (AIDecisionMakingRunning || enemyPlanets.Count != 0)
        {

            //Find my strongest & weakest Planet
            PlanetView enemyWeakestPlanet = null;
            PlanetView enemyStrongestPlanet = null;

            GetStrongestAndWeakestPlanet(ref enemyWeakestPlanet, ref enemyStrongestPlanet, true);

            //Then find the strongest & weakest friendly or neutral planet with best growth rate
            PlanetView playerWeakestPlanet = null;
            PlanetView playerStrongestPlanet = null;

            GetStrongestAndWeakestPlanet(ref playerWeakestPlanet, ref playerStrongestPlanet, false);

            // Check if any of the enemyAI planets should be defended
            bool defendAIPlanet = CheckIfAIShouldDefendPlanet(enemyWeakestPlanet, enemyStrongestPlanet, playerStrongestPlanet);

            // If defend order is given, iterate the next decision
            if (defendAIPlanet)
            {
                List<PlanetView> sourceAsList = new List<PlanetView>();
                sourceAsList.Add(enemyStrongestPlanet);

                BattleManager.instance.SendShips(sourceAsList, enemyWeakestPlanet, false);
                yield return new WaitForSeconds(AITurnTime);

                continue;

            }
            // Check if attacking with one planet to another if the decision is valuable

            bool attackSingleTarget = CheckIfAIShouldAttackPlanet(enemyStrongestPlanet, playerWeakestPlanet);

            if (attackSingleTarget)
            {
                Debug.Log("Attack one order given");

                List<PlanetView> sourceAsList = new List<PlanetView>();
                sourceAsList.Add(enemyStrongestPlanet);

                BattleManager.instance.SendShips(sourceAsList, playerWeakestPlanet, false);
            }
            else
            {


                // Placeholder, get another enemy planet with enough score to help the strongest one
                foreach (PlanetView planetView in enemyPlanets)
                {
                    // Get one different to the strongest
                    if (planetView != enemyStrongestPlanet)
                    {
                        if (playerWeakestPlanet == null || enemyStrongestPlanet == null)
                        {
                            //yield return new WaitForSeconds(AITurnTime);

                            continue;
                        }

                        if (planetView.planetData.score + enemyStrongestPlanet.planetData.score > playerWeakestPlanet.planetData.score)
                        {
                            Debug.Log("Attack multiple order given");

                            List<PlanetView> sourceAsList = new List<PlanetView>();
                            sourceAsList.Add(enemyStrongestPlanet);
                            sourceAsList.Add(planetView);

                            BattleManager.instance.SendShips(sourceAsList, playerWeakestPlanet, false);
                        }
                    }
                }


                  
            }

            // Check if not if it can conquer a planet with multiple planets

            // Finally take the decision to attack



            yield return new WaitForSeconds(AITurnTime);

        }

    }

    private bool CheckIfAIShouldAttackPlanet(PlanetView enemyStrongestPlanet, PlanetView playerWeakestPlanet)
    {
        bool shouldAttack = false;

        if (playerWeakestPlanet != null && enemyStrongestPlanet != null)
        {
            Debug.Log(playerWeakestPlanet.name + enemyStrongestPlanet.name);
            // If we pass the minimun requirements, send the order to attack
            if ((enemyStrongestPlanet.planetData.score > playerWeakestPlanet.planetData.score * playerPlanetDecisionWeight && playerWeakestPlanet.planetData.playerControlled) ||
                (enemyStrongestPlanet.planetData.score > playerWeakestPlanet.planetData.score * neutralPlanetDecisionWeight && !playerWeakestPlanet.planetData.playerControlled))
            {
                shouldAttack = true;
            }
        }
        return shouldAttack;
    }

    private bool CheckIfAIShouldDefendPlanet(PlanetView enemyWeakestPlanet, PlanetView enemyStrongestPlanet, PlanetView playerStrongestPlanet)
    {
        bool shouldDefend = false;

        if (playerStrongestPlanet == null || enemyWeakestPlanet == null || enemyStrongestPlanet == null)
        {
            return shouldDefend;
        }

        if(playerStrongestPlanet.planetData.score > 2 * enemyWeakestPlanet.planetData.score && 
           enemyStrongestPlanet.planetData.score + enemyWeakestPlanet.planetData.score > 2 * enemyWeakestPlanet.planetData.score &&
           Vector3.Distance(enemyWeakestPlanet.transform.position,playerStrongestPlanet.transform.position) < 
           Vector3.Distance(enemyStrongestPlanet.transform.position, playerStrongestPlanet.transform.position))
        {
            shouldDefend = true;
        }

        return shouldDefend;
    }

    private void GetStrongestAndWeakestPlanet(ref PlanetView weakestPlanet,ref PlanetView strongestPlanet, bool getEnemyPlanets)
    {
        float weakestScore = 0;
        float enemyStrongestScore = 0;

        List<PlanetView> planetsToCheck;

        if (getEnemyPlanets)
        {
            planetsToCheck = enemyPlanets;
        }
        else
        {
            planetsToCheck = nonEnemyPlanets;
        }

        // Find my strongest planet
        foreach (PlanetView planetView in planetsToCheck)
        {
            // Get the score of the planet relative to the current power and the growth rate 
            //
            //    x                1
            // -------   =   --------------
            // x + 1/Gr      1 + 1/(Gr * x)
            //
            float score = 1 / (1 + (1 / (planetView.planetData.growthRate * planetView.planetData.score)));

            // if this new planet is better, chose it
            if (score > enemyStrongestScore)
            {
                enemyStrongestScore = score;
                strongestPlanet = planetView;
            }
            // Get the weaker one. Get the weaker than prev one but also add it if we dont have any
            if (score < weakestScore || weakestScore == 0)
            {
                weakestScore = score;
                weakestPlanet = planetView;
            }
        }
    }

    // Method used to update the list used by AI to take decisions
    public void UpdatePlanetsList()
    {
        // get a list of every planet ingame
        List<PlanetView> numberOfPlanets = new List<PlanetView>(allPlanetsContainer.GetComponentsInChildren<PlanetView>());

        // Get planets flagged as enemyPlanets and not as
        foreach (PlanetView planetView in numberOfPlanets)
        {
            // enemy planets
            if (planetView.planetData.enemyControlled && !enemyPlanets.Contains(planetView))
            {
                enemyPlanets.Add(planetView);
            }

            // neutral and friendly ones
            if (!planetView.planetData.enemyControlled && !nonEnemyPlanets.Contains(planetView))
            {
                nonEnemyPlanets.Add(planetView);
            }
        }

        // Remove all planets that are not from  the enemy on the list of planets
        for (int i = enemyPlanets.Count - 1; i >= 0; i--)
        {
            if (!enemyPlanets[i].planetData.enemyControlled) enemyPlanets.RemoveAt(i);
        }

        // Remove all planets that are not from  the non enemy on the list of planets
        for (int i = nonEnemyPlanets.Count - 1; i >= 0; i--)
        {
            if (nonEnemyPlanets[i].planetData.enemyControlled) nonEnemyPlanets.RemoveAt(i);
        }
    }
}
